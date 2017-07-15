using FSC.Moduls.Printing.Invoice;

namespace FSC.Moduls.Printing
{
    public static class DocumentGeneratorFactory
    {
        public static DocumentGenerator GetGenerator(DocumentTypeEnum documentType)
        {
            switch (documentType)
            {
                case DocumentTypeEnum.Invoice:
                        return new InvoiceGenerator();
                //case DocumentTypeEnum.InvoiceCorrection:
                //        return new InvoiceCorrectionGenereator();
                default:
                        return null;
            }
        }
    }
}