using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FSC.ViewModels.Api
{
    public class CreateChecklistVM
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public int ParentId { get; set; }
    }
}