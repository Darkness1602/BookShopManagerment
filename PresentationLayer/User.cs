using BusinessLayer;
using DataLayer;
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
    public partial class User : Form
    {
        private UserBLL userBLL;
        public User()
        {
            InitializeComponent();
            userBLL = new UserBLL();
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

        private void LoadUser()
        {
            try
            {
                // Tải danh sách người dùng từ lớp UserBLL
                dgvUserDetails.DataSource = userBLL.GetAllUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách người dùng: " + ex.Message);
            }
            dgvUserDetails.ReadOnly = true;
        }

        private void clear()
        {
            txtAddress.Clear();
            txtPassword.Clear();
            txtPhone.Clear();
            txtUsername.Clear();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtPhone.Text) && !txtPhone.Text.All(char.IsDigit))
            {
                MessageBox.Show("Số điện thoại chỉ được phép nhập số!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Lấy dữ liệu từ các ô nhập
            string username = txtUsername.Text.Trim();
            string userphone = txtPhone.Text.Trim();
            string useradress = txtAddress.Text.Trim();
            string password = txtPassword.Text.Trim();
            string role = "User"; // Mặc định vai trò là User

            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userphone) ||
                string.IsNullOrEmpty(useradress) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tạo đối tượng UserDTO
            UserDTO user = new UserDTO(username, userphone, useradress, password, role);

            try
            {
                // Gọi lớp nghiệp vụ để thêm người dùng, truyền vai trò là "Admin" để bypass kiểm tra (nếu đang test)
                userBLL.AddUser(user);

                MessageBox.Show("Thêm người dùng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Làm mới danh sách và các ô nhập
                LoadUser();
                clear();
            }
            catch (UnauthorizedAccessException uaex)
            {
                MessageBox.Show("Bạn không có quyền thêm người dùng. " + uaex.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm người dùng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBook_Click(object sender, EventArgs e)
        {
            Book Obj = new Book();
            this.Hide();
            Obj.Show();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            Dashboard Obj = new Dashboard();
            this.Hide();
            Obj.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            UserLogin Obj = new UserLogin();
            this.Hide();
            Obj.Show();
        }

        private void User_Load(object sender, EventArgs e)
        {
            LoadUser();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtPhone.Text) && !txtPhone.Text.All(char.IsDigit))
            {
                MessageBox.Show("Số điện thoại chỉ được phép nhập số!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dgvUserDetails.CurrentRow == null || dgvUserDetails.CurrentRow.Index < 0)
            {
                MessageBox.Show("Vui lòng chọn người dùng để sửa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy UserID từ dòng được chọn
            int userId = Convert.ToInt32(dgvUserDetails.CurrentRow.Cells["UserID"].Value);

            // Lấy dữ liệu nhập mới
            string username = txtUsername.Text.Trim();
            string userphone = txtPhone.Text.Trim();
            string useraddress = txtAddress.Text.Trim();
            string password = txtPassword.Text.Trim();
            string role = dgvUserDetails.CurrentRow.Cells["Role"].Value.ToString(); // Giữ nguyên vai trò

            // Kiểm tra đầu vào
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userphone) || string.IsNullOrEmpty(useraddress) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin người dùng.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tạo đối tượng UserDTO với đầy đủ thông tin
            UserDTO updatedUser = new UserDTO(userId, username, userphone, useraddress, password, role);

            try
            {
                userBLL.UpdateUser(updatedUser); // Thay bằng vai trò thật nếu có đăng nhập
                MessageBox.Show("Cập nhật người dùng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadUser();  // Làm mới bảng
                clear();     // Xóa ô nhập
            }
            catch (UnauthorizedAccessException uaex)
            {
                MessageBox.Show("Bạn không có quyền cập nhật người dùng. " + uaex.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật người dùng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void dgvUserDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Lấy thông tin từ dòng được chọn
                var selectedUser = dgvUserDetails.Rows[e.RowIndex].DataBoundItem as UserDTO;

                // Điền thông tin sách vào các trường nhập liệu
                if (selectedUser != null)
                {
                    txtUsername.Text = selectedUser.UserName;
                    txtPassword.Text = selectedUser.Password;
                    txtAddress.Text = selectedUser.UserAddress;
                    txtPhone.Text = selectedUser.UserPhone;
                }
            
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvUserDetails.CurrentRow == null || dgvUserDetails.CurrentRow.Index < 0)
            {
                MessageBox.Show("Vui lòng chọn người dùng cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy UserID của người dùng được chọn
            int userId = Convert.ToInt32(dgvUserDetails.CurrentRow.Cells["UserID"].Value);

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa người dùng này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    userBLL.DeleteUser(userId); // Thay "Admin" bằng vai trò thực tế nếu có hệ thống đăng nhập
                    MessageBox.Show("Xóa người dùng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUser(); // Làm mới bảng
                    clear();    // Xóa nội dung ô nhập
                }
                catch (UnauthorizedAccessException uaex)
                {
                    MessageBox.Show("Bạn không có quyền xóa người dùng. " + uaex.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa người dùng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtAddress.Clear();
            txtPhone.Clear();
        }
    }
}
