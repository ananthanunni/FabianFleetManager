namespace FleetManagerWeb.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Web.Hosting;
    using System.Web.Mvc;
    using iTextSharp.text;
    using iTextSharp.text.pdf;
    using FleetManagerWeb.Common;
    using FleetManagerWeb.Models;

    public class CommonController : Controller
    {
        private readonly int inNoOfRows = 9999;
        
        private readonly IClsRole objiClsRole = null;
        private readonly IClsUser objiClsUser = null;
        private readonly IClsFleetMakes objiClsFleetMakes = null;
        private readonly IClsFleetModels objiClsFleetModels = null;
        private readonly IClsFleetColors objiClsFleetColors = null;
        private readonly IClsTripReason objiClsTripReason = null;
        private readonly IClsTracker objiClsTracker = null;

        public CommonController(IClsUser objIClsUser, IClsRole objIClsRole, IClsFleetMakes objIClsFleetMakes, IClsFleetModels objIClsFleetModels, IClsFleetColors objIClsFleetColors, IClsTripReason objIClsTripReason, IClsTracker objIClsTracker)
        {
            this.objiClsUser = objIClsUser;
            this.objiClsRole = objIClsRole;
            this.objiClsFleetMakes = objIClsFleetMakes;
            this.objiClsFleetModels = objIClsFleetModels;
            this.objiClsFleetColors = objIClsFleetColors;
            this.objiClsTripReason = objIClsTripReason;
            this.objiClsTracker = objIClsTracker;
        }

        public ActionResult ExportToCSVPDF(bool blCSVPDF, string strTableName, string strSearchValue)
        {
            try
            {
                DataTable dt = new DataTable();

                if (strTableName.ToLower() == "user")
                {
                    List<SearchUserResult> lstClsUser = this.objiClsUser.SearchUser(this.inNoOfRows, 1, strSearchValue, "FirstName");
                    if (lstClsUser != null)
                    {
                        dt = Extension.ListToDataTable(lstClsUser);
                    }
                }
                else if (strTableName.ToLower() == "role")
                {
                    List<SearchRoleResult> lstRole = this.objiClsRole.SearchRole(this.inNoOfRows, 1, strSearchValue, "RoleName");
                    if (lstRole != null)
                    {
                        dt = Extension.ListToDataTable(lstRole);
                    }
                }
                else if (strTableName.ToLower() == "fleetmakes")
                {
                    List<SearchFleetMakesResult> lstFleetMakes = this.objiClsFleetMakes.SearchFleetMakes(this.inNoOfRows, 1, strSearchValue, "FleetMakesName");
                    if (lstFleetMakes != null)
                    {
                        dt = Extension.ListToDataTable(lstFleetMakes);
                    }
                }
                else if (strTableName.ToLower() == "fleetmodels")
                {
                    List<SearchFleetModelsResult> lstFleetModels = this.objiClsFleetModels.SearchFleetModels(this.inNoOfRows, 1, strSearchValue, "FleetModelsName");
                    if (lstFleetModels != null)
                    {
                        dt = Extension.ListToDataTable(lstFleetModels);
                    }
                }
                else if (strTableName.ToLower() == "fleetcolors")
                {
                    List<SearchFleetColorsResult> lstFleetColors = this.objiClsFleetColors.SearchFleetColors(this.inNoOfRows, 1, strSearchValue, "FleetColorsName");
                    if (lstFleetColors != null)
                    {
                        dt = Extension.ListToDataTable(lstFleetColors);
                    }
                }
                else if (strTableName.ToLower() == "tripreason")
                {
                    List<SearchTripReasonResult> lstTripReason = this.objiClsTripReason.SearchTripReason(this.inNoOfRows, 1, strSearchValue, "TripReasonName");
                    if (lstTripReason != null)
                    {
                        dt = Extension.ListToDataTable(lstTripReason);
                    }
                }
                else if (strTableName.ToLower() == "tracker")
                {
                    List<SearchTrackerResult> lstTracker = this.objiClsTracker.SearchTracker(this.inNoOfRows, 1, strSearchValue, "TripStartDate", string.Empty, string.Empty, string.Empty, string.Empty);
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
                        this.Response.ClearContent();
                        this.Response.AddHeader("content-disposition", "attachment;filename=" + strTableName + string.Empty);
                        this.Response.ContentType = "text/csv";
                        this.Response.Write(sw.ToString());
                        this.Response.End();
                        return this.File(new System.Text.UTF8Encoding().GetBytes(sw.ToString()), "text/csv", strTableName);
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
                        this.Response.AppendHeader("Content-Disposition", "attachment; filename=" + strTableName);
                        this.Response.ContentType = "application/pdf";
                        this.Response.TransmitFile(filePath + strTableName);
                        this.Response.End();
                        return this.File(strTableName, "application/pdf");
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name, PageMaster.LgCommon, mySession.Current.UserId);
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