   using FleetManager.Core.Common;
    using FleetManager.Core.Extensions;
    using FleetManager.Data.Models;
    using FleetManagerWeb.Models;
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Web.Hosting;
    using System.Web.Mvc;

 namespace FleetManagerWeb.Controllers
{
    public class CommonController : BaseController
    {
	  private readonly int inNoOfRows = 9999;

	  private readonly IClsRole _objiClsRole = null;
	  private readonly IClsUser _objiClsUser = null;
	  private readonly IClsFleetMakes _objiClsFleetMakes = null;
	  private readonly IClsFleetModels _objiClsFleetModels = null;
	  private readonly IClsFleetColors _objiClsFleetColors = null;
	  private readonly IClsTripReason _objiClsTripReason = null;
	  private readonly IClsTracker _objiClsTracker = null;
	  private readonly IMySession _mySession;

	  public CommonController(IClsUser objIClsUser, IClsRole objIClsRole, IClsFleetMakes objIClsFleetMakes, IClsFleetModels objIClsFleetModels, IClsFleetColors objIClsFleetColors, IClsTripReason objIClsTripReason, IClsTracker objIClsTracker,
		IMySession mySession)
	  {
		_objiClsUser = objIClsUser;
		_objiClsRole = objIClsRole;
		_objiClsFleetMakes = objIClsFleetMakes;
		_objiClsFleetModels = objIClsFleetModels;
		_objiClsFleetColors = objIClsFleetColors;
		_objiClsTripReason = objIClsTripReason;
		_objiClsTracker = objIClsTracker;
		_mySession = mySession;
	  }

