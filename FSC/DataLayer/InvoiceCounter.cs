namespace FSC.DataLayer
{
    public class InvoiceCounter
    {
        public int Id { get; set; }
        public string DocumentType { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public int Counter { get; set; }
    }
}