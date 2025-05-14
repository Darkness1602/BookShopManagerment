using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferObject
{
    public class BookDTO
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string CategoryId { get; set; } // dùng ID
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public BookDTO(string title, string author, string categoryId, int quantity, decimal price)
        {
            Title = title;
            Author = author;
            CategoryId = categoryId;
            Quantity = quantity;
            Price = price;
        }

        public BookDTO(int bookId, string title, string author, string categoryId, int quantity, decimal price)
        {
            BookId = bookId;
            Title = title;
            Author = author;
            CategoryId = categoryId;
            Quantity = quantity;
            Price = price;
        }
    }
}
