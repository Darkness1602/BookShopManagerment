using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferObject;


namespace BusinessLayer
{
    public class BillBLL
    {
        private BillDAL billDAL;
        private BillItemDAL billItemDAL;

        public BillBLL()
        {
            billDAL = new BillDAL();
            billItemDAL = new BillItemDAL();
        }

        public void AddBill(BillDTO billDTO)
        {
            CheckBill(billDTO);

            try
            {
                billDAL.AddBill(billDTO);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
   
        public decimal GetTotalAmount()
        {
            try
            {
                return billDAL.GetTotalAmount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CheckBill(BillDTO billDTO)
        {
            try
            {
                if (billDTO.UserID == 0 || string.IsNullOrWhiteSpace(billDTO.ClientName))
                    throw new Exception("UserID và ClientName không được để trống!");
                if (billDTO.Amount < 0)
                    throw new Exception("Số tiền phải không âm!");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddBillDetail(string clientName, string phone, decimal amount)
        {
            billDAL.AddBillDetail(clientName, phone, amount);
        }

        public int AddBillWithItems(BillDTO bill, List<BillItemDTO> items)
        {
            CheckBill(bill);

            if (items == null || items.Count == 0)
                throw new Exception("Không thể tạo hóa đơn nếu không có mặt hàng.");

            try
            {
                int billId = billDAL.AddBill(bill); // phải return BillId từ stored procedure

                foreach (var item in items)
                {
                    item.BillId = billId;
                    billItemDAL.AddBillItem(item);
                }

                return billId;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm hóa đơn và chi tiết: " + ex.Message);
            }
        }
        
    }
}
