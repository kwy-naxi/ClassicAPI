using BaseAPI.Common.Base;

using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Html;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Crmf;
using System.Data;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;
using System.Xml;
using BaseAPI.Common.Web.Util;
using BaseAPI.Common.DataTypeUtility;

namespace BaseAPI
{
    public class WebHelper : IWebHelper
    {
        public HttpContext HttpConextCurrent { get; set; }
        public IConfiguration Configuration { get; set; }
        public ILogger<WebHelper> Logger { get; set; }
        public IWebHostEnvironment Env { get; set; }

        private static object myLock = new object();
        private static WebHelper instance = null;

        private WebHelper()
        {

        }

        public static WebHelper GetInstance()
        {
            if (instance == null)
            {
                lock (myLock)
                {
                    if (instance == null)
                    {
                        instance = new WebHelper();
                    }
                }
            }
            return instance;
        }

        public WebHelper(IHttpContextAccessor httpContextAccessor,
                        IConfiguration configuration,
                        ILogger<WebHelper> logger,
                        IWebHostEnvironment env)
        {
            HttpConextCurrent = httpContextAccessor.HttpContext;
            Configuration = configuration;
            Logger = logger;
            Env = env;
        }

        public string CurrentLanguage(DataRow drUser = null)
        {
            if (AppServices.Config.MultiLanguageUseYn.Equals("Y"))
            {
                if (HttpConextCurrent.User.Identity.IsAuthenticated && drUser != null)
                {
                    if (drUser.Table.Columns.Contains("ui_language_type_key"))
                    {
                        var langValue = drUser.ItemValue("ui_language_type_key", AppServices.Config.DefaultLanguage);
                        HttpConextCurrent.Session.SetString("Lang", langValue);

                        return langValue;
                    }
                }
                else
                {
                    if (!HttpConextCurrent.Request.Cookies.TryGetValue("Lang", out var val1))
                    {
                        return AppServices.Config.DefaultLanguage;
                    }
                }

                var lang = HttpConextCurrent.Request.Cookies["Lang"].ToValue();

                var lang_arr = (new string[] { "en", "ko", "jp", "cn", "es", "en_new" }).Where(x => x.Equals(lang));

                if (lang_arr.Count() == 0)
                {
                    return AppServices.Config.DefaultLanguage;
                }

                return lang;
            }

            return "";
        }

        public static string GetRequest(string key, HttpContext httpContext)
        {
            try
            {
                if (httpContext.Request.ContentType == null)
                    httpContext.Request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";

                if (httpContext.Request.Method.Equals("GET"))
                {
                    return httpContext.Request.Query[key].ToValue();
                }
                else if (httpContext.Request.Form[key].ToValue().Equals(""))
                {
                    return httpContext.Request.Query[key].ToValue();
                }

                return httpContext.Request.Form[key].ToValue();
            }
            catch (Exception)
            {
                return httpContext.Request.Query[key].ToValue();
            }
        }

        public string GetRequest(string key, bool useHTML)
        {
            try
            {
                if (HttpConextCurrent.Request.ContentType == null)
                    HttpConextCurrent.Request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            }
            catch
            {

            }

            if (HttpConextCurrent.Request.Method.Equals("GET"))
            {
                return HttpConextCurrent.Request.Query[key].ToValue();
            }
            else
            {
                try
                {
                    if (HttpConextCurrent.Request.Form[key].ToValue().Equals(""))
                    {
                        return HttpConextCurrent.Request.Query[key].ToValue();
                    }

                    return HttpConextCurrent.Request.Form[key].ToValue();
                }
                catch
                {
                    return "";
                }
            }
        }

        public string GetRequest(string key)
        {
            return GetRequest(key, false);
        }
        public object GetRequestByObject(string key)
        {
            object obj = HttpConextCurrent.Request.Query[key];

            if (obj == null)
            {
                return DBNull.Value;
            }
            else
            {
                if (HttpConextCurrent.Request.Query[key].ToValue().Equals("")
                    || HttpConextCurrent.Request.Query[key].ToValue().Equals("0"))
                {
                    return DBNull.Value;
                }

                return HttpConextCurrent.Request.Query[key];
            }
        }

