using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSC.ViewModels.Api
{
    public class CheckListDisplayVM
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public List<CheckListItem> Items { get; set; }

        public CheckListDisplayVM()
        {
            Items = new List<CheckListItem>();

        }
    }
    public class CheckListItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}