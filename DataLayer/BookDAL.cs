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
    public class BookDAL : DataProvider
    {
            public List<BookDTO> GetBookDTOs()
            {
                string sql = "SELECT * FROM BooksTbl";
                int bookId, quantity;
                string title, author, categoryId;
                decimal price;
                List<BookDTO> books = new List<BookDTO>();
                try
                {
                    Connect();
                    SqlDataReader reader = MyExecuteReader(sql, CommandType.Text);
                    while (reader.Read())
                    {
                        bookId = Convert.ToInt32(reader["BookId"]);
                        title = reader["Title"].ToString();
                        author = reader["Author"].ToString();
                        categoryId = reader["CategoryId"].ToString();
                        quantity = Convert.ToInt32(reader["Quantity"]);
                        price = Convert.ToDecimal(reader["Price"]);
                        BookDTO bookDTO = new BookDTO(bookId, title, author, categoryId, quantity, price);
                        books.Add(bookDTO);
                    }
                    reader.Close();
                    return books;
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }
        
        public void AddBook(BookDTO bookDTO)
        {
            string sql = "uspAddBook";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Title", bookDTO.Title));
            parameters.Add(new SqlParameter("@Author", bookDTO.Author));
            parameters.Add(new SqlParameter("@CategoryId", bookDTO.CategoryId));
            parameters.Add(new SqlParameter("@Quantity", bookDTO.Quantity));
            parameters.Add(new SqlParameter("@Price", bookDTO.Price));
            try
            {
                MyExecuteNonQuery(sql, CommandType.StoredProcedure, parameters);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public int UpdateBook(BookDTO bookDTO)
        {
            string sql = "uspUpdateBook";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@BookId", bookDTO.BookId));
            parameters.Add(new SqlParameter("@Title", bookDTO.Title));
            parameters.Add(new SqlParameter("@Author", bookDTO.Author));
            parameters.Add(new SqlParameter("@CategoryId", bookDTO.CategoryId));
            parameters.Add(new SqlParameter("@Quantity", bookDTO.Quantity));
            parameters.Add(new SqlParameter("@Price", bookDTO.Price));

            try
            {
                return MyExecuteNonQuery(sql, CommandType.StoredProcedure, parameters);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        // Xóa một cuốn sách theo BookId
        public int DeleteBook(int bookId)
        {
            string sql = "uspDeleteBook";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@BookId", bookId));
            try
            {
                return MyExecuteNonQuery(sql, CommandType.StoredProcedure, parameters);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public int UpdateBookQuantity(int bookId, int newQuantity)
        {
            string sql = "uspUpdateBookQuantity";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@BookId", bookId));
            parameters.Add(new SqlParameter("@NewQuantity", newQuantity));
            try
            {
                return MyExecuteNonQuery(sql, CommandType.StoredProcedure, parameters);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public int GetStockByBookId(int bookId)
        {
            string sql = "SELECT Quantity FROM BooksTbl WHERE BookId = @BookId";
            SqlParameter param = new SqlParameter("@BookId", bookId);
            object result = MyExecuteScalar(sql, CommandType.Text, new List<SqlParameter> { param });

            return result != null ? Convert.ToInt32(result) : 0;
        }
        public int GetTotalBooks()
        {
            string sql = "uspGetTotalBooks";
            try
            {
                object result = MyExecuteScalar(sql, CommandType.StoredProcedure);
                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
                else
                {
                    throw new Exception("Unable to retrieve total books.");
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Error getting total books: " + ex.Message, ex);
            }
        }

        public List<BookDTO> GetCategory()
        {
            throw new NotImplementedException();
        }
    }
}
