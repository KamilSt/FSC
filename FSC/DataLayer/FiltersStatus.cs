using FSC.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FSC.DataLayer
{
    public class FiltersStatus
    {
        public int Id { get; set; }
        public string FilterName { get; set; }
        public int Version { get; set; }
        public string Filters { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
    }
}