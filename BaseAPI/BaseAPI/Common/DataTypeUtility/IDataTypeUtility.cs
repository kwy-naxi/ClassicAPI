namespace BaseAPI.Common.DataTypeUtility
{
    public interface IDataTypeUtility
    {
        string JSonString(string code, string status, string message, object data);
        string JSonString(string code, string status, string message, object data, object option);
    }
}
