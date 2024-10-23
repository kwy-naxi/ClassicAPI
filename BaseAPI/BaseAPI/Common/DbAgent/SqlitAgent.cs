﻿using System.Data;
using System.Data.SQLite;

namespace BaseAPI.Common.DbAgent
{
    public class SqlitAgent : IDbAgent
    {
        private SQLiteConnection _connection = null;
        private SQLiteDataAdapter _dataAdapter = null;
        private SQLiteCommandBuilder _sqlCom = null;

        public SqlitAgent()
        {
            _connection = new SQLiteConnection();
        }

        public SqlitAgent(string connectStr)
        {
            _connection = new SQLiteConnection(connectStr);
        }

        public SqlitAgent(SQLiteConnection connection)
        {
            _connection = connection;
        }

        public SQLiteDataAdapter DataAdapter
        {
            get
            {
                return _dataAdapter;
            }
            set
            {
                if (_dataAdapter == value)
                    return;
                _dataAdapter = value;
            }
        }

        public string ConnectionString
        {
            get
            {
                if (_connection.ConnectionString == null)
                    return "";

                return _connection.ConnectionString;
            }
            set
            {
                this._connection.ConnectionString = value;
            }
        }

        public int Update(DataSet dsDataSet, string strAlias)
        {
            int returnVal = _dataAdapter.Update(dsDataSet, strAlias);
            return returnVal;
        }

        public DataSet FillDataSet(string strQuery, string strAlias, DataSet dsDataSet, IDataParameter[] paramArray, CommandType cmdType, out IDataParameterCollection paramCol)
        {
            _dataAdapter = new SQLiteDataAdapter(strQuery, _connection);
            _sqlCom = new SQLiteCommandBuilder(_dataAdapter);

            _dataAdapter.SelectCommand.CommandType = cmdType;
            _dataAdapter.SelectCommand.CommandTimeout = 0;
            if (dsDataSet == null)
                dsDataSet = new DataSet();

            if (paramArray != null)
            {
                foreach (IDataParameter param in paramArray)
                {
                    _dataAdapter.SelectCommand.Parameters.Add(param);
                }
            }
            _dataAdapter.Fill(dsDataSet, strAlias);
            paramCol = _dataAdapter.SelectCommand.Parameters;

            return dsDataSet;
        }

        public DataSet FillDataSet(string strQuery, string strAlias, DataSet dsDataSet, IDataParameter[] paramArray, CommandType cmdType)
        {
            IDataParameterCollection col = null;
            return FillDataSet(strQuery, strAlias, dsDataSet, paramArray, cmdType, out col);
        }

        public DataSet FillDataSet(string strQuery, string strAlias, DataSet dsDataSet, IDataParameter[] paramArray)
        {
            IDataParameterCollection col = null;
            return FillDataSet(strQuery, strAlias, dsDataSet, paramArray, CommandType.Text, out col);
        }

        public DataSet FillDataSet(string strQuery, string strAlias, DataSet dsDataSet)
        {
            IDataParameterCollection col = null;
            return FillDataSet(strQuery, strAlias, dsDataSet, null, CommandType.Text, out col);
        }

        public DataSet FillDataSet(string strQuery, string strAlias)
        {
            IDataParameterCollection col = null;
            return FillDataSet(strQuery, strAlias, null, null, CommandType.Text, out col);
        }

        public DataSet Fill(string strQuery, IDataParameter[] paramArray, CommandType cmdType)
        {
            string strAlias = "DefaultTBL";

            return FillDataSet(strQuery, strAlias, null, paramArray, cmdType);
        }

        public DataSet Fill(string strQuery, IDataParameter[] paramArray)
        {
            return Fill(strQuery, paramArray, CommandType.Text);
        }

        public DataSet Fill(string strQuery)
        {
            return Fill(strQuery, null);
        }

        public DataTable FillTable(string strQuery, IDataParameter[] paramArray)
        {
            return Fill(strQuery, paramArray, CommandType.Text).Tables[0];
        }

        public DataTable FillTable(string strQuery)
        {
            return FillTable(strQuery, null);
        }

        public IDataReader ExecuteReader(string strQuery, IDataParameter[] paramArray, CommandType cmdType, out IDataParameterCollection paramCol)
        {
            SQLiteDataReader reader = null;
            paramCol = null;

            SQLiteCommand cmd = new SQLiteCommand(strQuery, _connection);
            cmd.CommandType = cmdType;

            if (paramArray != null)
            {
                foreach (IDataParameter param in paramArray)
                {
                    cmd.Parameters.Add(param);
                }
            }

            if (_connection.State == ConnectionState.Open)
                _connection.Close();

            _connection.Open();
            reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            paramCol = cmd.Parameters;

            return reader;
        }

        public IDataReader ExecuteReader(string strQuery, IDataParameter[] paramArray, CommandType cmdType)
        {
            IDataParameterCollection col = null;
            return ExecuteReader(strQuery, paramArray, cmdType, out col);
        }

        public IDataReader ExecuteReader(string strQuery, IDataParameter[] paramArray)
        {
            IDataParameterCollection col = null;
            return ExecuteReader(strQuery, paramArray, CommandType.Text, out col);
        }

