using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferObject;
namespace DataLayer
{
    public class CustomerDAL: DataProvider
    {
        public CustomerDTO GetCustomerByPhone(string phone, string name)
        {
            string sql = "uspGetCustomerByPhone";
            List<SqlParameter> parameters = new List<SqlParameter>
    {
        new SqlParameter("@Phone", SqlDbType.NVarChar) { Value = phone },
        new SqlParameter("@Name", SqlDbType.NVarChar) { Value = name }
    };

            SqlDataReader reader = null;
            CustomerDTO customer = null;

            try
            {
                // Gọi stored procedure để lấy thông tin khách hàng (hoặc thêm mới nếu không có)
                reader = MyExecuteReader(sql, CommandType.StoredProcedure, parameters);

                if (reader.Read())  // Nếu có kết quả trả về từ SQL
                {
                    int customerId = Convert.ToInt32(reader["CustomerId"]);
                    string customerName = reader["Name"].ToString();
                    string phoneNumber = reader["Phone"].ToString();
                    int totalPoints = Convert.ToInt32(reader["TotalPoints"]);
                    int usedPoints = Convert.ToInt32(reader["UsedPoints"]);

                    customer = new CustomerDTO(customerId, name, phoneNumber, totalPoints, usedPoints);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching customer data.", ex);
            }
            finally
            {
                reader?.Close();  // Đảm bảo đóng reader
            }

            return customer;  // Trả về khách hàng tìm được hoặc null nếu không tìm thấy
        }
        //Đọc Customer
        public List<CustomerDTO> GetCustomerDTOs()
        {
            string sql = "SELECT * FROM CustomersTbl";
            List<CustomerDTO> customers = new List<CustomerDTO>();

            try
            {
                Connect(); // Hàm kết nối DB đã có sẵn trong lớp DAL của bạn
                SqlDataReader reader = MyExecuteReader(sql, CommandType.Text);

                while (reader.Read())
                {
                    int customerID = Convert.ToInt32(reader["CustomerID"]);
                    string name = reader["Name"].ToString();
                    string phone = reader["Phone"].ToString();
                    int totalPoints = Convert.ToInt32(reader["TotalPoints"]);
                    int usedPoints = Convert.ToInt32(reader["UsedPoints"]);

                    CustomerDTO customer = new CustomerDTO(customerID, name, phone, totalPoints, usedPoints);
                    customers.Add(customer);
                }

                reader.Close();
                return customers;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        // Thêm khách hàng mới vào hệ thống
        public int AddCustomer(CustomerDTO customer)
        {
            string sql = "uspAddCustomer";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Name", SqlDbType.NVarChar) { Value = customer.Name },
                new SqlParameter("@Phone", SqlDbType.NVarChar) { Value = customer.Phone },
                new SqlParameter("@TotalPoints", SqlDbType.Int) { Value = customer.TotalPoints},
                new SqlParameter("@UsedPoints", SqlDbType.Int) { Value = customer.UsedPoints}
            };

            object result = MyExecuteScalar(sql, CommandType.StoredProcedure, parameters);
            return Convert.ToInt32(result); // Trả về CustomerId mới
        }
        public void UpdateCustomer(CustomerDTO customer)
        {
            string sql = "uspUpdateCustomer";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@CustomerID", customer.CustomerID),
                new SqlParameter("@Name", customer.Name),
                new SqlParameter("@Phone", customer.Phone),
                new SqlParameter("@TotalPoints", customer.TotalPoints),
                new SqlParameter("@UsedPoints", customer.UsedPoints)
            };

            try
            {
                MyExecuteNonQuery(sql, CommandType.StoredProcedure, parameters);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void DeleteCustomer(int customerId)
        {
            string sql = "uspDeleteCustomer";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@CustomerID", customerId)
            };

            try
            {
                MyExecuteNonQuery(sql, CommandType.StoredProcedure, parameters);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public int GetTotalCustomers()
        {
            string sql = "SELECT COUNT(*) FROM CustomerTbl";
            try
            {
                object result = MyExecuteScalar(sql, CommandType.Text);
                return result != null ? Convert.ToInt32(result) : 0;
            }
            catch (SqlException ex)
            {
                throw new Exception("Lỗi khi lấy tổng số khách hàng: " + ex.Message, ex);
            }
        }

        // Cập nhật điểm tích lũy và điểm sử dụng cho khách hàng
        public int UpdateCustomerPoints(string phone, decimal amountSpent, int pointsToUse)
        {
            string sql = "uspUpdateCustomerPoints";
            List<SqlParameter> parameters = new List<SqlParameter>
            { 
                new SqlParameter("@Phone", SqlDbType.NVarChar) { Value = phone },
                new SqlParameter("@AmountSpent", SqlDbType.Decimal) { Value = amountSpent },
                new SqlParameter("@PointsToUse", SqlDbType.Int) { Value = pointsToUse }
            };

            return MyExecuteNonQuery(sql, CommandType.StoredProcedure, parameters);
        }
        public int AddCustomerPoints(string phone, int pointsToAdd)
        {
            string sql = "uspAddPoints";
            List<SqlParameter> parameters = new List<SqlParameter>
    {
        new SqlParameter("@phone", SqlDbType.NVarChar) { Value = phone },
        new SqlParameter("@pointsToAdd", SqlDbType.Int) { Value = pointsToAdd }
    };

            return MyExecuteNonQuery(sql, CommandType.StoredProcedure, parameters);
        }
        public int UpdatePoints(string phone, int pointsEarned)
        {
            string sql = "uspUpdatePointsWithUsedPoints";
            List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@phone", SqlDbType.NVarChar) { Value = phone },
            new SqlParameter("@pointsEarned", SqlDbType.Int) { Value = pointsEarned }
        };

            return MyExecuteNonQuery(sql, CommandType.StoredProcedure, parameters);
        }

    }
}
