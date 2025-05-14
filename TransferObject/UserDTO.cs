using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferObject
{
    public class UserDTO
    {
        public int UserID { get; set; }  // Thêm UserID
        public string UserName { get; set; }
        public string UserPhone { get; set; }
        public string UserAddress { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public UserDTO(int userID, string userName, string userPhone, string userAddress, string password, string role)
        {
            UserID = userID;
            UserName = userName;
            UserPhone = userPhone;
            UserAddress = userAddress;
            Password = password;
            Role = role;
        }

        public UserDTO(string userName, string userPhone, string userAddress, string password, string role)
        {
            UserName = userName;
            UserPhone = userPhone;
            UserAddress = userAddress;
            Password = password;
            Role = role;
        }
        public UserDTO(int userID, string password, string role)
        {
            UserID = userID;
            Password = password;
            Role = role;
        }

    }
}
