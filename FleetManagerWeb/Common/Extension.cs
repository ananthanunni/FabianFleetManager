namespace FleetManagerWeb.Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public static class Extension
    {
        private static readonly string[] Strformate = { "dd/MM/yyyy", "dd/M/yyyy", "dd/MMM/yyyy", "dd/MMM/yy", "dd/MMM/yyyy", "dd/MMMM/yy", "dd/MMMM/yyyy", "MM/dd/yyyy", "dddd, dd MMMM yyyy", "dddd, dd MMMM yyyy", "dddd, dd MMMM yyyy HH:mm", "dddd, dd MMMM yyyy hh:mm tt", "dddd, dd MMMM yyyy H:mm", "dddd, dd MMMM yyyy h:mm tt", "dddd, dd MMMM yyyy HH:mm:ss", "MM/dd/yyyy HH:mm", "MM/dd/yyyy hh:mm tt", "MM/dd/yyyy H:mm", "MM/dd/yyyy h:mm tt", "MM/dd/yyyy HH:mm:ss tt", "M/d/yyyy HH:mm:ss tt", "M/d/yyyy H:mm:ss tt", "M/d/yyyy h:m:s tt", "MM/dd/yyyy", "M/d/yyyy", "MMM/dd/yyyy HH:mm", "MMM/dd/yyyy hh:mm tt", "MMM/dd/yyyy H:mm", "MMM/dd/yyyy h:mm tt", "MMM/dd/yyyy HH:mm:ss tt", "MMM/d/yyyy HH:mm:ss tt", "MMM/d/yyyy H:mm:ss tt", "MMM/d/yyyy h:m:s tt", "MMMM/dd/yyyy HH:mm", "MMMM/dd/yyyy hh:mm tt", "MMMM/dd/yyyy H:mm", "MMMM/dd/yyyy h:mm tt", "MMMM/dd/yyyy HH:mm:ss tt", "MMMM/d/yyyy HH:mm:ss tt", "MMMM/d/yyyy H:mm:ss tt", "MMMM/d/yyyy h:m:s tt", "MMM/dd/yyyy", "MMMM/dd/yyyy", "yyyy/MM/dd HH:mm:ss tt", "yyyy/MM/dd hh:mm tt", "yyyy/MM/dd h:mm tt", "yyyy/MM/dd h:mm", "yyyy/MM/dd hh:mm:ss tt", "yyyy/MM/dd" };

        private static readonly string StrAcceptedCharacters = "KRISH1234567891123456".ToUpper();

        public static bool boolSafe(this string boolStr)
        {
            bool bl = false;
            bool.TryParse(boolStr, out bl);
            return bl;
        }

        public static DateTime? dateNullSafe(this string str)
        {
            DateTime date;
            if (DateTime.TryParseExact(str, Strformate, null, System.Globalization.DateTimeStyles.None, out date) == true)
            {
                return date;
            }

            return null;
        }

        public static DateTime dateSafe(this string str)
        {
            DateTime date;
            if (DateTime.TryParseExact(str, Strformate, null, System.Globalization.DateTimeStyles.None, out date) == true)
            {
                return date;
            }

            return date;
        }

        public static decimal? decNullSafe(this string decStr)
        {
            decimal ret = 0;
            decStr = decStr.Replace(",", string.Empty);
            decimal.TryParse(decStr, out ret);
            if (ret == 0)
            {
                return null;
            }
            else
            {
                return ret;
            }
        }

        public static string Decode(this string str)
        {
            try
            {
                str = str.Replace("%3d", "=");
                byte[] decbuff = Convert.FromBase64String(str);
                string decode = System.Text.Encoding.ASCII.GetString(decbuff);
                return decode;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string DecryptString(this string base64StringToDecrypt)
        {
            using (AesCryptoServiceProvider acsp = GetProvider(Encoding.Default.GetBytes(StrAcceptedCharacters)))
            {
                byte[] rawBytes = Convert.FromBase64String(base64StringToDecrypt);
                ICryptoTransform ictD = acsp.CreateDecryptor();
                MemoryStream msD = new MemoryStream(rawBytes, 0, rawBytes.Length);
                CryptoStream csD = new CryptoStream(msD, ictD, CryptoStreamMode.Read);
                return (new StreamReader(csD)).ReadToEnd();
            }
        }

        public static decimal decSafe(this string decStr)
        {
            decimal ret = 0;
            decStr = decStr.Replace(",", string.Empty);
            decimal.TryParse(decStr, out ret);
            return ret;
        }

        public static string Encode(this string str)
        {
            try
            {
                byte[] encbuff = System.Text.Encoding.ASCII.GetBytes(str);
                string encode = Convert.ToBase64String(encbuff);
                return encode;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string EncryptString(this string plainSourceStringToEncrypt)
        {
            using (AesCryptoServiceProvider acsp = GetProvider(Encoding.Default.GetBytes(StrAcceptedCharacters)))
            {
                byte[] sourceBytes = Encoding.ASCII.GetBytes(plainSourceStringToEncrypt);
                ICryptoTransform ictE = acsp.CreateEncryptor();
                MemoryStream msS = new MemoryStream();
                CryptoStream csS = new CryptoStream(msS, ictE, CryptoStreamMode.Write);
                csS.Write(sourceBytes, 0, sourceBytes.Length);
                csS.FlushFinalBlock();
                byte[] encryptedBytes = msS.ToArray();
                return Convert.ToBase64String(encryptedBytes);
            }
        }

        public static int GetWeekNumber(this DateTime date)
        {
            return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static int? intNullSafe(this string intStr)
        {
            if (intStr != null)
            {
                intStr = intStr.Replace(",", string.Empty);
            }

            int ret = 0;
            int.TryParse(intStr, out ret);
            if (ret == 0)
            {
                return null;
            }
            else
            {
                return ret;
            }
        }

        public static int intSafe(this string intStr)
        {
            if (intStr != null)
            {
                intStr = intStr.Replace(",", string.Empty);
            }

            int ret = 0;
            int.TryParse(intStr, out ret);
            return ret;
        }

        public static DataTable ListToDataTable<T>(List<T> list)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in list)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }

                table.Rows.Add(row);
            }

            if (table.Columns.Contains("RowNum"))
            {
                table.Columns.Remove("RowNum");
            }

            if (table.Columns.Contains("Total"))
            {
                table.Columns.Remove("Total");
            }

            return table;
        }

        public static long? longNullSafe(this string longStr)
        {
            if (longStr != null)
            {
                longStr = longStr.Replace(",", string.Empty);
            }

            long ret = 0;
            long.TryParse(longStr, out ret);
            if (ret == 0)
            {
                return null;
            }
            else
            {
                return ret;
            }
        }

        public static long longSafe(this string longStr)
        {
            long ret = 0;
            long.TryParse(longStr, out ret);
            return ret;
        }

        public static string MonthName(this int intMonth)
        {
            try
            {
                switch (intMonth)
                {
                    case 1:
                        return "Jan";
                    case 2:
                        return "Feb";
                    case 3:
                        return "Mar";
                    case 4:
                        return "Apr";
                    case 5:
                        return "May";
                    case 6:
                        return "Jun";
                    case 7:
                        return "Jul";
                    case 8:
                        return "Aug";
                    case 9:
                        return "Sep";
                    case 10:
                        return "Oct";
                    case 11:
                        return "Nov";
                    case 12:
                        return "Dec";
                }
            }
            catch (Exception ex)
            {
                Functions.Write(ex, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString(), PageMaster.LgCommon);
            }

            return string.Empty;
        }

        public static string strSafe(this string str)
        {
            return Convert.ToString(str);
        }

        private static byte[] GetKey(byte[] suggestedKey, SymmetricAlgorithm p)
        {
            byte[] lstRaw;
            
            lstRaw = suggestedKey;
            
            List<byte> lstK;
            
            lstK = new List<byte>();

            for (int i = 0; i < p.LegalKeySizes[0].MinSize; i += 8)
            {
                lstK.Add(lstRaw[(i / 8) % lstRaw.Length]);
            }

            byte[] k = lstK.ToArray();
            return k;
        }

        private static AesCryptoServiceProvider GetProvider(byte[] key)
        {
            AesCryptoServiceProvider result = new AesCryptoServiceProvider();
            result.BlockSize = 128;
            result.KeySize = 128;
            result.Mode = CipherMode.CBC;
            result.Padding = PaddingMode.PKCS7;

            result.GenerateIV();
            result.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            byte[] realKey = GetKey(key, result);
            result.Key = realKey;
            return result;
        }
    }
}