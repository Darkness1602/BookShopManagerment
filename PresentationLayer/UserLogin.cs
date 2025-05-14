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
    public partial class UserLogin : Form
    {
        private UserBLL userBLL;
        public UserLogin()
        {
            InitializeComponent();
            userBLL = new UserBLL();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text.Trim();

                UserDTO user = userBLL.AuthenticateUser(username, password);

                if (user.Role == "User")
                {
                    Billing Obj = new Billing();
                    Obj.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Tài khoản không hợp lệ cho người dùng thường.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void lblUser_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AdminLogin Obj = new AdminLogin();
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

        private void UserLogin_Load(object sender, EventArgs e)
        {
            this.AcceptButton = btnLogin;

        }
    }
}
