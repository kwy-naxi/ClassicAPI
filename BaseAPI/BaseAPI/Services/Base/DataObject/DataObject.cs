using BaseAPI.Common.DbAgent;

namespace BaseAPI.Services.Base.DataObject
{
    public abstract class DataObject
    {
        protected IDbAgent _dbAgent;

        public DataObject()
        {
            string connectionString = "";
            if (AppServices.Config.DBProdYn.Equals("Y"))
            {
                connectionString = AppServices.Config.ConnectionStrings["MainDb"];
                _dbAgent = new MSSqlAgent(connectionString);
            }
            else
            {
                connectionString = AppServices.Config.ConnectionStringsDev["MainDb"];
                _dbAgent = new MSSqlAgent(connectionString);
            }
        }
    }
}
