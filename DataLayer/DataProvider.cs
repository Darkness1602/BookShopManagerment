using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class DataProvider
    {
        private SqlConnection cn;

        public DataProvider()
        {
            string cnStr = @"Data Source=DARKNESS;Initial Catalog=BOOKSHOP;Integrated Security=True";
            cn = new SqlConnection(cnStr);
        }

        public void Connect()
        {
            try
            {
                if (cn != null && cn.State == ConnectionState.Closed)
                    cn.Open();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void DisConnect()
        {
            try
            {
                if (cn != null && cn.State == ConnectionState.Open)
                    cn.Close();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        // ExecuteScalar có hỗ trợ SqlParameter
        public object MyExecuteScalar(string sql, CommandType type, List<SqlParameter> parameters = null)
        {
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.CommandType = type;

                if (parameters != null)
                    cmd.Parameters.AddRange(parameters.ToArray());

                return cmd.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                DisConnect();
            }
        }
        public SqlDataReader MyExecuteReader(string sql, CommandType type, List<SqlParameter> parameters = null)
        {
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.CommandType = type;

                if (parameters != null)
                    cmd.Parameters.AddRange(parameters.ToArray());

                return cmd.ExecuteReader(); // nhớ reader.Close() và DisConnect() bên ngoài
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        // ExecuteNonQuery đã có sẵn tham số
        public int MyExecuteNonQuery(string sql, CommandType type, List<SqlParameter> parameters = null)
        {
            try
            {
                Connect();
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.CommandType = type;

                if (parameters != null)
                    cmd.Parameters.AddRange(parameters.ToArray());

                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DisConnect();
            }
        }
    }
}