	  public ActionResult ExportToCSVPDF(bool blCSVPDF, string strTableName, string strSearchValue)
	  {
		try
		{
		    DataTable dt = new DataTable();

		    if (strTableName.ToLower() == "user")
		    {
			  List<SearchUserResult> lstClsUser = _objiClsUser.SearchUser(inNoOfRows, 1, strSearchValue, "FirstName");
			  if (lstClsUser != null)
			  {
				dt = Extension.ListToDataTable(lstClsUser);
			  }
		    }
		    else if (strTableName.ToLower() == "role")
		    {
			  List<SearchRoleResult> lstRole = _objiClsRole.SearchRole(inNoOfRows, 1, strSearchValue, "RoleName");
			  if (lstRole != null)
			  {
				dt = Extension.ListToDataTable(lstRole);
			  }
		    }
		    else if (strTableName.ToLower() == "fleetmakes")
		    {
			  List<SearchFleetMakesResult> lstFleetMakes = _objiClsFleetMakes.SearchFleetMakes(inNoOfRows, 1, strSearchValue, "FleetMakesName");
			  if (lstFleetMakes != null)
			  {
				dt = Extension.ListToDataTable(lstFleetMakes);
			  }
		    }
		    else if (strTableName.ToLower() == "fleetmodels")
		    {
			  List<SearchFleetModelsResult> lstFleetModels = _objiClsFleetModels.SearchFleetModels(inNoOfRows, 1, strSearchValue, "FleetModelsName");
			  if (lstFleetModels != null)
			  {
				dt = Extension.ListToDataTable(lstFleetModels);
			  }
		    }
		    else if (strTableName.ToLower() == "fleetcolors")
		    {
			  List<SearchFleetColorsResult> lstFleetColors = _objiClsFleetColors.SearchFleetColors(inNoOfRows, 1, strSearchValue, "FleetColorsName");
			  if (lstFleetColors != null)
			  {
				dt = Extension.ListToDataTable(lstFleetColors);
			  }
		    }
		    else if (strTableName.ToLower() == "tripreason")
		    {
			  List<SearchTripReasonResult> lstTripReason = _objiClsTripReason.SearchTripReason(inNoOfRows, 1, strSearchValue, "TripReasonName");
			  if (lstTripReason != null)
			  {
				dt = Extension.ListToDataTable(lstTripReason);
			  }
		    }
		    else if (strTableName.ToLower() == "tracker")
		    {
			  List<SearchTrackerResult> lstTracker = _objiClsTracker.SearchTracker(inNoOfRows, 1, strSearchValue, "TripStartDate", string.Empty, string.Empty, string.Empty, string.Empty);
			  if (lstTracker != null)
			  {
				dt = Extension.ListToDataTable(lstTracker);
			  }
		    }

		    if (dt != null && dt.Rows.Count > 0)
		    {
			  StringWriter sw = new StringWriter();
			  if (blCSVPDF)
			  {
				strTableName = strTableName + ".csv";
				for (int i = 0; i < dt.Columns.Count; i++)
				{
				    sw.Write(dt.Columns[i]);
				    if (i < dt.Columns.Count - 1)
				    {
					  sw.Write(",");
				    }
				}

				sw.Write(sw.NewLine);
				foreach (DataRow dr in dt.Rows)
				{
				    for (int i = 0; i < dt.Columns.Count; i++)
				    {
					  if (!Convert.IsDBNull(dr[i]))
					  {
						string value = dr[i].ToString();
						if (value.Contains(','))
						{
						    value = string.Format("\"{0}\"", value);
						    sw.Write(value);
						}
						else
						{
						    sw.Write(dr[i].ToString());
						}
					  }

					  if (i < dt.Columns.Count - 1)
					  {
						sw.Write(",");
					  }
				    }

				    sw.Write(sw.NewLine);
				}

				sw.Close();
				Response.ClearContent();
				Response.AddHeader("content-disposition", "attachment;filename=" + strTableName + string.Empty);
				Response.ContentType = "text/csv";
				Response.Write(sw.ToString());
				Response.End();
				return File(new System.Text.UTF8Encoding().GetBytes(sw.ToString()), "text/csv", strTableName);
			  }
			  else
			  {
				Document document;
				int inFontSize = 6;
				if (dt.Columns.Count > 6)
				{
				    document = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 10f);
				    inFontSize = 2;
				}
				else
				{
				    document = new iTextSharp.text.Document(PageSize.A4, 10f, 10f, 10f, 10f);
				}

				string filePath;
				if (Directory.Exists(HostingEnvironment.MapPath("~/Content/ExportFiles")))
				{
				    filePath = HostingEnvironment.MapPath("~/Content/ExportFiles/");
				    PdfWriter.GetInstance(document, new FileStream(filePath + strTableName, FileMode.Create));
				}
				else
				{
				    Directory.CreateDirectory(HostingEnvironment.MapPath("~/Content/ExportFiles"));
				    filePath = HostingEnvironment.MapPath("~/Content/ExportFiles/");
				    PdfWriter.GetInstance(document, new FileStream(filePath + strTableName, FileMode.Create));
				}

				document.Open();
				document.NewPage();
				PdfPTable table = new PdfPTable(dt.Columns.Count);
				table.WidthPercentage = 100;
				table.SpacingBefore = 10;
				for (int i = 0; i < dt.Columns.Count; i++)
				{
				    addCell(table, dt.Columns[i].ColumnName, inFontSize);
				}

				foreach (DataRow dr in dt.Rows)
				{
				    for (int i = 0; i < dt.Columns.Count; i++)
				    {
					  string value = dr[i].ToString();
					  addCell(table, value, inFontSize);
				    }
				}

				document.Add(table);
				document.Close();
				Response.AppendHeader("Content-Disposition", "attachment; filename=" + strTableName);
				Response.ContentType = "application/pdf";
				Response.TransmitFile(filePath + strTableName);
				Response.End();
				return File(strTableName, "application/pdf");
			  }
		    }
		    else
		    {
			  return null;
		    }
		}
		catch (Exception ex)
		{
		    Logger.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.LgCommon, _mySession.UserId);
		    return null;
		}
	  }

	  private static void addCell(PdfPTable table, string strTextValue, int inFontSize)
	  {
		BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
		Font times = new Font(bfTimes, inFontSize);

		PdfPCell cell = new PdfPCell(new Phrase(strTextValue, times));
		cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
		cell.VerticalAlignment = PdfPCell.ALIGN_LEFT;
		table.AddCell(cell);
	  }
    }
}