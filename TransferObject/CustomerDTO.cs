using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferObject
{
    public class CustomerDTO
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int TotalPoints { get; set; }
        public int UsedPoints { get; set; }

        public CustomerDTO(string name, string phone, int totalPoints, int usedPoints)
        {
            Name = name;
            Phone = phone;
            TotalPoints = totalPoints;
            UsedPoints = usedPoints;
        }
        public CustomerDTO () { }
            public CustomerDTO(int customerID, string name, string phone, int totalPoints, int usedPoints)
        {
            CustomerID = customerID;
            Name = name;
            Phone = phone;
            TotalPoints = totalPoints;
            UsedPoints = usedPoints;
        }
    }
}
