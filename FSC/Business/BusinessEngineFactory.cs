using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FSC.Business
{
    public class BusinessEngineFactory : IBusinessEngineFactory
    {
        T IBusinessEngineFactory.GetBusinessEngine<T>()
        {
            return DependencyResolver.Current.GetService<T>();
        }
    }

    public interface IBusinessEngineFactory
    {
        T GetBusinessEngine<T>() where T : IBusinessEngine;
    }

    public interface IBusinessEngine
    {

    }
}