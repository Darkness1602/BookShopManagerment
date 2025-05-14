using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using BusinessLayer;
using DataLayer;
using TransferObject;

namespace PresentationLayer
{
    public partial class Customers : Form
    {
        private CustomerBLL customerBLL;
        public Customers()
        {
            InitializeComponent();
            customerBLL = new CustomerBLL();
        }

        private void Customers_Load(object sender, EventArgs e)
        {
            try
            {
                // Tải danh sách người dùng từ lớp UserBLL
                dgvCustomerDetail.DataSource = customerBLL.GetCustomerDTOs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách người dùng: " + ex.Message);
            }
            dgvCustomerDetail.ReadOnly = true;
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
        private void LoadCustomer()
        {
            dgvCustomerDetail.DataSource = customerBLL.GetCustomerDTOs();
            dgvCustomerDetail.ReadOnly = true;
        }

        private void ClearFields()
        {
            txtCusname.Clear();
            txtPhone.Clear();
            txtTotalPoints.Clear();
            txtUsePoints.Clear();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCusname.Text) ||
            string.IsNullOrWhiteSpace(txtPhone.Text) ||
            string.IsNullOrWhiteSpace(txtTotalPoints.Text) ||
            string.IsNullOrWhiteSpace(txtUsePoints.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!txtPhone.Text.All(char.IsDigit))
            {
                MessageBox.Show("Số điện thoại không hợp lệ!");
                return;
            }
            if (!int.TryParse(txtTotalPoints.Text, out _) || !int.TryParse(txtUsePoints.Text, out _))
            {
                MessageBox.Show("TotalPoints và UsedPoints phải là số nguyên.", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var customer = new CustomerDTO(
                txtCusname.Text.Trim(),
                txtPhone.Text.Trim(),
                int.Parse(txtTotalPoints.Text.Trim()),
                int.Parse(txtUsePoints.Text.Trim())
            );

            try
            {
                customerBLL.AddCustomer(customer);
                MessageBox.Show("Thêm khách hàng thành công!");
                LoadCustomer();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCusname.Text) ||
string.IsNullOrWhiteSpace(txtPhone.Text) ||
string.IsNullOrWhiteSpace(txtTotalPoints.Text) ||
string.IsNullOrWhiteSpace(txtUsePoints.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!txtPhone.Text.All(char.IsDigit))
            {
                MessageBox.Show("Số điện thoại không hợp lệ!");
                return;
            }
            if (!int.TryParse(txtTotalPoints.Text, out _) || !int.TryParse(txtUsePoints.Text, out _))
            {
                MessageBox.Show("TotalPoints và UsedPoints phải là số nguyên.", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dgvCustomerDetail.CurrentRow == null) return;

            int id = Convert.ToInt32(dgvCustomerDetail.CurrentRow.Cells["CustomerID"].Value);
            var customer = new CustomerDTO(
                id,
                txtCusname.Text.Trim(),
                txtPhone.Text.Trim(),
                int.Parse(txtTotalPoints.Text.Trim()),
                int.Parse(txtUsePoints.Text.Trim())
            );

            try
            {
                customerBLL.UpdateCustomer(customer);
                MessageBox.Show("Cập nhật thành công!");
                LoadCustomer();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCusname.Text) ||
string.IsNullOrWhiteSpace(txtPhone.Text) ||
string.IsNullOrWhiteSpace(txtTotalPoints.Text) ||
string.IsNullOrWhiteSpace(txtUsePoints.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!txtPhone.Text.All(char.IsDigit))
            {
                MessageBox.Show("Số điện thoại không hợp lệ!");
                return;
            }
            if (!int.TryParse(txtTotalPoints.Text, out _) || !int.TryParse(txtUsePoints.Text, out _))
            {
                MessageBox.Show("TotalPoints và UsedPoints phải là số nguyên.", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dgvCustomerDetail.CurrentRow == null) return;

            int id = Convert.ToInt32(dgvCustomerDetail.CurrentRow.Cells["CustomerID"].Value);

            var confirm = MessageBox.Show("Xác nhận xóa khách hàng?", "Xác nhận", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    customerBLL.DeleteCustomer(id);
                    MessageBox.Show("Xóa thành công!");
                    LoadCustomer();
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private void dgvCustomerDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var selected = dgvCustomerDetail.Rows[e.RowIndex].DataBoundItem as CustomerDTO;
            if (selected != null)
            {
                txtCusname.Text = selected.Name;
                txtPhone.Text = selected.Phone;
                txtTotalPoints.Text = selected.TotalPoints.ToString();
                txtUsePoints.Text = selected.UsedPoints.ToString();
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void btnBilling_Click(object sender, EventArgs e)
        {
            Billing Obj = new Billing();
            this.Hide();
            Obj.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            UserLogin Obj = new UserLogin();
            this.Hide();
            Obj.Show();
        }
    }
}