        public IDataReader ExecuteReader(string strQuery)
        {
            IDataParameterCollection col = null;
            return ExecuteReader(strQuery, null, CommandType.Text, out col);
        }

        public int ExecuteNonQuery(IDbConnection connection, IDbTransaction trx, string strQuery, IDataParameter[] paramArray, CommandType cmdType, out IDataParameterCollection col)
        {
            SQLiteCommand cmd = null;

            int nRc = 0;
            col = null;

            if (connection == null)
                cmd = new SQLiteCommand(strQuery, _connection);
            else
                cmd = new SQLiteCommand(strQuery, (SQLiteConnection)connection);

            if (trx != null)
            {
                cmd.Transaction = (SQLiteTransaction)trx;
            }

            cmd.CommandTimeout = 0;
            cmd.CommandType = cmdType;

            if (paramArray != null)
            {
                foreach (IDataParameter param in paramArray)
                {
                    cmd.Parameters.Add(param);
                }
            }

            try
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();

                if (connection == null)
                    _connection.Open();

                nRc = cmd.ExecuteNonQuery();
            }
            catch (SQLiteException)
            {
                throw;
            }
            finally
            {
                col = cmd.Parameters;

                if (connection == null)
                    _connection.Close();
            }

            return nRc;
        }

        public int ExecuteNonQuery(IDbConnection connection, IDbTransaction trx, string strQuery, IDataParameter[] paramArray, CommandType cmdType)
        {
            IDataParameterCollection col = null;
            return ExecuteNonQuery(connection, trx, strQuery, paramArray, cmdType, out col);
        }

        public int ExecuteNonQuery(IDbConnection connection, IDbTransaction trx, string strQuery, IDataParameter[] paramArray)
        {
            IDataParameterCollection col = null;
            return ExecuteNonQuery(connection, trx, strQuery, paramArray, CommandType.Text, out col);
        }

        public int ExecuteNonQuery(IDbConnection connection, IDbTransaction trx, string strQuery)
        {
            IDataParameterCollection col = null;
            return ExecuteNonQuery(connection, trx, strQuery, null, CommandType.Text, out col);
        }

        public int ExecuteNonQuery(string strQuery, IDataParameter[] paramArray, CommandType cmdType, out IDataParameterCollection col)
        {
            return ExecuteNonQuery(null, null, strQuery, paramArray, cmdType, out col);
        }

        public int ExecuteNonQuery(string strQuery, IDataParameter[] paramArray, CommandType cmdType)
        {
            IDataParameterCollection col = null;
            return ExecuteNonQuery(null, null, strQuery, paramArray, cmdType, out col);
        }

        public int ExecuteNonQuery(string strQuery, IDataParameter[] paramArray)
        {
            IDataParameterCollection col = null;
            return ExecuteNonQuery(null, null, strQuery, paramArray, CommandType.Text, out col);
        }

        public int ExecuteNonQuery(string strQuery)
        {
            IDataParameterCollection col = null;
            return ExecuteNonQuery(null, null, strQuery, null, CommandType.Text, out col);
        }

        public object ExecuteScalar(IDbConnection connection, IDbTransaction trx, string strQuery, IDataParameter[] paramArray, CommandType cmdType, out IDataParameterCollection col)
        {
            SQLiteCommand cmd = null;

            object oRc = null;
            col = null;

            if (connection == null)
                cmd = new SQLiteCommand(strQuery, _connection);
            else
                cmd = new SQLiteCommand(strQuery, (SQLiteConnection)connection);

            if (trx != null)
            {
                cmd.Transaction = (SQLiteTransaction)trx;
            }

            cmd.CommandTimeout = 0;
            cmd.CommandType = cmdType;

            if (paramArray != null)
            {
                foreach (IDataParameter param in paramArray)
                {
                    cmd.Parameters.Add(param);
                }
            }

            //if (_connection.State == ConnectionState.Open)
            //    _connection.Close();

            //_connection.Open();
            //oRc = cmd.ExecuteScalar();
            //_connection.Close();

            try
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();

                if (connection == null)
                    _connection.Open();

                oRc = cmd.ExecuteScalar();
            }
            catch (SQLiteException)
            {
                throw;
            }
            finally
            {
                col = cmd.Parameters;

                if (connection == null)
                    _connection.Close();
            }

            col = cmd.Parameters;

            return oRc;
        }

        public object ExecuteScalar(string strQuery, IDataParameter[] paramArray,
        CommandType cmdType)
        {
            IDataParameterCollection col = null;
            return ExecuteScalar(null, null, strQuery, paramArray, cmdType, out col);
        }

        public object ExecuteScalar(IDbConnection connection, IDbTransaction trx, string strQuery, IDataParameter[] paramArray)
        {
            IDataParameterCollection col = null;
            return ExecuteScalar(connection, trx, strQuery, paramArray, CommandType.Text, out col);
        }

        public object ExecuteScalar(string strQuery, IDataParameter[] paramArray)
        {
            IDataParameterCollection col = null;
            return ExecuteScalar(null, null, strQuery, paramArray, CommandType.Text, out col);
        }

        public object ExecuteScalar(string strQuery)
        {
            IDataParameterCollection col = null;
            return ExecuteScalar(null, null, strQuery, null, CommandType.Text, out col);
        }
    }
}
