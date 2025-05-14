using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferObject;

namespace BusinessLayer
{
    public class UserBLL
    {
        private UserDAL userDAL;
        public UserBLL()
        {
            userDAL = new UserDAL();
        }
        public UserDTO AuthenticateUser(string userName, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
                {
                    throw new Exception("Hãy nhập đầy đủ tên đăng nhập và mật khẩu.");
                }

                UserDTO user = userDAL.CheckLogin(userName, password);

                if (user == null)
                {
                    throw new Exception("Sai tên đăng nhập hoặc mật khẩu.");
                }
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xác thực người dùng: " + ex.Message);
            }
        }
        public List<UserDTO> GetAllUsers()
        {
            try
            {
                return userDAL.GetAllUsers();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách người dùng: " + ex.Message);
            }
        }
        public void AddUser(UserDTO user)
        {
            
            try
            {
                userDAL.AddUser(user);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm người dùng: " + ex.Message);
            }
        }
        public void UpdateUser(UserDTO user)
        {
           
            try
            {
                userDAL.UpdateUser(user);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật người dùng: " + ex.Message);
            }
        }
        public void DeleteUser(int userId)
        {
            try
            {
                userDAL.DeleteUser(userId);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa người dùng: " + ex.Message);
            }
        }

        public int GetTotalUsers()
        {
            try
            {
                return userDAL.GetTotalUsers();  // Gọi phương thức trong DAL để lấy tổng số người dùng
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy tổng số người dùng: " + ex.Message, ex);
            }
        }
    }
}