using System.Data;

namespace BaseAPI.Common.DbAgent
{
    public interface IDbAgent
    {
        string ConnectionString { get; set; }
        int Update(DataSet dsDataSet, string strAlias);
        DataSet FillDataSet(string strQuery, string strAlias, DataSet dsDataSet, IDataParameter[] paramArray, CommandType cmdType, out IDataParameterCollection paramCol);
        DataSet FillDataSet(string strQuery, string strAlias, DataSet dsDataSet, IDataParameter[] paramArray, CommandType cmdType);
        DataSet FillDataSet(string strQuery, string strAlias, DataSet dsDataSet, IDataParameter[] paramArray);
        DataSet FillDataSet(string strQuery, string strAlias, DataSet dsDataSet);
        DataSet FillDataSet(string strQuery, string strAlias);
        DataSet Fill(string strQuery, IDataParameter[] paramArray, CommandType cmdType);
        DataSet Fill(string strQuery, IDataParameter[] paramArray);
        DataSet Fill(string strQuery);
        DataTable FillTable(string strQuery, IDataParameter[] paramArray);
        DataTable FillTable(string strQuery);
        IDataReader ExecuteReader(string strQuery, IDataParameter[] paramArray, CommandType cmdType, out IDataParameterCollection paramCol);
        IDataReader ExecuteReader(string strQuery, IDataParameter[] paramArray, CommandType cmdType);
        IDataReader ExecuteReader(string strQuery, IDataParameter[] paramArray);
        IDataReader ExecuteReader(string strQuery);
        int ExecuteNonQuery(IDbConnection connection, IDbTransaction trx, string strQuery, IDataParameter[] paramArray, CommandType cmdType, out IDataParameterCollection col);
        int ExecuteNonQuery(IDbConnection connection, IDbTransaction trx, string strQuery, IDataParameter[] paramArray, CommandType cmdType);
        int ExecuteNonQuery(IDbConnection connection, IDbTransaction trx, string strQuery, IDataParameter[] paramArray);
        int ExecuteNonQuery(IDbConnection connection, IDbTransaction trx, string strQuery);
        int ExecuteNonQuery(string strQuery, IDataParameter[] paramArray, CommandType cmdType, out IDataParameterCollection col);
        int ExecuteNonQuery(string strQuery, IDataParameter[] paramArray, CommandType cmdType);
        int ExecuteNonQuery(string strQuery, IDataParameter[] paramArray);
        int ExecuteNonQuery(string strQuery);
        object ExecuteScalar(IDbConnection connection, IDbTransaction trx, string strQuery, IDataParameter[] paramArray, CommandType cmdType, out IDataParameterCollection col);
        object ExecuteScalar(string strQuery, IDataParameter[] paramArray, CommandType cmdType);
        object ExecuteScalar(IDbConnection connection, IDbTransaction trx, string strQuery, IDataParameter[] paramArray);
        object ExecuteScalar(string strQuery, IDataParameter[] paramArray);
        object ExecuteScalar(string strQuery);
    }
}
