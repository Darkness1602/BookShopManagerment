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
    public class CustomerBLL
    {
        private CustomerDAL customerDAL;

        public CustomerBLL()
        {
            customerDAL = new CustomerDAL();
        }

        // Tìm khách hàng theo số điện thoại
        public CustomerDTO GetCustomerByPhone(string phone, string name)
        {
            //if (string.IsNullOrEmpty(phone))
            //    throw new ArgumentException("Phone number cannot be empty.", nameof(phone));

            // Gọi DAL để lấy khách hàng theo số điện thoại
            var customer = customerDAL.GetCustomerByPhone(phone, name);

            //if (customer == null)
            //    throw new Exception("Customer not found.");

            return customer;
        }
        public List<CustomerDTO> GetCustomerDTOs()
        {
            try
            {
                return customerDAL.GetCustomerDTOs();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        // Thêm khách hàng mới
        public int AddCustomer(CustomerDTO customer)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer), "Customer data cannot be null.");
            if (string.IsNullOrEmpty(customer.Name))
                throw new ArgumentException("Customer name cannot be empty.", nameof(customer.Name));
            if (string.IsNullOrEmpty(customer.Phone))
                throw new ArgumentException("Phone number cannot be empty.", nameof(customer.Phone));

            // Gọi DAL để thêm khách hàng mới
            return customerDAL.AddCustomer(customer);
        }


        // Cập nhật thông tin khách hàng
        public void UpdateCustomer(CustomerDTO customer)
        {
            customerDAL.UpdateCustomer(customer);
        }

        public void DeleteCustomer(int customerID)
        {
            customerDAL.DeleteCustomer(customerID);
        }

        public int GetTotalCustomers()
        {
            return customerDAL.GetTotalCustomers();
        }
        // Cập nhật điểm tích lũy và điểm đã sử dụng cho khách hàng
        public int UpdatePointsWithUsedPoints(string phone, decimal amountSpent, int pointsToUse)
        {
            if (string.IsNullOrEmpty(phone))
                throw new ArgumentException("Phone number cannot be empty.", nameof(phone));
            if (amountSpent < 0)
                throw new ArgumentException("Amount spent cannot be negative.", nameof(amountSpent));
            if (pointsToUse < 0)
                throw new ArgumentException("Points to use cannot be negative.", nameof(pointsToUse));

            // Gọi DAL để cập nhật điểm cho khách hàng
            return customerDAL.UpdateCustomerPoints(phone, amountSpent, pointsToUse);

        }
        public void AddPoints(string phone, int pointsToAdd)
        {
            customerDAL.AddCustomerPoints(phone, pointsToAdd);
        }

        public void UpdatePoints(string phone, int pointsEarned)
        {
            customerDAL.UpdatePoints(phone, pointsEarned);
        }
    }
}