        public static string AddTelHyphen(string tel)
        {
            tel = tel.Replace("-", "");

            string t1 = string.Empty;
            string t2 = string.Empty;
            string t3 = string.Empty;
            string t_sum = string.Empty;

            if (tel.Length == 8)     //1588-xxxx
            {
                t1 = tel.Substring(0, 4);
                t2 = tel.Substring(4, 4);
                t_sum = t1 + "-" + t2;
            }
            else if (tel.Length == 9)    //02-xxx-xxxx
            {
                t1 = tel.Substring(0, 2);
                t2 = tel.Substring(2, 3);
                t3 = tel.Substring(5, 4);
                t_sum = t1 + "-" + t2 + "-" + t3;
            }
            else if (tel.Length == 10)
            {
                if (tel.Substring(0, 2) == "01")
                {
                    t1 = tel.Substring(0, 3);
                    t2 = tel.Substring(3, 3);
                    t3 = tel.Substring(6, 4);
                    t_sum = t1 + "-" + t2 + "-" + t3;
                }
                else if (tel.Substring(0, 2) == "02")
                {
                    t1 = tel.Substring(0, 2);
                    t2 = tel.Substring(2, 4);
                    t3 = tel.Substring(6, 4);
                    t_sum = t1 + "-" + t2 + "-" + t3;
                }
                else
                {
                    t1 = tel.Substring(0, 3);
                    t2 = tel.Substring(3, 3);
                    t3 = tel.Substring(6, 4);
                    t_sum = t1 + "-" + t2 + "-" + t3;
                }
            }
            else if (tel.Length == 11)   //xxx-xxxx-xxxx(휴대전화,070)
            {
                t1 = tel.Substring(0, 3);
                t2 = tel.Substring(3, 4);
                t3 = tel.Substring(7, 4);
                t_sum = t1 + "-" + t2 + "-" + t3;
            }

            return t_sum;
        }

        public int GetRequestByInt(string key, int nullValue)
        {
            if (HttpConextCurrent.Request.ContentType == null)
                HttpConextCurrent.Request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";

            int a;
            string v;
            if (HttpConextCurrent.Request.Method.Equals("GET"))
            {
                v = HttpConextCurrent.Request.Query[key].ToValue();
                if (int.TryParse(v, out a))
                {
                    return a;
                }
            }
            else
            {
                try
                {
                    if (HttpConextCurrent.Request.Method.Equals("POST"))
                    {
                        v = HttpConextCurrent.Request.Form[key].ToValue();

                        if (v.Equals(""))
                        {
                            v = HttpConextCurrent.Request.Query[key].ToValue();
                        }
                    }
                    else
                    {
                        v = HttpConextCurrent.Request.Form[key].ToValue();
                    }

                    if (int.TryParse(v, out a))
                    {
                        return a;
                    }
                }
                catch
                {
                    return nullValue;
                }
            }

            return nullValue;
        }

        public int GetRequestByInt(string key)
        {
            return GetRequestByInt(key, 0);
        }

        public float GetRequestByFloat(string key, float nullValue)
        {
            if (HttpConextCurrent.Request.ContentType == null)
                HttpConextCurrent.Request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";

            float a;
            string v;
            if (HttpConextCurrent.Request.Method.Equals("GET"))
                v = HttpConextCurrent.Request.Query[key].ToValue();
            else
            {
                if (HttpConextCurrent.Request.Form[key].Count > 0)
                    v = HttpConextCurrent.Request.Form[key].ToValue();
                else
                    v = HttpConextCurrent.Request.Query[key].ToValue();
            }


            if (float.TryParse(v, out a))
            {
                return a;
            }

            return nullValue;
        }

        public DateTime GetRequestByDateTime(string key, DateTime nullValue)
        {
            DateTime a;
            string v;
            if (HttpConextCurrent.Request.Method.Equals("GET"))
                v = HttpConextCurrent.Request.Query[key].ToValue();
            else
                v = HttpConextCurrent.Request.Form[key].ToValue();
            if (DateTime.TryParse(v, out a))
            {
                return a;
            }

            return nullValue;
        }

