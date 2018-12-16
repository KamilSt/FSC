using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FSC.Business;
using FSC.ViewModels.Api;

namespace FSC.Moduls.BusinessEngines.Interface
{
    public interface ICreateInvoicesEngine : IBusinessEngine
    {
        CreatedInvoiceVM CreateIinvoice(int carId);
    }
}