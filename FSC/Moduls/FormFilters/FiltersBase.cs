using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSC.Moduls.FormFilters
{
    public class FiltersBase<T> : IFilter
    {
        public string key { get; set; }
        public string label { get; set; }
        public T value { get; set; }
        public string controlType { get; set; }
        public int order { get; set; }
        public bool visible { get; set; }
        //  required;
    }
    public interface IFilter { }
}