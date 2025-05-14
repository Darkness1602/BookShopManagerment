using BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationLayer
{
    public partial class Dashboard : Form
    {
        private BookBLL bookBLL;
        public Dashboard()
        {
            InitializeComponent();
            bookBLL = new BookBLL();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            UserLogin Obj = new UserLogin();
            this.Hide();
            Obj.Show();
        }
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                // Thoát ứng dụng
                Application.Exit();
            }
        }

        private void btnBook_Click(object sender, EventArgs e)
        {
            Book Obj = new Book();
            this.Hide();
            Obj.Show();
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            User Obj = new User();
            this.Hide();
            Obj.Show();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            ShowTotalBooks();
            ShowTotalAmount();
            ShowTotalUsers();

        }
        private void ShowTotalBooks()
        {
            try
            {
                // Lấy tổng số sách từ BLL
                int totalBooks = bookBLL.GetTotalBooks();

                // Hiển thị tổng số sách lên Label
                lblTotalBooks.Text = "Tổng số sách: " + totalBooks.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy tổng số sách: " + ex.Message);
            }
        }
        private void ShowTotalAmount()
        {
            try
            {
                BillBLL billBLL = new BillBLL();  // Khởi tạo đối tượng BillBLL
                decimal totalAmount = billBLL.GetTotalAmount();  // Lấy tổng số tiền từ BillBLL
                lblTotalAmount.Text = $"Tổng số tiền: {totalAmount:C}";  // Hiển thị tổng số tiền trên Label
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy tổng số tiền: " + ex.Message);
            }
        }

        private void ShowTotalUsers()
        {
            try
            {
                UserBLL userBLL = new UserBLL();  // Khởi tạo đối tượng UserBLL
                int totalUsers = userBLL.GetTotalUsers();  // Lấy tổng số người dùng từ UserBLL
                lblTotalUsers.Text = $"Tổng người dùng: {totalUsers}";  // Hiển thị tổng số người dùng trên Label
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy tổng số người dùng: " + ex.Message);
            }
        }
    }
}
