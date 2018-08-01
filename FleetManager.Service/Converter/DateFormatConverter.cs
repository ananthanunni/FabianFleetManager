namespace FleetManager.Service.Converter
{
    public class DateFormatConverter:IDateFormatConverter
    {
	  public string ConvertDateFormat(string strDate)
	  {
		string[] arrDate = null;
		if (strDate.Contains("/"))
		{
		    arrDate = strDate.Split('/');
		}
		else if (strDate.Contains("-"))
		{
		    arrDate = strDate.Split('-');
		}

		if (arrDate.Length > 2)
		{
		    string strMonth = arrDate[1];
		    if (strMonth == "1" || strMonth == "01")
		    {
			  return arrDate[0] + " JAN " + arrDate[2];
		    }
		    else if (strMonth == "2" || strMonth == "02")
		    {
			  return arrDate[0] + " FEB " + arrDate[2];
		    }
		    else if (strMonth == "3" || strMonth == "03")
		    {
			  return arrDate[0] + " MAR " + arrDate[2];
		    }
		    else if (strMonth == "4" || strMonth == "04")
		    {
			  return arrDate[0] + " APR " + arrDate[2];
		    }
		    else if (strMonth == "5" || strMonth == "05")
		    {
			  return arrDate[0] + " MAY " + arrDate[2];
		    }
		    else if (strMonth == "6" || strMonth == "06")
		    {
			  return arrDate[0] + " JUN " + arrDate[2];
		    }
		    else if (strMonth == "7" || strMonth == "07")
		    {
			  return arrDate[0] + " JUL " + arrDate[2];
		    }
		    else if (strMonth == "8" || strMonth == "08")
		    {
			  return arrDate[0] + " AUG " + arrDate[2];
		    }
		    else if (strMonth == "9" || strMonth == "09")
		    {
			  return arrDate[0] + " SEP " + arrDate[2];
		    }
		    else if (strMonth == "10")
		    {
			  return arrDate[0] + " OCT " + arrDate[2];
		    }
		    else if (strMonth == "11")
		    {
			  return arrDate[0] + " NOV " + arrDate[2];
		    }
		    else if (strMonth == "12")
		    {
			  return arrDate[0] + " DEC " + arrDate[2];
		    }
		}

		return strDate;
	  }
    }
}
