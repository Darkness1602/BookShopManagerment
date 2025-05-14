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
    public class BillDAL : DataProvider
    {
        public int AddBill(BillDTO billDTO)
        {
            string sql = "uspAddBill";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@UserID", billDTO.UserID));
            parameters.Add(new SqlParameter("@ClientName", billDTO.ClientName));
            parameters.Add(new SqlParameter("@Amount", billDTO.Amount));
            parameters.Add(new SqlParameter("@Phone", billDTO.Phone));
            try
            {
                // Trả về BillId (SCOPE_IDENTITY())
                return Convert.ToInt32(MyExecuteScalar(sql, CommandType.Text, parameters));
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }


        public decimal GetTotalAmount()
        {
            string sql = "SELECT SUM(Amount) FROM BillsTbl";
            try
            {
                return Convert.ToDecimal(MyExecuteScalar(sql, CommandType.Text));
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void AddBillDetail(string clientName, string phone, decimal amount)
        {
            string sql = "INSERT INTO BillsTbl (ClientName, Phone, Amount) " +
                         "VALUES (@ClientName, @Phone, @Amount)";

            List<SqlParameter> parameters = new List<SqlParameter>()
            {
            new SqlParameter("@ClientName", clientName),
            new SqlParameter("@Phone", phone),
            new SqlParameter("@Amount", amount)
            };

            try
            {
                MyExecuteNonQuery(sql, CommandType.Text, parameters);
            }
            catch (SqlException ex)
            {
                throw new Exception("Lỗi khi thêm chi tiết hóa đơn: " + ex.Message);
            }
        }

        public void UpdateBookQuantity(string bookName, int quantitySold)
        {
            string sql = "UPDATE BooksTbl SET Quantity = Quantity - @QuantitySold WHERE BookName = @BookName";
            List<SqlParameter> parameters = new List<SqlParameter>()
        {
            new SqlParameter("@BookName", bookName),
            new SqlParameter("@QuantitySold", quantitySold)
        };

            try
            {
                MyExecuteNonQuery(sql, CommandType.Text, parameters);
            }
            catch (SqlException ex)
            {
                throw new Exception("Lỗi khi cập nhật số lượng sách: " + ex.Message);
            }
        }

        
    }
}
