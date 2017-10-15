using FSC.DataLayer;
using FSC.Moduls.SalaryCalculators;
using FSC.ViewModels.Api;
using Microsoft.AspNet.Identity;
using System.Web.Http;
using System.Linq;

namespace FSC.Controllers.api
{

    public class FinancesController : ApiController
    {
        private ApplicationDbContext applicationDB = null;
        private readonly string userId = null;
        public FinancesController()
        {
            applicationDB = new ApplicationDbContext();
            userId = User.Identity.GetUserId();
        }

        [Route("api/finances/salaryCalculator/")]
        [HttpPost]
        public IHttpActionResult salaryCalculator([FromBody]FormSalaryCalculatorVM value)
        {
            if (value == null)
                return BadRequest();
            var contract = new ContractBuilder()
             .TypeOfContract(value.typeOfContract)
             .Salary(value.salary, value.salaryFrom)
             .HigherCostOfGettingIncome(value.higherCostOfGettingIncome)
             .HealthInsurance(value.healthInsurance)
             .Build();

            var calculator = new SalaryCalculator(contract);
            var result = calculator.Calculator();

            return Ok(result);
        }

        [Route("api/finances/financialDocuments/")]
        [HttpGet]
        public IHttpActionResult faktury()
        {
            var result = applicationDB.InvoiceDocuments
               .GroupBy(g => new
               {
                   year = (g.DateOfInvoice.Year),
                   quarter = ((g.DateOfInvoice.Month - 1) / 3)
               })
               .Select(u => new InvoiceDocumentVM
               {
                   Sum = u.Sum(i => i.Order.OrderItems.Sum(p => p.Quantity * p.Rate)),
                   Year = u.Key.year,
                   Quarter = u.Key.quarter,
                   InvoiceInfo = u.Select(p => new InfoInfoice()
                   {
                       Id = p.Id,
                       CompanyName = p.Order.Customer.CompanyName,
                       InvoiceDate = p.DateOfInvoice,
                       InvoiNumber = p.InvoiceNmuber,
                       Sum = p.Order.OrderItems.Sum(o => o.Quantity * o.Rate)
                   })
               })
               .OrderBy(o => o.Year).ThenBy(k => k.Quarter)
               .ToList();
            return Ok(result);
        }
    }
}