        public DateTime GetRequestByDateTime(string key)
        {
            return GetRequestByDateTime(key, DateTime.MinValue);
        }

        public double GetRequestByDouble(string key, double nullValue)
        {
            double a;
            string v;
            if (HttpConextCurrent.Request.Method.Equals("GET"))
                v = HttpConextCurrent.Request.Query[key].ToValue();
            else
                v = HttpConextCurrent.Request.Form[key].ToValue();
            if (double.TryParse(v, out a))
            {
                return a;
            }

            return nullValue;
        }

        public float GetRequestByFloat(string key)
        {
            return GetRequestByFloat(key, 0);
        }

        public double GetRequestByDouble(string asKey)
        {
            return GetRequestByDouble(asKey, 0);
        }

        public string GetRequest(string key, string nullValue)
        {
            string v;

            try
            {
                if (HttpConextCurrent.Request.Method.Equals("GET"))
                    v = HttpConextCurrent.Request.Query[key].ToValue();
                else if (HttpConextCurrent.Request.Form[key].ToValue().Equals(""))
                    v = HttpConextCurrent.Request.Query[key].ToValue();
                else
                    v = HttpConextCurrent.Request.Form[key].ToValue();

                return (v.Equals("") ? nullValue : v);
            }
            catch
            {
                v = HttpConextCurrent.Request.Query[key].ToValue();
                return (v.Equals("") ? nullValue : v);
            }
        }

        public int UserIdx()
        {
            return UserIdx("user_idx");
        }

        public int UserIdx(string key_name)
        {
            if (this.HttpConextCurrent == null)
                return 0;

            int a;
            if (!int.TryParse(this.HttpConextCurrent.User.Identity.Name, out a))
            {
                return 0;
            }

            //if (Env.IsDevelopment())
            //{
            //    if (this.GetRequestByInt(key_name) == 0)
            //    {
            //        return this.HttpConextCurrent.User.Identity.Name.ToInt();
            //    }
            //    else
            //    {
            //        return this.GetRequestByInt(key_name, this.HttpConextCurrent.User.Identity.Name.ToInt());
            //    }
            //}

            return this.HttpConextCurrent.User.Identity.Name.ToInt();
        }

        public string ClientId(string key_name)
        {
            if (Env.IsDevelopment())
            {
                if (this.GetRequest(key_name) == "")
                {
                    return this.HttpConextCurrent.User.Identity.Name.ToValue();
                }
                else
                {
                    return this.GetRequest(key_name, this.HttpConextCurrent.User.Identity.Name.ToValue());
                }
            }

            return this.HttpConextCurrent.User.Identity.Name.ToValue();
        }


        public string ClientId()
        {
            return ClientId("client_id");
        }

        public string GetCurrentUrl()
        {
            var location = new Uri($"{AppServices.HttpContextCurrent.Request.Scheme}://{AppServices.HttpContextCurrent.Request.Host}{AppServices.HttpContextCurrent.Request.Path}{AppServices.HttpContextCurrent.Request.QueryString}");
            return location.AbsoluteUri.ToLower();
        }


        public static string GetHtmlEncodeChar(string asStr)
        {
            asStr = asStr.Replace(";", "&#59;");
            asStr = asStr.Replace("&", "&#38;");
            asStr = asStr.Replace("\\", "");
            asStr = asStr.Replace("\"", "&#34;");
            asStr = asStr.Replace("(", "&#40;");
            asStr = asStr.Replace(")", "&#41;");
            asStr = asStr.Replace("'", "");

            return asStr;
        }

        public static string GetValueForSplit(DataTable dataTable
                                            , string dataColumnName
                                            , string splitChar)
        {
            string temp = "";
            bool isFirst = true;

            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (isFirst)
                {
                    temp += dataRow[dataColumnName].ToString();
                    isFirst = false;
                }
                else
                {
                    temp += string.Format("{0}{1}", splitChar, dataRow[dataColumnName]);
                }
            }

            return temp;
        }

