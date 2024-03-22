using DocumentFormat.OpenXml.Wordprocessing;
using Library_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using PageSize = DocumentFormat.OpenXml.Wordprocessing.PageSize;


namespace Library_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ExportPDFController : ControllerBase
	{
		private readonly LibraryContext _libraryContext;

		public ExportPDFController(LibraryContext libraryContext)
		{
			_libraryContext = libraryContext;
		}


		[HttpGet("generatepdf")]
		public async Task<IActionResult> GeneratePDF (string InvoiceNo)
		{
			var document = new PdfDocument();
			string HtmlContent = "<h1>Hello World!</h1>";
			PdfGenerator.AddPdfPages(document, HtmlContent, PdfSharpCore.PageSize.A4); 
			byte[]? response = null;
			using (MemoryStream ms = new MemoryStream())
			{
				document.Save(ms);
				response = ms.ToArray();
			}


			string Filename = "Invoice_" + InvoiceNo + ".pdf";
			return File(response, "application/pdf", Filename);
		}

	}
}
