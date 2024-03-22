using ClosedXML.Excel;
using Library_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Library_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ExportExcelController : ControllerBase
	{
		private readonly LibraryContext _libraryContext;

		public ExportExcelController(LibraryContext libraryContext)
		{
			_libraryContext = libraryContext;
		}

		[HttpGet("ExportExcel")]
		public ActionResult ExportExcel()
		{
			var _emdata = GetEmpdata();
			using (XLWorkbook wb = new XLWorkbook())
			{

				wb.AddWorksheet(_emdata, "PDP Records");
				using (MemoryStream ms = new MemoryStream())
				{
					wb.SaveAs(ms);
					return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Sample.xlsx");
				}
			}

		}
		[NonAction]
		private DataTable GetEmpdata()
		{
			DataTable dt = new DataTable();
			dt.TableName = "PDPdata";
			dt.Columns.Add("pdp_Id", typeof(int));
			dt.Columns.Add("pdp_TongTienPhat", typeof(float));
			dt.Columns.Add("pdp_NgayDong", typeof(DateTime));
			dt.Columns.Add("pdp_TrangThaiDong", typeof(Boolean));
			dt.Columns.Add("pm_Id", typeof(int));

			var _list = _libraryContext.PhieuDongPhats.ToList();
			if (_list.Count > 0)
			{
				_list.ForEach(item =>
				{
					dt.Rows.Add(item.PdpId, item.PdpTongTienPhat, item.PdpNgayDong, item.PdpTrangThaiDong, item.PmId);
				});
			}

			return dt;
		}
	}
}