        public static async Task<string> GetBodyStringAsync(HttpRequest httpRequest)
        {
            using (var reader = new StreamReader(httpRequest.Body, Encoding.UTF8))
            {
                return await reader.ReadToEndAsync();
            }
        }

        public static string GetBodyString(HttpRequest httpRequest)
        {
            using (var reader = new StreamReader(httpRequest.Body, Encoding.UTF8))
            {
                return reader.ReadToEnd().ToValue();
            }
        }

        public static string RemoveHTMLTags(string content)
        {
            var cleaned = string.Empty;
            try
            {
                StringBuilder textOnly = new StringBuilder();
                using (var reader = XmlNodeReader.Create(new System.IO.StringReader("<xml>" + content + "</xml>")))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Text)
                            textOnly.Append(reader.ReadContentAsString());
                    }
                }
                cleaned = textOnly.ToString();
            }
            catch
            {
                string textOnly = string.Empty;
                Regex tagRemove = new Regex(@"<[^>]*(>|$)");
                Regex compressSpaces = new Regex(@"[\s\r\n]+");
                textOnly = tagRemove.Replace(content, string.Empty);
                textOnly = compressSpaces.Replace(textOnly, " ");
                cleaned = textOnly;
            }

            return cleaned;
        }

        public static string GetRepeat(string source, int count)
        {
            string temp = "";

            for (int i = 0; i < count; i++)
            {
                temp += source;
            }

            return temp;
        }

        public static string GetPager(int page, int pageCount, string url)
        {
            if (pageCount == 0)
                return "";

            bool isFirstQS = true;
            int last_page = 0;

            if (url.Contains("?"))
                isFirstQS = false;

            string html = "";
            html += "<div class='page'>";
            html += "<ul class='pagination pagination-sm'>\n";

            if (page == 1)
            {
                html += string.Format("\t\t<li class='disabled'><a hreF='{0}'><span>처음</span></a></li>", "javascript:void(0)");
                html += string.Format("\t\t<li class='disabled'><a href='{0}'><span>&laquo;</span></a></li>", "javascript:void(0)");
            }
            else
            {
                html += string.Format("\t\t<li><a hreF='{0}'><span>처음</span></a></li>", url + ((isFirstQS) ? "?page=" : "&page=") + (1).ToString());

                if (page > 5)
                {
                    int page_f = (page - 6) / 10;
                    int page_n = page_f * 10 + 1;

                    html += string.Format("\t\t<li><a href='{0}'>{1}</a></li>", url + ((isFirstQS) ? "?page=" : "&page=") + page_n, page_n);
                    html += string.Format("\t\t<li class='active'><a href='{0}'>...</a></li>", "javascript:void(0)");
                    html += string.Format("\t\t<li><a href='{0}'><span>&laquo;</span></a></li>", url + ((isFirstQS) ? "?page=" : "&page=") + (page - 1).ToString());
                }
                else
                {
                    html += string.Format("\t\t<li><a href='{0}'><span>&laquo;</span></a></li>", url + ((isFirstQS) ? "?page=" : "&page=") + (page - 1).ToString());
                }
            }

            int a;

            if (!int.TryParse(page.ToString(), out a))
                page = 1;

            if (!int.TryParse(pageCount.ToString(), out a))
                pageCount = 0;

            if (page < 0)
                page = 1;
            else if (page > pageCount)
                page = pageCount;

            if (page - 2 <= 0)
            {
                for (int i = 1; i <= Math.Min(pageCount, 5); i++)
                {
                    html += string.Format("\t\t<li {2}><a href='{0}'>{1}</a></li>", (i != page) ? url + ((isFirstQS) ? "?page=" : "&page=") + i.ToString() : "javascript:void(0)", i, (i != page) ? "" : "class='active'");

                    if (i == Math.Min(pageCount, 5))
                        last_page = i;
                }
            }
            else if (page + 2 > pageCount)
            {
                for (int i = pageCount - Math.Min(pageCount - 1, 5); i <= pageCount; i++)
                {
                    html += string.Format("\t\t<li {2}><a href='{0}'>{1}</a></li>", (i != page) ? url + ((isFirstQS) ? "?page=" : "&page=") + i.ToString() : "javascript:void(0)", i, (i != page) ? "" : "class='active'");

                    if (i == pageCount)
                        last_page = i;
                }
            }
            else
            {
                for (int i = 0; i < Math.Min(pageCount, 5); i++)
                {
                    html += string.Format("\t\t<li {2}><a href='{0}'>{1}</a></li>", (page - 2 + i != page) ? url + ((isFirstQS) ? "?page=" : "&page=") + (page - 2 + i).ToString() : "javascript:void(0)", page - 2 + i, (page - 2 + i != page) ? "" : "class='active'");

                    if (i == Math.Min(pageCount, 5) - 1)
                        last_page = page - 2 + i;
                }
            }

            if (page == pageCount)
            {
                html += string.Format("\t\t<li class='disabled'><a href='{0}'><span>&raquo;</span></a></li>", "javascript:void(0)");
                html += string.Format("\t\t<li class='disabled'><a hreF='{0}'><span>마지막</span></a></li>", "javascript:void(0)");
            }
            else
            {
                html += string.Format("\t\t<li><a href='{0}'><span>&raquo;</span></a></li>", url + ((isFirstQS) ? "?page=" : "&page=") + (page + 1).ToString());
                html += string.Format("\t\t<li><a hreF='{0}'><span>마지막</span></a></li>", url + ((isFirstQS) ? "?page=" : "&page=") + (pageCount).ToString());
            }

            html += "</ul>\n";
            html += "</div>\n";

            return html;
        }

        public static string StripTags(string HTML)
        {
            string Val = Regex.Replace(HTML, "<[^>]*>", "");
            return Val;
        }

        public static string Encrypt(string password)
        {
            UnicodeEncoding uEncode = new UnicodeEncoding();
            byte[] bytPassword = uEncode.GetBytes(password);
            SHA512Managed sha = new SHA512Managed();
            byte[] hash = sha.ComputeHash(bytPassword);
            return Convert.ToBase64String(hash).Replace("+", "#");
        }

        public static string GetCompCode()
        {
            string strHostName = "";
            strHostName = Dns.GetHostName();
            return strHostName;
        }

        public static void Write(object str)
        {

        }

        public static string GetSearchText(string searchText)
        {
            string[] strArr = searchText.Replace("+", " ").Split(' ');
            string temp = "";
            bool isFirst = true;

            foreach (string str in strArr)
            {
                if (str.Equals(""))
                    continue;

                if (isFirst)
                {
                    temp += string.Format("\"{0}*\"", str);
                    isFirst = false;
                }
                else
                {
                    temp += string.Format("AND \"{0}*\"", str);
                }
            }

            return temp;
        }

        public string GetRequestWithEcryption(string asKey, string isEncryption)
        {
            if (isEncryption.Equals("Y"))
            {
                return GetRequest(asKey, false);
            }

            return GetRequest(asKey, false);
        }


        public int GetDeviceTypeIdx()
        {
            try
            {
                var userAgent = AppServices.HttpContextCurrent.Request.Headers["Referer"].ToValue();

                if (userAgent.Contains("ipad") ||
                    userAgent.Contains("iphone") ||
                    userAgent.Contains("ios"))
                    return 1;

                return 2;
            }
            catch
            {
                return 2;
            }
        }

        public static string GetDifferenceDate(DateTime startDate)
        {
            DateTime endTime = DateTime.Now;

            TimeSpan span = endTime.Subtract(startDate);

            int sec = (int)span.TotalSeconds;

            if (sec < 60)
                return string.Format("{0}초 전", sec);
            else if (sec < 60 * 60)
                return string.Format("{0}분 전", sec / 60);
            else if (sec < 60 * 60 * 24)
                return string.Format("{0}시간 전", sec / (60 * 60));
            else if (sec < 60 * 60 * 24 * 30)
                return string.Format("{0}일 전", sec / (60 * 60 * 24));
            else if (sec < 60 * 60 * 24 * 30 * 12)
                return string.Format("{0}달 전", sec / (60 * 60 * 24 * 30));

            return string.Format("{0}년 전", sec / (60 * 60 * 24 * 30 * 12));
        }

        [Obsolete]
        public static string WebEncrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(clearText);
            using (System.Security.Cryptography.Aes encryptor = System.Security.Cryptography.Aes.Create())
            {
                System.Security.Cryptography.Rfc2898DeriveBytes pdb = new System.Security.Cryptography.Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    using (System.Security.Cryptography.CryptoStream cs = new System.Security.Cryptography.CryptoStream(ms, encryptor.CreateEncryptor(), System.Security.Cryptography.CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        [Obsolete]
        public static string WebDecrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (System.Security.Cryptography.Aes encryptor = System.Security.Cryptography.Aes.Create())
            {
                System.Security.Cryptography.Rfc2898DeriveBytes pdb = new System.Security.Cryptography.Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    using (System.Security.Cryptography.CryptoStream cs = new System.Security.Cryptography.CryptoStream(ms, encryptor.CreateDecryptor(), System.Security.Cryptography.CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = System.Text.Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public static string GetFileNameFromUrl(string url)
        {
            Uri baseUri = new Uri(AppServices.Config.WebsiteUrl);
            Uri uri;
            if (!Uri.TryCreate(url, UriKind.Absolute, out uri))
                uri = new Uri(baseUri, url);
            return Path.GetFileName(uri.LocalPath);
        }

        public string GetUserAgent()
        {
            var user_agent = AppServices.HttpContextCurrent.Request.Headers["User-Agent"].ToString();
            return user_agent;
        }

        public static Dictionary<string, string> JwtDecoder(string token)
        {
            return null;
        }

        public string AuthQueryString(int user_idx, string email = "", int expire_minutes = 10)
        {
            EncryptedQueryString args = new EncryptedQueryString();
            args["user_idx"] = DataTypeUtility.GetValue(user_idx);
            args["expire"] = DateTime.Now.AddMinutes(expire_minutes).ToFormatString();

            if (!email.ToValue().Equals(""))
                args["email"] = email;

            return args.ToString();
        }

        public string AuthQueryStringByArgs(string arg1, string arg2 = "", string arg3 = "", int expire_minutes = 10)
        {
            EncryptedQueryString args = new EncryptedQueryString();
            args["arg1"] = arg1;
            args["arg2"] = arg2;
            args["arg3"] = arg3;
            args["expire"] = DateTime.Now.AddMinutes(expire_minutes).ToFormatString();

            return args.ToString();
        }

        public string AuthQueryGroupString(string group_info_code, string email, int expire_minutes = 4320)
        {
            EncryptedQueryString args = new EncryptedQueryString();
            args["group_info_code"] = DataTypeUtility.GetValue(group_info_code);
            args["email"] = DataTypeUtility.GetValue(email);
            args["expire"] = DateTime.Now.AddMinutes(expire_minutes).ToFormatString();

            return args.ToString();
        }

        public string AuthQueryShareString(int share_idx, int user_idx, int event_idx, string arg3 = "share", int expire_minutes = 4320)
        {
            EncryptedQueryString args = new EncryptedQueryString();
            args["share_idx"] = DataTypeUtility.GetValue(share_idx);
            args["user_idx"] = DataTypeUtility.GetValue(user_idx);
            args["event_idx"] = DataTypeUtility.GetValue(event_idx);
            args["arg3"] = DataTypeUtility.GetValue(arg3);
            args["expire"] = DateTime.Now.AddMinutes(expire_minutes).ToFormatString();

            return args.ToString();
        }

        public static string EncryptDecrypt(string input)
        {
            char[] key = { 'K', 'C', 'Q' };
            char[] output = new char[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                output[i] = (char)(input[i] ^ key[i % key.Length]);
            }

            return new string(output);
        }

        public IHtmlContent ForEach(IEnumerable<dynamic> items, Func<dynamic, IHtmlContent> template, int times = 1)
        {
            var html = new HtmlContentBuilder();

            foreach (var item in items)
            {
                for (var i = 0; i < times; i++)
                {
                    _ = html.AppendHtml(template(item));
                }
            }

            return html;
        }

        public IHtmlContent ForEach(DataRowCollection items, Func<DataRow, IHtmlContent> template)
        {
            var html = new HtmlContentBuilder();

            foreach (DataRow item in items)
                html.AppendHtml(template(item));

            return html;
        }

        public string GetUniqueString(int string_length = 32)
        {
#pragma warning disable SYSLIB0023 // Type or member is obsolete
            using (var rng = new RNGCryptoServiceProvider())
            {
                var bit_count = (string_length * 6);
                var byte_count = ((bit_count + 7) / 8); // rounded up
                var bytes = new byte[byte_count];
                rng.GetBytes(bytes);
                return Convert.ToBase64String(bytes);
            }
#pragma warning restore SYSLIB0023 // Type or member is obsolete
        }

        public static string GetStringFromDictionary(Dictionary<string, string> dic)
        {
            return string.Join(";", dic.Select(x => x.Key + "=" + x.Value).ToArray());
        }

        public static Dictionary<string, string> GetDictionaryFromString(string str)
        {
            return str.Split(';')
                .Select(part => part.Split('='))
                .Where(part => part.Length == 2)
                .ToDictionary(sp => sp[0], sp => sp[1]);
        }

        public static void CookieSet(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();
            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);
            AppServices.HttpContextCurrent.Response.Cookies.Append(key, value, option);
        }

        public static void CookieRemove(string key)
        {
            CookieOptions option = new CookieOptions();
            AppServices.HttpContextCurrent.Response.Cookies.Delete(key);
        }

        public static string CookieGet(string key, string defaultValue)
        {
            string output;

            if (!AppServices.HttpContextCurrent.Request.Cookies.TryGetValue(key, out output))
            {
                CookieSet(key, defaultValue, null);
                return defaultValue;
            }

            return output;
        }

        public static string GenCouponCode(string prefix = "SC", int length = 13)
        {
            return $"{prefix}{RandomString(13)}";
        }

        public static string RandomString(int length = 6, string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
        {
            if (length < 0) throw new ArgumentOutOfRangeException("length", "length cannot be less than zero.");
            if (string.IsNullOrEmpty(allowedChars)) throw new ArgumentException("allowedChars may not be empty.");

            const int byteSize = 0x100;
            var allowedCharSet = new HashSet<char>(allowedChars).ToArray();
            if (byteSize < allowedCharSet.Length) throw new ArgumentException(String.Format("allowedChars may contain no more than {0} characters.", byteSize));

            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                var result = new StringBuilder();
                var buf = new byte[128];
                while (result.Length < length)
                {
                    rng.GetBytes(buf);
                    for (var i = 0; i < buf.Length && result.Length < length; ++i)
                    {
                        var outOfRangeStart = byteSize - (byteSize % allowedCharSet.Length);
                        if (outOfRangeStart <= buf[i]) continue;
                        result.Append(allowedCharSet[buf[i] % allowedCharSet.Length]);
                    }
                }
                return result.ToString();
            }
        }

        public string GetIpAddress()
        {
            try
            {
                IPHostEntry heserver = Dns.GetHostEntry(Dns.GetHostName());
                var a = heserver.AddressList.ToList().Where(p => p.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork && p.ToValue() != "::1");
                //var a = heserver.AddressList.ToList().Where(p => p.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);

                var list = new List<string>();

                foreach (var addr in a)
                {
                    list.Add(addr.ToValue());
                }

                return string.Join(',', list);
            }
            catch (Exception)
            {
                var ip1 = (AppServices.HttpContextCurrent.Response.HttpContext.GetServerVariable("HTTP_X_FORWARDED_FOR") != null
                            && AppServices.HttpContextCurrent.Response.HttpContext.GetServerVariable("HTTP_X_FORWARDED_FOR") != "")
                            ? AppServices.HttpContextCurrent.Response.HttpContext.GetServerVariable("HTTP_X_FORWARDED_FOR")
                            : AppServices.HttpContextCurrent.Response.HttpContext.GetServerVariable("REMOTE_ADDR");

                if (ip1.ToValue() != "")
                {
                    if (ip1.ToValue().Contains(","))
                        ip1 = ip1.Split(',').First().Trim();

                    return ip1;
                }
                return "127.0.0.1";
            }
        }

        public string GetUploadDirectory()
        {
            if (AppServices.Config.UploadDir.AppendDevName().Contains("[UserProfile]"))
            {
                var dir1 = AppServices.Config.UploadDir.AppendDevName().Replace("[UserProfile]", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
                if (!Directory.Exists(dir1))
                {
                    Directory.CreateDirectory(dir1);
                }

                return dir1;
            }

            var dir2 = AppServices.Config.UploadDir.AppendDevName();

            if (!Directory.Exists(dir2))
            {
                Directory.CreateDirectory(dir2);
            }

            return dir2;
        }

        public string GetCurrentUrl(HttpRequest request)
        {
            return $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
        }

        public bool CheckRouteChar(string routeChar_a, string ui_ver)
        {
            var routeChar = AppServices.WebHelper.GetRootDir(routeChar_a, ui_ver);
            var request = AppServices.HttpContextCurrent.Request;
            var current_path = request.Path.ToValue();

            if (current_path.Equals("/") ||
                current_path.Equals("/home") ||
                current_path.Equals(""))
            {
                return true;
            }

            if (!current_path.Contains(routeChar))
            {
                return false;
            }

            if (routeChar.Equals(routeChar_a))
            {
                if ((new Regex($"{routeChar_a}v[0-9]{1,2}")).IsMatch(current_path))
                {
                    return false;
                }
            }
            else if (routeChar.Equals("m/"))
            {
                if (current_path.Contains($"/{routeChar_a}"))
                {
                    return false;
                }
            }

            return true;
        }

        public string GetRootDir(string routeChar, string ui_ver)
        {
            if (!ui_ver.Equals(""))
            {
                ui_ver = $"{ui_ver}/";
            }
            var root_dir = $"{routeChar}{ui_ver}";

            if (root_dir.Equals(""))
                root_dir = $"m/";

            return root_dir;
        }

        public static string GetIP1()
        {
            string localIP = "";
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }

            return localIP;
        }

        [SupportedOSPlatform("windows")]
        public static string GetIP2()
        {
            UnicastIPAddressInformation mostSuitableIp = null;

            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var network in networkInterfaces)
            {
                if (network.OperationalStatus != OperationalStatus.Up)
                    continue;

                var properties = network.GetIPProperties();

                if (properties.GatewayAddresses.Count == 0)
                    continue;

                foreach (var address in properties.UnicastAddresses)
                {
                    if (address.Address.AddressFamily != AddressFamily.InterNetwork)
                        continue;

                    if (IPAddress.IsLoopback(address.Address))
                        continue;

                    if (!address.IsDnsEligible)
                    {
                        if (mostSuitableIp == null)
                            mostSuitableIp = address;
                        continue;
                    }

                    if (address.PrefixOrigin != PrefixOrigin.Dhcp)
                    {
                        if (mostSuitableIp == null || !mostSuitableIp.IsDnsEligible)
                            mostSuitableIp = address;
                        continue;
                    }

                    return address.Address.ToString();
                }
            }

            return mostSuitableIp != null
                ? mostSuitableIp.Address.ToString()
                : "";
        }

        public static string GetIP3()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static bool ValidEmail(string email)
        {
            var em = new System.ComponentModel.DataAnnotations.EmailAddressAttribute();
            return em.IsValid(email);
        }

        public static bool ValidPhone(string phone_number)
        {
            string regExPattern1 = @"010-?[0-9]{4}-?[0-9]{4}$";
            string regExPattern4 = @"\+[0-9]{2,3} [0-1]{2,3}-?[0-9]{4}-?[0-9]{4}$";

            var pattern1 = new Regex(regExPattern1);
            var val1 = pattern1.IsMatch(phone_number);

            var pattern4 = new Regex(regExPattern4);
            var val4 = pattern4.IsMatch(phone_number);

            return val1 | val4;
        }
    }
}
