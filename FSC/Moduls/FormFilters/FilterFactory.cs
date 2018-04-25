using FSC.Moduls.FormFilters.FilterTemplates;

namespace FSC.Moduls.FormFilters
{
    public static class FilterFactory
    {
        public const string FilterFor_OrderListFilter = "OrderListFilter";

        public static FilterResponse GetFilter(string filterName)
        {
            switch (filterName)
            {
                case FilterFor_OrderListFilter:
                    {
                        return OrderListFilter.GetFilter();
                    }
            }
            return null;
        }
    }
}