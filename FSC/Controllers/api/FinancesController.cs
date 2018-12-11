using FSC.DataLayer;
using FSC.Moduls.SalaryCalculators;
using FSC.ViewModels.Api;
using System.Web.Http;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using FSC.Providers.UserProvider.Interface;

namespace FSC.Controllers.api
{

    public class FinancesController : ApiController
    {
        private ApplicationDbContext applicationDB = null;
        private readonly string userId = null;
        public FinancesController(IUserProvider user)
        {
            applicationDB = new ApplicationDbContext();
            userId = user.GuidId;
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
                   Sum = u.Sum(i => i.Order.Total),
                   Year = u.Key.year,
                   Quarter = u.Key.quarter,
                   InvoiceInfo = u.Select(p => new InfoInfoice()
                   {
                       Id = p.Id,
                       CompanyName = p.Order.Customer.CompanyName,
                       InvoiceDate = p.DateOfInvoice,
                       InvoiNumber = p.InvoiceNmuber,
                       Sum = p.Order.Total
                   })
               })
               .OrderBy(o => o.Year).ThenBy(k => k.Quarter)
               .ToList();
            return Ok(result);
        }

        [Route("api/finances/invoice/{id}")]
        [HttpGet]
        public HttpResponseMessage DownloadInvoice(int id)
        {
            if (id == 0)
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            var invoice = applicationDB.InvoiceDocuments.FirstOrDefault(x => x.Id == id);
            if (invoice == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(invoice.File)
            };
            result.Content.Headers.ContentDisposition =
                new ContentDispositionHeaderValue("attachment")
                {
                    FileName = invoice.FileName
                };
            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");

            return result;
        }
    }
}
