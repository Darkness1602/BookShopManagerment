using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferObject
{
    public class BillItemDTO
    {
        public int BillItemId { get; set; }
        public int BillId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public decimal Total => Price * Quantity;

        public BillItemDTO(int billId, int bookId, int quantity, decimal price)
        {
            BillId = billId;
            BookId = bookId;
            Quantity = quantity;
            Price = price;
        }

    } 
}
