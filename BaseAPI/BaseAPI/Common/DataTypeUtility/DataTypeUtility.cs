using System.Data;
using System.Collections;
using System.Xml;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace BaseAPI.Common.DataTypeUtility
{
    public class DataTypeUtility : IDataTypeUtility
    {
        public static DataTable GetSplitDataTable(DataTable dt, int totalCount, int index)
        {
            if (totalCount <= 0)
                return dt;

            if (index < 0)
                return dt;

            var totalRows = dt.Rows.Count;
            var devideCount = totalRows / totalCount;

            return null;
        }

        public static int GetBooleanToInt32(bool isTrue)
        {
            if (isTrue)
                return 1;
            return 0;
        }

        public static bool GetInt32ToBoolean(int isTrue)
        {
            if (isTrue == 1)
                return true;
            return false;
        }

        public static bool GetToBoolean(object obj)
        {
            if (obj == null
                || obj == DBNull.Value
                || obj.ToString() == "")
            {
                return false;
            }

            return Convert.ToBoolean(obj);
        }

        public static Double GetToDouble(object obj)
        {
            if (obj == null
                || obj == DBNull.Value
                || obj.ToString() == "")
            {
                return 0;
            }

            return Convert.ToDouble(obj.ToString().Replace(",", ""));
        }

        public static Double GetToPlusDouble(object obj)
        {
            if (obj == null
                || obj == DBNull.Value
                || obj.ToString() == "")
            {
                return 0;
            }

            double num = Convert.ToDouble(obj.ToString().Replace(",", ""));

            if (num < 0)
                return num * (-1);

            return num;
        }

        public static Double GetToMinusDouble(object obj)
        {
            if (obj == null
                || obj == DBNull.Value
                || obj.ToString() == "")
            {
                return 0;
            }

            double num = Convert.ToDouble(obj.ToString().Replace(",", ""));

            if (num > 0)
                return num * (-1);

            return num;
        }

        public static string GetToOnlyMinusText(object obj)
        {
            return GetToOnlyMinusText(obj, "#,##0", true);
        }

        /// <summary>
        /// 그리드에서 만약 값이 - 부호가 붙었을 경우 () 로 변경해 주는 메소드
        /// </summary>
        /// <param name="obj">값</param>
        /// <param name="format">포맷</param>
        /// <returns></returns>
        public static string GetToOnlyMinusText(object obj, string format, bool hasRedText)
        {
            if (obj == null
                || obj == DBNull.Value
                || obj.ToString() == "")
            {
                return "0";
            }

            double num = Convert.ToDouble(obj.ToString().Replace(",", ""));

            if (num >= 0)
                return num.ToString(format);
            else
                num = num * (-1);

            if (hasRedText)
                return string.Format("<font style='color:Red'>({0})</font>", num.ToString(format));
            else
                return string.Format("({0})", num.ToString(format));
        }

        /// <summary>
        /// GetToOnlyMinusText 로 생성된 값을 다시 원래 값(마이너스값)으로 되돌림
        /// </summary>
        public static object GetToOnlyPlusText(object getToOnlyMinusString)
        {
            object objReturn = getToOnlyMinusString;

            if (getToOnlyMinusString.ToString().IndexOf("<font style='color:Red'>(") == 0)
            {
                string strReturn = getToOnlyMinusString.ToString().Replace("<font style='color:Red'>(", string.Empty);
                strReturn = strReturn.Replace(")</font>", string.Empty);

                double dbReturn = Convert.ToDouble(strReturn);
                dbReturn = dbReturn * -1;

                objReturn = dbReturn;
            }

            return objReturn;
        }

        public static DateTime GetDateFirstDay(DateTime date)
        {
            return DataTypeUtility.GetToDateTime(date.ToString("yyyy-MM") + "-01");
        }

        public static DateTime GetDateLastDay(DateTime date)
        {
            return DataTypeUtility.GetToDateTime(GetDateFirstDay(date).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd"));
        }

        public static float GetToFloat(object obj)
        {
            if (obj == null
                || obj == DBNull.Value
                || obj.ToString() == "")
            {
                return 0;
            }

            return Convert.ToSingle(obj.ToString().Replace(",", ""));
        }

        public static int GetToInt32(object obj)
        {
            if (obj == null)
                return 0;

            if (obj == DBNull.Value)
                return 0;

            if (obj.ToString() == "")
                return 0;

            if (int.TryParse(obj?.ToString().Replace(",", ""), out var r))
            {
                return Convert.ToInt32(obj.ToString().Replace(",", ""));
            }

            return 0;
        }

        public static int GetTimeStamp()
        {
            var d1 = DateTime.Parse("1970-01-01 09:00:00");
            var d2 = DateTime.Now;
            var d3 = d2 - d1;
            return Convert.ToInt32(d3.TotalSeconds);
        }

        public static string GetToInt32_String(object obj, string format)
        {
            if (obj == null
                || obj == DBNull.Value
                || obj.ToString() == "")
            {
                return "";
            }

            if (int.Parse(obj.ToString()) == 0)
                return "";

            if (format != null)
                return Convert.ToInt32(obj.ToString().Replace(",", "")).ToString(format);

            return Convert.ToInt32(obj).ToString();
        }

        public static Int64 GetToInt64(object obj)
        {
            if (obj == null
                || obj == DBNull.Value
                || obj.ToString() == "")
            {
                return 0;
            }

            return Convert.ToInt64(obj.ToString().Replace(",", ""));
        }

        public static string GetToDateTimeText(object obj)
        {
            return GetToDateTimeText(obj, "yyyy-MM-dd");
        }

        public static string GetToDateTimeText(object obj, string formatString)
        {
            if (obj == null
                || obj == DBNull.Value
                || obj.ToString() == "")
            {
                return "";
            }

            return Convert.ToDateTime(obj).ToString(formatString);
        }

        public static string GetToLastDateText(object year, object month, bool isReverse)
        {
            return GetToLastDateText(year, month, isReverse, "yyyy-MM-dd");
        }

        /// <summary>
        /// 전표에서 전표일자를 만들기 위함 메소드
        /// </summary>
        /// <param name="year">년</param>
        /// <param name="month">월</param>
        /// <param name="isReverse">역기표여부</param>
        /// <param name="formatString">포맷</param>
        /// <returns></returns>
        public static string GetToLastDateText(object year, object month, bool isReverse, string formatString)
        {
            if (year == null
                || year == DBNull.Value
                || year.ToString() == ""
                || month == null
                || month == DBNull.Value
                || month.ToString() == ""
                )
            {
                return "";
            }

            string year_str = year.ToString();
            string month_str = month.ToString();

            if (month_str.Equals("00"))
                month_str = "01";

            DateTime date = GetToDateTime(year_str + "-" + month_str + "-" + "01");

            // 역기표라면
            if (isReverse)
            {
                return date.ToString(formatString);
            }

            return date.AddMonths(1).AddDays(-1).ToString(formatString);
        }

        public static DateTime GetToDateTime(object obj)
        {
            if (obj == null
                || obj == DBNull.Value
                || obj.ToString() == "")
            {
                return DateTime.MinValue;
            }

            return Convert.ToDateTime(obj);
        }

        public static string GetString(object obj)
        {
            if (obj == null
                || obj == DBNull.Value
                || obj.ToString() == "")
            {
                return "";
            }

            return obj.ToString();
        }

        public static string GetBooleanToYN(bool isTrue)
        {
            if (isTrue)
                return "Y";

            return "N";
        }

        public static bool GetYNToBoolean(string ynStr)
        {
            if (ynStr == null || ynStr.Trim() == "")
                return false;

            if (ynStr.ToUpper().Equals("Y"))
                return true;

            return false;
        }

        public static string GetValue(object obj)
        {
            return GetValue(obj, "");
        }

        public static string GetValue(object obj, string defaultVaue)
        {
            if (obj == null
                || obj == DBNull.Value)
            {
                return defaultVaue;
            }

            return obj.ToString();
        }

        public static decimal GetToDecimal(object obj)
        {
            if (obj == null
                || obj == DBNull.Value
                || obj.ToString() == "")
            {
                return 0;
            }

            return Convert.ToDecimal(obj.ToString().Replace(",", ""));
        }

        public static void SetYearMonthByNextMonth(string year
                                                , string month
                                                , out string next_year
                                                , out string next_month)
        {
            if (month.Equals("12"))
            {
                next_year = Convert.ToInt32(Convert.ToInt32(year) + 1).ToString();
                next_month = "00";
            }
            else if (month.Equals("00"))
            {
                next_year = year;
                next_month = "01";
            }

            next_year = Convert.ToDateTime(year + "-" + month + "-" + "01").AddMonths(1).Year.ToString();
            next_month = Convert.ToDateTime(year + "-" + month + "-" + "01").AddMonths(1).Month.ToString().PadLeft(2, '0');
        }

        public static string GetInt32ToAlphabet(int index)
        {
            List<string> list = new List<string>();
            Dictionary<int, string> dic = new Dictionary<int, string>();

            dic.Add(0, "");
            dic.Add(1, "A");
            dic.Add(2, "B");
            dic.Add(3, "C");
            dic.Add(4, "D");
            dic.Add(5, "E");
            dic.Add(6, "F");
            dic.Add(7, "G");
            dic.Add(8, "H");
            dic.Add(9, "I");
            dic.Add(10, "J");
            dic.Add(11, "K");
            dic.Add(12, "L");
            dic.Add(13, "M");
            dic.Add(14, "N");
            dic.Add(15, "O");
            dic.Add(16, "P");
            dic.Add(17, "Q");
            dic.Add(18, "R");
            dic.Add(19, "S");
            dic.Add(20, "T");
            dic.Add(21, "U");
            dic.Add(22, "V");
            dic.Add(23, "W");
            dic.Add(24, "X");
            dic.Add(25, "Y");
            dic.Add(26, "Z");

            if (dic.ContainsKey(index))
                return dic[index];

            return "";
        }

        public static Type GetStringToType(string dataType)
        {
            if (dataType.ToUpper().Equals("STRING"))
            {
                return typeof(string);
            }
            else if (dataType.ToUpper().Equals("DATETIME"))
            {
                return typeof(DateTime);
            }
            if (dataType.ToUpper().Equals("INT"))
            {
                return typeof(int);
            }
            if (dataType.ToUpper().Equals("LONG"))
            {
                return typeof(long);
            }
            if (dataType.ToUpper().Equals("DOUBLE"))
            {
                return typeof(double);
            }
            if (dataType.ToUpper().Equals("FLOAT"))
            {
                return typeof(float);
            }
            if (dataType.ToUpper().Equals("OBJECT"))
            {
                return typeof(object);
            }

            return typeof(object);
        }

        public static bool TryParse(string dataType, object value)
        {
            DateTime dateTime_out;
            int int_out;
            long long_out;
            double double_out;
            float float_out;

            if (dataType.ToUpper().Equals("STRING"))
            {
                return true;
            }
            else if (dataType.ToUpper().Equals("DATETIME"))
            {
                return DateTime.TryParse(value.ToString(), out dateTime_out);
            }
            if (dataType.ToUpper().Equals("INT"))
            {
                return int.TryParse(value.ToString(), out int_out);
            }
            if (dataType.ToUpper().Equals("LONG"))
            {
                return long.TryParse(value.ToString(), out long_out);
            }
            if (dataType.ToUpper().Equals("DOUBLE"))
            {
                return double.TryParse(value.ToString(), out double_out);
            }
            if (dataType.ToUpper().Equals("FLOAT"))
            {
                return float.TryParse(value.ToString(), out float_out);
            }
            if (dataType.ToUpper().Equals("OBJECT"))
            {
                return true;
            }

            return true;
        }

        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null)
                throw new ArgumentNullException("password");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null)
                throw new ArgumentNullException("password");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            if (storedHash.Length != 64)
                throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");

            if (storedSalt.Length != 128)
                throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        public static DataSet FilterSortDataSet(DataSet dsStart, int iTbl, string filter, string sort)
        {
            System.Data.DataTable dt = dsStart.Tables[iTbl].Clone();
            DataRow[] drs = dsStart.Tables[iTbl].Select(filter, sort);

            foreach (DataRow dr in drs)
            {
                dt.ImportRow(dr);
            }

            DataSet ds = new DataSet();

            for (int i = 0; i < dsStart.Tables.Count; i++)
            {
                if (i == iTbl)
                {
                    ds.Tables.Add(dt);
                }
                else
                {
                    ds.Tables.Add(dsStart.Tables[i].Copy());
                }
            }

            return ds;
        }

        public static DataSet FilterSortDataSet(DataSet dsStart, string filter, string sort)
        {
            DataSet ds = dsStart.Clone();
            DataRow[] drs = dsStart.Tables[0].Select(filter, sort);

            foreach (DataRow dr in drs)
            {
                ds.Tables[0].ImportRow(dr);
            }

            return ds;
        }

        public static DataTable Limit(DataTable dtStart, int limit)
        {
            return FilterSortDataTable(dtStart, null, null, null, limit);
        }

        public static DataTable FilterSortDataTable(DataTable dtStart
                                                , string filter
                                                , string sort
                                                , string tableName
                                                , int limit)
        {
            if (dtStart.Rows.Count == 0)
                return dtStart;

            if (filter == null && sort == null)
                filter = "";


            System.Data.DataTable dt = dtStart.Clone();

            if (tableName != null && tableName != "")
                dt.TableName = tableName;

            DataRow[] drs = null;

            if (sort == null)
                drs = dtStart.Select(filter);
            else
                drs = dtStart.Select(filter, sort);

            int rowCount = 0;

            if (limit > 0)
                rowCount = Math.Min(drs.Length, limit);
            else
                rowCount = drs.Length;

            for (int i = 0; i < rowCount; i++)
            {
                DataRow dr = drs[i];
                dt.ImportRow(dr);
            }

            return dt;
        }

        public static DataTable TopDataRow(DataTable dt, int count)
        {
            DataTable dtn = dt.Clone();
            int i = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (i < count)
                {
                    dtn.ImportRow(row);
                    i++;
                }
                if (i > count)
                    break;
            }
            return dtn;
        }

        public static System.Data.DataTable FilterSortDataTable(System.Data.DataTable dtStart, string filter)
        {
            return FilterSortDataTable(dtStart, filter, null, null, 0);
        }

        public static System.Data.DataTable FilterSortDataTable(System.Data.DataTable dtStart, string filter, string sort)
        {
            return FilterSortDataTable(dtStart, filter, sort, null, 0);
        }


        // Filter 문자열 반환
        public static string GetFilterString(DataRow dataRow, string[] filterColumns_L, string[] filterColumns_R)
        {
            string filter = "1 = 1";

            for (int i = 0; i < filterColumns_L.Length; i++)
            {
                filter += string.Format(" AND {0} = '{1}'", filterColumns_R[i], dataRow[filterColumns_L[i]]);
            }

            return filter;
        }

        // Filter 문자열 반환
        public static string GetFilterString(DataRow dataRow, string[] filterColumns)
        {
            string filter = "1 = 1";

            for (int i = 0; i < filterColumns.Length; i++)
            {
                filter += string.Format(" AND {0} = '{1}'", filterColumns[i], dataRow[filterColumns[i]]);
            }

            return filter;
        }

        // Where MAIN_UNIT_ID = 'MAIN' And SUB_UNIT_ID = 'SUB' And ACT_ID = 'ACT'
        //	And SelectColumnName = 'SelectColumnValue'
        public static string GetFilterString(DataRow dataRow, string[] selectGeneralColumns, string selectColumnName, string selectColumnValue)
        {
            string strReturn = string.Empty;

            int intCount = selectGeneralColumns.Length;
            if (intCount != 0)
            {
                for (int i = 0; i < intCount; i++)
                {
                    if (i != 0)
                        strReturn += "AND ";

                    strReturn += string.Format("{0} = '{1}' ", selectGeneralColumns[i], dataRow[selectGeneralColumns[i]]);
                }

                if (selectColumnName.Length == 0 || selectColumnValue.Length == 0)
                    return strReturn;
                else
                    strReturn += string.Format("AND {0} = '{1}'", selectColumnName, selectColumnValue);
            }
            else
                strReturn += string.Format("{0} = '{1}'", selectColumnName, selectColumnValue);

            return strReturn;
        }

        /// <summary>
        /// 07.12.4 류승태
        /// DataRow에 NULL이 있을때 Is Null로...
        /// </summary>
        public static string GetFilterStringIsNull(DataRow dataRow, string[] selectGeneralColumns, string[] nullColumns)
        {
            string strReturn = string.Empty;

            int intCount = selectGeneralColumns.Length;
            if (intCount != 0)
            {
                for (int i = 0; i < intCount; i++)
                {
                    if (i != 0)
                        strReturn += "AND ";

                    // NULL 여부 확인
                    if (dataRow[selectGeneralColumns[i]].ToString().Length != 0)
                    {
                        strReturn += string.Format("{0} = '{1}' ", selectGeneralColumns[i], dataRow[selectGeneralColumns[i]]);
                    }
                    else
                    {
                        if (ChkValue(nullColumns, selectGeneralColumns[i]) == true)
                            strReturn += string.Format("{0} IS NULL ", selectGeneralColumns[i]);
                        else
                            strReturn += string.Format("{0} = '' ", selectGeneralColumns[i]);
                    }
                }
            }

            return strReturn;
        }

        private static bool ChkValue(string[] strarr, string strValue)
        {
            bool bolReturn = false;

            for (int i = 0; i < strarr.Length; i++)
            {
                if (strarr[i].Equals(strValue) == true)
                    return true;
            }

            return bolReturn;
        }

        // Sort 문자열 반환 오버로드 1
        public static string GetSortString(string[] sortColumns, string lastfix, string asc_desc)
        {
            string sorter = "";

            for (int i = 0; i < sortColumns.Length; i++)
            {
                if (i == 0)
                {
                    sorter += sortColumns[i] + lastfix + " " + asc_desc;
                }
                else
                {
                    sorter += "," + sortColumns[i] + lastfix + " " + asc_desc;
                }
            }
            return sorter;
        }

        // Sort 문자열 반환 오버로드 2
        public static string GetSortString(string[] sortColumns)
        {
            return GetSortString(sortColumns, "", "");
        }

        // Array Merging...
        public static string[] MergeArrays(string[] array1, string[] array2)
        {
            if (array2 == null)
                return array1;

            string[] array = new string[array1.Length + array2.Length];

            int i = 0;

            for (; i < array1.Length; i++)
            {
                array[i] = array1[i];
            }

            i++;

            for (; i < array.Length; i++)
            {
                array[i] = array2[i];
            }

            return array;
        }

        public static string Right(string s, int n)
        {
            if (n == s.Length)
            {
                return s;
            }
            else if (n < 1)
            {
                return "";
            }
            else
            {
                return s.Substring(s.Length - n, n);
            }
        }

        public static object GetToDbNull(object obj)
        {
            if (obj == null ||
                obj == DBNull.Value ||
                obj.ToString().Length == 0)
                return DBNull.Value;

            return obj;
        }

        public static string GetSplitString(DataTable dataTable
                                                , string columnName
                                                , string spliter)
        {
            return GetSplitString(dataTable
                                , columnName
                                , spliter
                                , true);
        }

        /// <summary>
	    /// 데이터 테이블에서 해당 컬럼의 Spliter를 붙인 문자열을 반환한다.
	    /// </summary>
	    /// <param name="dataTable"></param>
	    /// <param name="columnName"></param>
	    /// <param name="spliter"></param>
	    /// <returns></returns>
	    public static string GetSplitString(DataTable dataTable
                                            , string columnName
                                            , string spliter
                                            , bool isString)
        {
            string temp = "";
            bool isFirst = true;

            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (isFirst)
                {
                    if (isString)
                        temp += string.Format("'{0}'", dataRow[columnName]);
                    else
                        temp += string.Format("{0}", dataRow[columnName]);

                    isFirst = false;
                }
                else
                {
                    if (isString)
                        temp += string.Format("{1}'{0}'", dataRow[columnName], spliter);
                    else
                        temp += string.Format("{1}{0}", dataRow[columnName], spliter);
                }
            }

            return temp;
        }

        public static string GetSplitString(string[] strings
                                            , string spliter)
        {
            string temp = "";
            bool isFirst = true;

            foreach (string str in strings)
            {
                if (isFirst)
                {
                    temp += string.Format("{0}", str);
                    isFirst = false;
                }
                else
                {
                    temp += string.Format("{1}{0}", str, spliter);
                }
            }

            return temp;
        }


        public static string ConvertDataTableToXML(DataTable dt)
        {
            XmlDocument doc = new XmlDocument();
            XmlNode rows;
            XmlNode row;
            XmlNode item;

            rows = doc.CreateElement("Rows");
            doc.AppendChild(rows);

            foreach (DataRow dr in dt.Rows)
            {
                row = doc.CreateElement("Row");
                rows.AppendChild(row);

                foreach (DataColumn dc in dt.Columns)
                {
                    item = doc.CreateElement(dc.ColumnName);
                    item.InnerText = DataTypeUtility.GetValue(dr[dc]);
                    row.AppendChild(item);
                }
            }

            return doc.InnerXml;
        }

        public static string JSonStatus(string status, string message)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("status", typeof(string));
            dt.Columns.Add("message", typeof(string));

            DataRow dr = dt.NewRow();
            dr[0] = status;
            dr[1] = message;
            dt.Rows.Add(dr);

            return JsonConvert.SerializeObject(dt);
        }

        private static object lockObj = new object();

        public static string JSon(string code,
                                    string status,
                                    string message,
                                    object data)
        {
            return JSon(code,
                        status,
                        message,
                        data,
                        null);
        }

        public static string JSon(string code,
                                    string status,
                                    string message,
                                    object data,
                                    object option)
        {
            lock (lockObj)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("response_code", code);

                if (data is DataTable)
                {
                    dic.Add("response_data_count", (data != null) ? ((DataTable)data).Rows.Count : 0);
                }
                else if (data is Dictionary<string, DataTable>)
                {
                    dic.Add("response_data_count", (data != null) ? 1 : 0);
                }
                else if (data is IList)
                {
                    dic.Add("response_data_count", (data != null) ? ((IList)data).Count : 0);
                }
                else if (data is Dictionary<string, object> || data is Dictionary<string, string>)
                {
                    dic.Add("response_data_count", 1);
                }
                else if (data == null)
                {
                    dic.Add("response_data_count", 0);
                }
                else
                {
                    dic.Add("response_data_count", 1);
                }

                dic.Add("response_data", data);
                dic.Add("response_message", message);
                dic.Add("response_status", status);

                if (option != null)
                {
                    Dictionary<string, object> dicOption = (Dictionary<string, object>)option;
                    dicOption.Add("option_use_yn", "Y");

                    if (dicOption.ContainsKey("page_count"))
                        dicOption.Add("paging_use_yn", "Y");
                    else
                        dicOption.Add("paging_use_yn", "N");

                    dic.Add("response_option", dicOption);
                }
                else
                {
                    Dictionary<string, object> dicOption = new Dictionary<string, object>();
                    dicOption.Add("option_use_yn", "N");

                    if (dicOption.ContainsKey("page_count"))
                        dicOption.Add("paging_use_yn", "Y");
                    else
                        dicOption.Add("paging_use_yn", "N");

                    dic.Add("response_option", dicOption);
                }

                return JsonConvert.SerializeObject(dic);
            }
        }

        public static Dictionary<string, object> JsonObject(string code,
                                                            string status,
                                                            string message,
                                                            object data,
                                                            object option = null)
        {

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("response_code", code);

            if (data is DataTable)
            {
                dic.Add("response_data_count", (data != null) ? ((DataTable)data).Rows.Count : 0);
            }
            else if (data is Dictionary<string, DataTable>)
            {
                dic.Add("response_data_count", (data != null) ? 1 : 0);
            }
            else if (data is IList)
            {
                dic.Add("response_data_count", (data != null) ? ((IList)data).Count : 0);
            }
            else if (data is Dictionary<string, object> || data is Dictionary<string, string>)
            {
                dic.Add("response_data_count", 1);
            }
            else if (data == null)
            {
                dic.Add("response_data_count", 0);
            }
            else
            {
                dic.Add("response_data_count", 1);
            }

            dic.Add("response_data", data);
            dic.Add("response_message", message);
            dic.Add("response_status", status);

            if (option != null)
            {
                Dictionary<string, object> dicOption = (Dictionary<string, object>)option;
                dicOption.Add("option_use_yn", "Y");

                if (dicOption.ContainsKey("page_count"))
                    dicOption.Add("paging_use_yn", "Y");
                else
                    dicOption.Add("paging_use_yn", "N");

                dic.Add("response_option", dicOption);
            }
            else
            {
                Dictionary<string, object> dicOption = new Dictionary<string, object>();
                dicOption.Add("option_use_yn", "N");

                if (dicOption.ContainsKey("page_count"))
                    dicOption.Add("paging_use_yn", "Y");
                else
                    dicOption.Add("paging_use_yn", "N");

                dic.Add("response_option", dicOption);
            }

            return dic;

        }


        public string JSonString(string code,
                                    string status,
                                    string message,
                                    object data)
        {
            return JSonString(code,
                        status,
                        message,
                        data,
                        null);
        }

        public string JSonString(string code,
                                    string status,
                                    string message,
                                    object data,
                                    object option)
        {
            lock (lockObj)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("response_code", code);

                if (data is DataTable)
                {
                    dic.Add("response_data_count", (data != null) ? ((DataTable)data).Rows.Count : 0);
                }
                else if (data is Dictionary<string, DataTable>)
                {
                    dic.Add("response_data_count", (data != null) ? 1 : 0);
                }
                // else if (data is IFindFluent<Feed, Feed>)
                // {
                //     dic.Add("response_data_count", (data != null) ? (int)((IFindFluent<Feed, Feed>)data).CountDocuments() : 0);
                // }
                else if (data is IList)
                {
                    dic.Add("response_data_count", (data != null) ? ((IList)data).Count : 0);
                }
                else
                {
                    dic.Add("response_data_count", 0);
                }

                dic.Add("response_data", data);
                dic.Add("response_message", message);
                dic.Add("response_status", status);

                if (option != null)
                {
                    Dictionary<string, object> dicOption = (Dictionary<string, object>)option;
                    dicOption.Add("option_use_yn", "Y");

                    if (dicOption.ContainsKey("page_count"))
                        dicOption.Add("paging_use_yn", "Y");
                    else
                        dicOption.Add("paging_use_yn", "N");

                    dic.Add("response_option", dicOption);
                }
                else
                {
                    Dictionary<string, object> dicOption = new Dictionary<string, object>();
                    dicOption.Add("option_use_yn", "N");

                    if (dicOption.ContainsKey("page_count"))
                        dicOption.Add("paging_use_yn", "Y");
                    else
                        dicOption.Add("paging_use_yn", "N");

                    dic.Add("response_option", dicOption);
                }

                return JsonConvert.SerializeObject(dic);
            }
        }

        public static System.Drawing.Image ScaleByPercent(System.Drawing.Image imgPhoto, int Percent)
        {
            float nPercent = ((float)Percent / 100);

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;

            int destX = 0;
            int destY = 0;
            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        public static Image FixedSize(Image imgPhoto, int Width, int Height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((Width -
                              (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((Height -
                              (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height,
                              PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                             imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Red);
            grPhoto.InterpolationMode =
                    InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        public static bool IsEmail(string email)
        {
            email = email.ToValue();

            Regex rgxEmail = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                       @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                       @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            return rgxEmail.IsMatch(email);
        }

        public static Image CropWithScail(Image imgPhoto, int Width, int Height)
        {
            Bitmap bmp = null;
            Graphics g;

            // Scale:
            double scaleY = (double)imgPhoto.Width / Width;
            double scaleX = (double)imgPhoto.Height / Height;
            double scale = scaleY < scaleX ? scaleX : scaleY;

            // Create new bitmap:
            bmp = new Bitmap(
                (int)((double)imgPhoto.Width / scale),
                (int)((double)imgPhoto.Height / scale));

            // Set resolution of the new image:
            if (imgPhoto.HorizontalResolution == 0 || imgPhoto.VerticalResolution == 0)
            {
                bmp.SetResolution(
                    74,
                    74);
            }
            else
            {
                bmp.SetResolution(
                    imgPhoto.HorizontalResolution,
                    imgPhoto.VerticalResolution);
            }

            // Create graphics:
            g = Graphics.FromImage(bmp);

            // Set interpolation mode:
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // Draw the new image:
            g.DrawImage(
                imgPhoto,
                new Rectangle(            // Destination
                    0, 0,
                    bmp.Width, bmp.Height),
                new Rectangle(            // Source
                    0, 0,
                    imgPhoto.Width, imgPhoto.Height),
                GraphicsUnit.Pixel);

            // Release the resources of the graphics:
            g.Dispose();

            // Release the resources of the origin image:
            imgPhoto.Dispose();

            return bmp;
            //}

        }

        public static Image Crop(Image img, float Width, float Height)
        {
            Bitmap bmp = null;
            Graphics g;

            // 비율에 맞춰 사이즈를 줄임
            double scaleY = (double)img.Width / Width;
            double scaleX = (double)img.Height / Height;
            double scale = scaleY > scaleX ? scaleX : scaleY;

            bmp = new Bitmap((int)((double)img.Width / scale), (int)((double)img.Height / scale));
            if (img.HorizontalResolution == 0 || img.VerticalResolution == 0)
            {
                bmp.SetResolution(74, 74);
            }
            else
            {
                bmp.SetResolution(img.HorizontalResolution, img.VerticalResolution);
            }

            g = Graphics.FromImage(bmp);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(img,
                        new Rectangle(0, 0, bmp.Width, bmp.Height),
                        new Rectangle(0, 0, img.Width, img.Height),
                        GraphicsUnit.Pixel);
            g.Dispose();
            img.Dispose();

            // 비율에 줄인 이미지에 크롭을 함
            Image imgC = (Image)bmp;
            float sourceWidth = imgC.Width;
            float sourceHeight = imgC.Height;
            float targetWidth = (Width > sourceWidth) ? sourceWidth : Width;
            float targetHeight = (Height > sourceHeight) ? sourceHeight : Height;
            float x = (sourceWidth - targetWidth) / 2.0f;
            float y = (sourceHeight - targetHeight) / 2.0f;

            RectangleF cropArea1 = new RectangleF(x, y, targetWidth, targetHeight);
            Bitmap bmpImage = new Bitmap(imgC);
            bmp.Dispose();
            imgC.Dispose();
            Bitmap bmpCrop = bmpImage.Clone(cropArea1, bmpImage.PixelFormat);
            bmpImage.Dispose();

            return (Image)(bmpCrop);
        }

        //public static void CropForMagic(MagickImage img, float Width, float Height)
        //{
        //    // 비율에 맞춰 사이즈를 줄임
        //    double scaleY   = (double)img.Width / Width;
        //    double scaleX   = (double)img.Height / Height;
        //    double scale    = scaleY > scaleX ? scaleX : scaleY;

        //    int newWidth = (int)((double)img.Width / scale);
        //    int newHeight = (int)((double)img.Height / scale);

        //    img.Crop(newWidth, newHeight);
        //}

        public static bool GetVideoThumbnail(string pathToConvertor, string originFrom, string saveTo, string resolution)
        {
            string parameters = string.Format("-ss {0} -i {1} -an -f image2 -vframes 1 -y -s {3} {2}", 3, originFrom, saveTo, resolution);
            var processInfo = new ProcessStartInfo();
            processInfo.FileName = pathToConvertor;
            processInfo.Arguments = parameters;
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardOutput = true;

            string result = "";

            using (var process = new Process())
            {
                process.StartInfo = processInfo;
                process.Start();
                process.WaitForExit();
                result = process.StandardOutput.ReadToEnd();
                Console.WriteLine(result);
            }

            return File.Exists(saveTo);
        }

        public static bool ffmpeg(string exePath, string arguement, string saveTo)
        {

            var processInfo = new ProcessStartInfo();
            processInfo.FileName = exePath;
            processInfo.Arguments = arguement;
            processInfo.CreateNoWindow = false;
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardOutput = true;

            using (var process = new Process())
            {
                process.StartInfo = processInfo;
                process.Start();
                process.WaitForExit();
                process.Close();
                process.Dispose();
            }

            return File.Exists(saveTo);
        }

        public static void ffprobe(string exePath, string arguement, out string outString)
        {
            var processInfo = new ProcessStartInfo();
            processInfo.FileName = exePath;
            processInfo.Arguments = arguement;
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardOutput = true;

            using (var process = new Process())
            {
                process.StartInfo = processInfo;
                process.Start();
                outString = process.StandardOutput.ReadToEnd();
                //Console.WriteLine(outString);
                process.WaitForExit();
                process.Close();
                process.Dispose();
            }
        }

        // public static string GetCheckedValues(System.Web.UI.WebControls.CheckBoxList ckl)
        // {
        //     List<string> list = new List<string>();

        //     for (int i = 0; i < ckl.Items.Count; i++)
        //     {
        //         if(ckl.Items[i].Selected)
        //             list.Add(ckl.Items[i].Value);
        //     }

        //     return string.Join(",", list);
        // }

        public static string GetPhoneNumberWithhyphen(string number)
        {
            number = number.Replace("-", "");

            int r;

            if (!int.TryParse(number, out r))
                return "";

            if (number.Length < 10 || number.Length > 11)
            {
                return number;
            }

            if (number.Length == 10)
            {
                return number.Substring(0, 3) + "-" + number.Substring(3, 3) + "-" + number.Substring(6, 4);
            }
            else if (number.Length == 11)
            {
                return number.Substring(0, 3) + "-" + number.Substring(3, 4) + "-" + number.Substring(7, 4);
            }

            return number;
        }

        public static string GetIV()
        {
            AesCryptoServiceProvider aesCryptoProvider = new AesCryptoServiceProvider();

            try
            {
                aesCryptoProvider.GenerateIV();
                return ToHexString(aesCryptoProvider.IV);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public static string ToHexString(byte[] str)
        {
            var sb = new StringBuilder();

            var bytes = str;
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString();
        }

        public static byte[] FromHexString(string hexString)
        {
            var bytes = new byte[hexString.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return bytes;
        }

    }
}
