using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FSC.DataLayer
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        [Required]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        [Required]
        [Display(Name = "Kod usługi")]
        public string ServiceItemCode { get; set; }
        [Required]
        [Display(Name = "Nazwa usługi")]
        public string ServiceItemName { get; set; }
        [Display(Name = "Ilość")]
        public decimal Quantity { get; set; }
        [Display(Name = "Stawka")]
        public decimal Rate { get; set; }
        [Display(Name = "VAT")]
        public decimal VAT { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal Gross { get; private set; }
    }
}