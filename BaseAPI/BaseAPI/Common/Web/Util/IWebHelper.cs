using Microsoft.AspNetCore.Html;
using System.Data;

namespace BaseAPI
{
    public interface IWebHelper
    {
        HttpContext HttpConextCurrent { get; set; }
        IConfiguration Configuration { get; set; }
        ILogger<WebHelper> Logger { get; set; }
        IWebHostEnvironment Env { get; set; }
        string GetRequest(string key, bool useHTML);
        string GetRequest(string key);
        object GetRequestByObject(string key);
        int GetRequestByInt(string key, int nullValue);
        int GetRequestByInt(string key);
        float GetRequestByFloat(string key, float nullValue);
        DateTime GetRequestByDateTime(string key, DateTime nullValue);
        DateTime GetRequestByDateTime(string key);
        double GetRequestByDouble(string key, double nullValue);
        float GetRequestByFloat(string key);
        double GetRequestByDouble(string asKey);
        string GetRequest(string key, string nullValue);
        int UserIdx();
        int UserIdx(string key_name);
        string ClientId();
        string ClientId(string key_name);
        int GetDeviceTypeIdx();
        string GetRequestWithEcryption(string asKey, string isEncryption);
        string AuthQueryString(int user_idx, string email = "", int expire_minutes = 10);
        string AuthQueryStringByArgs(string arg1, string arg2 = "", string arg3 = "", int expire_minutes = 10);
        string AuthQueryGroupString(string group_info_code, string email, int expire_minutes = 7200);
        string AuthQueryShareString(int share_idx, int user_idx, int event_idx, string arg3 = "share", int expire_minutes = 7200);
        IHtmlContent ForEach(IEnumerable<dynamic> items, Func<dynamic, IHtmlContent> template, int times = 1);
        IHtmlContent ForEach(DataRowCollection items, Func<DataRow, IHtmlContent> template);
        string GetUniqueString(int string_length = 32);
        string GetIpAddress();
        string GetCurrentUrl();
        string GetUserAgent();
        string GetUploadDirectory();
        string CurrentLanguage(DataRow drUser = null);
        bool CheckRouteChar(string routeChar_a, string ui_ver);
        string GetRootDir(string routeChar, string ui_ver);
    }
}
