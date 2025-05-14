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
    public class BillItemDAL : DataProvider
    {
        public void AddBillItem(BillItemDTO item)
        {
            string sql = "INSERT INTO BillItemTbl (BillId, BookId, Quantity, Price) VALUES (@BillId, @BookId, @Quantity, @Price)";
            List<SqlParameter> parameters = new List<SqlParameter>()
        {
            new SqlParameter("@BillId", item.BillId),
            new SqlParameter("@BookId", item.BookId),
            new SqlParameter("@Quantity", item.Quantity),
            new SqlParameter("@Price", item.Price)
        };
            try
            {
                MyExecuteNonQuery(sql, CommandType.Text, parameters);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
