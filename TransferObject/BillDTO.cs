using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferObject
{
    public class BillDTO
    {
        public int BillId { get; set; }
        public int UserID { get; set; }
        public string ClientName { get; set; }
        public decimal Amount { get; set; }

        public string Phone { get; set; }
        public BillDTO(int userID, string clientName, string phone, decimal amount)
        {

            UserID = userID;
            ClientName = clientName;
            Amount = amount;
            Phone = phone;
        }

        public BillDTO(string clientName, string phone, decimal amount)
        {
            ClientName = clientName;
            Amount = amount;
            Phone = phone;
        }

    }
}
