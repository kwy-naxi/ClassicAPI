using BaseAPI.Services.Dac;
using System.Data;

namespace BaseAPI.Services.Biz
{
    public class Biz_User
    {
        private Dac_User _dac_user;

        public Biz_User()
        {
            _dac_user = new Dac_User();
        }

        public DataTable GetUser(string user_id, string password)
        {
            return _dac_user.Select(user_id, password);
        }
    }
}
