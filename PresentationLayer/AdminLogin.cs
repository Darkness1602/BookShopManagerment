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
using TransferObject;

namespace PresentationLayer
{
    public partial class AdminLogin : Form
    {
        public AdminLogin()
        {
            InitializeComponent();
            userBLL = new UserBLL();
        }
        private UserBLL userBLL;
        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text.Trim();

                UserDTO user = userBLL.AuthenticateUser(username, password);

                if (user.Role == "Admin")
                {
                    Book Obj = new Book();
                    Obj.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Tài khoản không hợp lệ cho quản trị viên.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void lblUser_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            UserLogin Obj = new UserLogin();
            Obj.Show();
            this.Hide();

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

        private void AdminLogin_Load(object sender, EventArgs e)
        {
            this.AcceptButton = btnLogin;

        }
    }
}
