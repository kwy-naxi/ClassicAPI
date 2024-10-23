using BaseAPI.Services.Base.DataObject;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BaseAPI.Services.Dac
{
    public class Dac_User : DataObject
    {
        public DataTable Select(string user_id, string password)
        {
            string query = @$"SELECT * FROM tbl_user";

            /*SqlParameter[] paramArray = new SqlParameter[1];

            paramArray[0] = new SqlParameter("@user_id", SqlDbType.NVarChar);
            paramArray[0].Value = user_id;*/

            DataTable dt = _dbAgent.Fill(query).Tables[0];
            return dt;
        }
    }
}
