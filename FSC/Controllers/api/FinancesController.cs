using FSC.DataLayer;
using FSC.Moduls.SalaryCalculators;
using FSC.ViewModels.Api;
using Microsoft.AspNet.Identity;
using System.Web.Http;

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
    }
}
