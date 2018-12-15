using FSC.DataLayer;
using FSC.Moduls.FormFilters.DataFiltering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSC.Moduls.FormFilters
{
    public static class DataFilteringFactory
    {
        public const string FilterFor_OrderListFilter = "OrderListFilter";

        public static IQueryable<T> GetFor<T>(string filterName, IQueryable<T> list, string filters)
        {
            switch (filterName)
            {
                case FilterFor_OrderListFilter:
                    {
                        return OrderListDataFiltering.Filter<Order>(list.Cast<Order>(), filters).Cast<T>();
                    }
            }
            return null;
        }
    }
}