using FSC.DataLayer;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Linq;

namespace FSC.Moduls.Printing.Invoice
{
    public class InvoiceGenerator : DocumentGenerator
    {
        private ApplicationDbContext applicationDB;
        private GeneratorInvoices generatorInvoices;
        private int orderId = 0;
        public InvoiceGenerator()
        {
            DateOfInvoice = DateTime.Now;
            generatorInvoices = new GeneratorInvoices(DateOfInvoice, DocumentTypeEnum.Invoice.ToString());
            applicationDB = new ApplicationDbContext();
        }
        public override void Generate(int orderId)
        {
            this.orderId = orderId;
            InvoiceNumber = generatorInvoices.GenerateInvoiceNumber();
            PDFFile = generatePDF();
            InvoiceDocument = createInvoiceDbDocument();
        }

        private byte[] generatePDF()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 88f, 88f, 10f, 10f);
                Font NormalFont = FontFactory.GetFont("Arial", 12, BaseColor.BLUE);
                //PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + InvoiceNumber.Replace("/", "-") + ".pdf",
                //     FileMode.Create));
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
                doc.Open();
                doc.AddTitle(InvoiceNumber);
                doc.AddSubject("Incoice " + InvoiceNumber);
                doc.AddCreator("FSC 0.1");

                var tableDoc = addHead();
                addContent(tableDoc);
                addFoot(tableDoc);

