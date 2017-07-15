using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FSC.DataLayer
{
    public class InvoiceDocument
    {
        public int Id { get; set; }
        public string InvoiceNmuber { get; set; }
        public int WorkOrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
        public int CustomerId { get; set; }
        public DateTime DateOfInvoice { get; set; }
        public string FileName { get; set; }
        public byte[] File { get; set; }
    }
}