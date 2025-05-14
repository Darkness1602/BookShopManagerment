using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferObject;

namespace DataLayer
{
    public class UserDAL : DataProvider
    {
        public List<UserDTO> GetAllUsers()
        {
            List<UserDTO> users = new List<UserDTO>();
            SqlDataReader reader = null;

            try
            {
                reader = MyExecuteReader("uspGetAllUsers", CommandType.StoredProcedure);
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["UserID"]);
                    string name = reader["UserName"].ToString();
                    string phone = reader["UserPhone"].ToString();
                    string address = reader["UserAddress"].ToString();
                    string password = reader["Password"].ToString();
                    string role = reader["Role"].ToString();

                    users.Add(new UserDTO(id, name, phone, address, password, role));
                }
            }
            finally
            {
                if (reader != null && !reader.IsClosed) reader.Close();
                DisConnect();
            }

            return users;
        }
        public void AddUser(UserDTO user)
        {
            string sql = "uspAddUser";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Username", user.UserName));
            parameters.Add(new SqlParameter("@UserPhone", user.UserPhone));
            parameters.Add(new SqlParameter("@UserAddress", user.UserAddress));
            parameters.Add(new SqlParameter("@Password", user.Password));
            parameters.Add(new SqlParameter("@Role", user.Role));
            try
            {
                MyExecuteNonQuery(sql, CommandType.StoredProcedure, parameters);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void UpdateUser(UserDTO user)
        {
            string sql = "uspUpdateUser";
            List<SqlParameter> parameters = new List<SqlParameter>();
            {
                parameters.Add(new SqlParameter("@UserID", user.UserID));
                parameters.Add(new SqlParameter("@UserName", user.UserName));
                parameters.Add(new SqlParameter("@UserPhone", user.UserPhone));
                parameters.Add(new SqlParameter("@UserAddress", user.UserAddress));
                parameters.Add(new SqlParameter("@Password", user.Password));
                parameters.Add(new SqlParameter("@Role", user.Role));
            }
            try
            {
                MyExecuteNonQuery(sql, CommandType.StoredProcedure, parameters);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void DeleteUser(int userID)
        {
            string sql = "uspDeleteUser";
            List<SqlParameter> parameters = new List<SqlParameter>();
            {
                parameters.Add(new SqlParameter("@UserID", userID));
            }
            try
            {
                MyExecuteNonQuery(sql, CommandType.StoredProcedure, parameters);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public UserDTO CheckLogin(string username, string password)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@UserName", username));
            parameters.Add(new SqlParameter("@Password", password));

            SqlDataReader reader = null;
            try
            {
                reader = MyExecuteReader("uspCheckLogin", CommandType.StoredProcedure, parameters);
                if (reader.Read())
                {
                    int id = Convert.ToInt32(reader["UserID"]);
                    string phone = reader["UserPhone"].ToString();
                    string address = reader["UserAddress"].ToString();
                    string role = reader["Role"].ToString();

                    return new UserDTO(id, username, phone, address, password, role);
                }
                return null;
            }
            finally
            {
                if (reader != null && !reader.IsClosed) reader.Close();
                DisConnect();
            }
        }

        public int GetTotalUsers()
        {
            string sql = "SELECT COUNT(*) FROM UsersTbl";  // Câu truy vấn lấy tổng số người dùng từ bảng UsersTbl
            try
            {
                object result = MyExecuteScalar(sql, CommandType.Text);  // Gọi MyExecuteScalar để lấy kết quả
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);  // Chuyển kết quả sang int
                }
                else
                {
                    throw new Exception("Không thể lấy tổng số người dùng.");
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Lỗi khi lấy tổng số người dùng: " + ex.Message, ex);
            }
        }
    }
}
