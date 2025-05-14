using DataLayer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferObject;

namespace BusinessLayer
{
    public class BookBLL
    {
        private BookDAL bookDAL;
  

        public BookBLL()
        {
            bookDAL = new BookDAL();
        }

        public List<BookDTO> GetBookDTOs()
        {
            try
            {
                return bookDAL.GetBookDTOs();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public bool AddBook(BookDTO book)
        {
            if (string.IsNullOrEmpty(book.Title) || string.IsNullOrEmpty(book.Author) ||
                string.IsNullOrEmpty(book.CategoryId) ||
                book.Quantity < 0 || book.Price < 0)
            {
                return false;
            }
            try
            {
                bookDAL.AddBook(book);
                return true;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void UpdateBook(BookDTO book)
        {
            if (book.BookId <= 0 ||
                string.IsNullOrEmpty(book.Title) ||
                string.IsNullOrEmpty(book.Author) ||
                string.IsNullOrEmpty(book.CategoryId) ||
                book.Quantity < 0 || book.Price < 0)
            {
                throw new ArgumentException("Thông tin sách không hợp lệ! Vui lòng kiểm tra lại các trường.");
            }

            try
            {
                bookDAL.UpdateBook(book);
            }
            catch (SqlException ex)
            {
                // Giữ lại thông tin lỗi ban đầu và không làm mất stack trace
                throw new Exception("Lỗi khi cập nhật sách vào cơ sở dữ liệu.", ex);
            }
        }

        public void DeleteBook(int bookId)
        {
            if (bookId <= 0)
                throw new Exception("Invalid book ID.");

            try
            {
                bookDAL.DeleteBook(bookId);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public int GetTotalBooks()
        {
            try
            {
                return bookDAL.GetTotalBooks();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public void UpdateBookQuantity(int bookId, int quantity, int stock)
        {
            if (quantity <= 0 || quantity > stock)
                throw new Exception("Invalid quantity or insufficient stock.");

            try
            {
                int newQuantity = stock - quantity;
                bookDAL.UpdateBookQuantity(bookId, newQuantity);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

    }
}