                doc.Add(tableDoc);
                doc.Close();
                byte[] bytes = ms.ToArray();
                ms.Close();
                return bytes;
            }
        }

        private InvoiceDocument createInvoiceDbDocument()
        {
            var doc = new InvoiceDocument();
            doc.DateOfInvoice = DateOfInvoice;
            doc.InvoiceNmuber = "Faktura " + InvoiceNumber;
            doc.FileName = "Faktura " + InvoiceNumber + ".pdf";
            doc.File = PDFFile;
            return doc;
        }

        private PdfPTable addHead()
        {
            PdfPTable tableLayout = new PdfPTable(3);
            float[] headers = { 200, 20, 200 };
            tableLayout.SetWidths(headers);
            tableLayout.WidthPercentage = 100;
            var seller = "Sprzedawca:\nMoja firma\nAdres\nNIP\nkonto :8745 854 864";

            PdfPCell cellSeller = new PdfPCell(new Phrase(seller));
            cellSeller.HorizontalAlignment = Element.ALIGN_LEFT;
            tableLayout.AddCell(cellSeller);
            tableLayout.AddCell(" ");

            PdfPCell cellInoice = new PdfPCell(new Phrase("Faktura VAT " + InvoiceNumber + "\n Orginał"));
            cellInoice.HorizontalAlignment = Element.ALIGN_RIGHT;
            tableLayout.AddCell(cellInoice);
            
            var order = applicationDB.Orders.SingleOrDefault(x => x.OrderId == orderId);
            var customer = "Nabywca:\n" + order.Customer.CompanyName + "\n" + order.Customer.NIP + "\n"
                + order.Customer.City + " " + order.Customer.Address;

            PdfPCell cellCustomer = new PdfPCell(new Phrase(customer));
            cellCustomer.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            tableLayout.AddCell(cellCustomer);
            tableLayout.AddCell(" ");
            var additionalData = "Forma płatności: przelew\n Data sprzedaży: " + order.OrderDateTime.ToShortDateString();
            PdfPCell cellAdditionalData = new PdfPCell(new Phrase(additionalData));
            cellAdditionalData.HorizontalAlignment = Element.ALIGN_RIGHT;
            tableLayout.AddCell(cellAdditionalData);

            return tableLayout;
        }

        private void addContent(PdfPTable glowne)
        {
            PdfPCell cell = new PdfPCell(midlle());
            cell.Colspan = 3;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            glowne.AddCell(cell);
        }

        private PdfPTable midlle()
        {
            var tableMidlle = new PdfPTable(6);

            AddCellToHeader(tableMidlle, "Nazwa Usługi");
            AddCellToHeader(tableMidlle, "Ilość");
            AddCellToHeader(tableMidlle, "Netto");
            AddCellToHeader(tableMidlle, "VAT");
            AddCellToHeader(tableMidlle, "Kwota VAT");
            AddCellToHeader(tableMidlle, "Brutto");
            tableMidlle.CompleteRow();

            var order = applicationDB.Orders.FirstOrDefault(p => p.OrderId == orderId);
            foreach (var emp in order.OrderItems)
            {
                AddCellToBody(tableMidlle, emp.ServiceItemName, Element.ALIGN_LEFT);
                AddCellToBody(tableMidlle, emp.Quantity.ToString());
                AddCellToBody(tableMidlle, emp.Rate.ToString());
                AddCellToBody(tableMidlle, emp.VAT.ToString());
                var amount = emp.Rate * emp.Quantity * (emp.VAT / 100);
                var amountRound = Math.Round(amount * 100) / 100;
                AddCellToBody(tableMidlle, amountRound.ToString("0.##"));

                var brutto = amountRound + (emp.Rate * emp.Quantity);
                var bruttoRound = Math.Round(brutto * 100) / 100;
                AddCellToBody(tableMidlle, bruttoRound.ToString("0.##"));
            }
            var amountSum = Math.Round(order.OrderItems.Sum(x => x.Rate * x.Quantity * (x.VAT / 100)) * 100) / 100;
            var bruttoSum = Math.Round((amountSum + order.OrderItems.Sum(x => x.Rate * x.Quantity)) * 100) / 100;

            tableMidlle.CompleteRow();
            AddCellToBody(tableMidlle, "Razem:", Element.ALIGN_CENTER);
            AddCellToBody(tableMidlle, " ");
            AddCellToBody(tableMidlle, " ");
            AddCellToBody(tableMidlle, " ");
            AddCellToBody(tableMidlle, amountSum.ToString("0.##"));
            AddCellToBody(tableMidlle, bruttoSum.ToString("0.##"));
            tableMidlle.CompleteRow();
            return tableMidlle;
        }

        private void addFoot(PdfPTable tableDocument)
        {
            PdfPTable tableFoot = new PdfPTable(3);
            float[] headers = { 200, 80, 200 };
            tableFoot.SetWidths(headers);
            tableFoot.WidthPercentage = 100;

            PdfPCell cell = new PdfPCell(new Phrase("Podpis osoby uprawnionej do wystawienia faktury", new Font(Font.FontFamily.HELVETICA, 6, 1, iTextSharp.text.BaseColor.BLACK)));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_BOTTOM;
            cell.FixedHeight = 50f;
            tableFoot.AddCell(cell);

            cell.Phrase = new Phrase("Data odbioru", new Font(Font.FontFamily.HELVETICA, 6, 1, iTextSharp.text.BaseColor.BLACK));
            tableFoot.AddCell(cell);
            cell.Phrase = new Phrase("Podpis osoby uprawnionej do odbioru faktury", new Font(Font.FontFamily.HELVETICA, 6, 1, iTextSharp.text.BaseColor.BLACK));
            tableFoot.AddCell(cell);

            PdfPCell cellFoot = new PdfPCell(tableFoot);
            cellFoot.Colspan = 3;
            cellFoot.HorizontalAlignment = Element.ALIGN_CENTER;
            tableDocument.AddCell(cellFoot);
        }

        private void AddCellToHeader(PdfPTable tableLayout, string cellText, int align = Element.ALIGN_CENTER)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.GRAY)))
            {
                HorizontalAlignment = align,
                Padding = 5,
                BackgroundColor = new iTextSharp.text.BaseColor(128, 0, 0)
            });
        }
        private void AddCellToBody(PdfPTable tableLayout, string cellText, int align = Element.ALIGN_RIGHT)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK)))
            {
                HorizontalAlignment = align,
                Padding = 5,
                BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255)
            });
        }
    }
}