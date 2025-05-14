using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLayer;
using DataLayer;
using TransferObject;

namespace PresentationLayer
{
    public partial class Billing : Form
    {
        PrintDocument printDocument = new PrintDocument();
        private BookBLL bookBLL;
        private BookDAL bookDAL;
        private BillBLL billBLL;
        private CustomerBLL customerBLL;
        List<BillItemDTO> billItems = new List<BillItemDTO>();
        public Billing()
        {
            InitializeComponent();
            bookDAL = new BookDAL();
            bookBLL = new BookBLL();
            customerBLL = new CustomerBLL();
            billBLL = new BillBLL();

        }
        int billItemIdCounter = 1; // tự tăng
        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                // Thoát ứng dụng
                Application.Exit();
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            UserLogin Obj = new UserLogin();
            this.Hide();
            Obj.Show();
        }

        private void btnBook_Click(object sender, EventArgs e)
        {
            Book Obj = new Book();
            this.Hide();
            Obj.Show();
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            User user = new User();
            user.Show();
            this.Hide();
        }

        decimal total = 0;
        int pointsUsed = 0;
        int pointsEarned = 0;
        private void btnPrint_Click(object sender, EventArgs e)
        {

            if (dgvBills.Rows.Count <= 0)
            {
                MessageBox.Show("Hóa đơn trống. Vui lòng thêm sách vào hóa đơn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
 
            PrintPreviewDialog preview = new PrintPreviewDialog();
            printDocument.PrintPage += new PrintPageEventHandler(PrintDocument1_PrintPage);
            preview.Document = printDocument;
            preview.ShowDialog();
            foreach (BillItemDTO item in billItems)
            {
                try
                {
                    // Lấy số lượng tồn kho mới từ cơ sở dữ liệu
                    int currentStock = bookDAL.GetStockByBookId(item.BookId);
                    int newQuantity = currentStock - item.Quantity;
                    bookDAL.UpdateBookQuantity(item.BookId, newQuantity);

                    // Nếu số lượng mới bằng 0 thì xóa sách
                    if (newQuantity == 0)
                    {
                        bookDAL.DeleteBook(item.BookId);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật số lượng cho sách\n" + ex.Message);
                }

            }

            string phone = txtPhone.Text.Trim();
            string name = txtClientName.Text.Trim();
            CustomerDTO customer = customerBLL.GetCustomerByPhone(phone, name);
            if (customer == null)
            {
                customer = new CustomerDTO(name, phone, 0, 0);
                try
                {
                    customerBLL.AddCustomer(customer);
                    customer = customerBLL.GetCustomerByPhone(phone, name);
                    if (customer == null)
                    {
                        MessageBox.Show("Không thể thêm khách hàng mới. Vui lòng kiểm tra lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi thêm khách hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                txtPoint.Text = customer.TotalPoints.ToString();
            }
            txtClientName.Clear();
            dgvBills.Rows.Clear();
            txtPhone.Clear();
            LoadBooksToDataGridView();

        }
        private void Billing_Load(object sender, EventArgs e)
        {
            LoadBooksToDataGridView();
            btnBook.Enabled = false;
            btnDashboard.Enabled = false;
            btnUser.Enabled = false;
            dgvBook.ReadOnly = true;
            dgvBills.ReadOnly = true;
        }
        private void LoadBooksToDataGridView()
        {
            try
            {
                List<BookDTO> books = bookDAL.GetBookDTOs();
                dgvBook.DataSource = books;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách sách: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvBook_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvBook.Rows[e.RowIndex];
                txtBookTitle.Text = row.Cells["Title"].Value.ToString();
                txtPrice.Text = row.Cells["Price"].Value.ToString();
            }
        }

        private void btnAddToBill_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtClientName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng trước khi thêm vào hóa đơn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvBook.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn sách từ danh sách.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtQuantity.Text) || string.IsNullOrWhiteSpace(txtClientName.Text) || string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ số lượng, tên và SĐT khách hàng.");
                return;
            }
            try
            {
                int quantity = Convert.ToInt32(txtQuantity.Text);
                decimal price = Convert.ToDecimal(txtPrice.Text);
                int bookId = Convert.ToInt32(dgvBook.CurrentRow.Cells["BookId"].Value);
                int stock = Convert.ToInt32(dgvBook.CurrentRow.Cells["Quantity"].Value);

                if (quantity > stock)
                {
                    MessageBox.Show("Không đủ sách trong kho. Vui lòng nhập số lượng nhỏ hơn hoặc bằng " + stock);
                    return;
                }

                // Tạo chi tiết hóa đơn
                BillItemDTO item = new BillItemDTO(billItemIdCounter, bookId, quantity, price);              
                billItems.Add(item);

                dgvBills.Rows.Add(billItemIdCounter, txtBookTitle.Text, quantity, price, item.Total);
                billItemIdCounter++;

                // Trừ số lượng tồn kho
                dgvBook.CurrentRow.Cells["Quantity"].Value = stock - quantity;

                // Cập nhật điểm cho khách hàng nếu cần
                string phone = txtPhone.Text.Trim();
                string name = txtClientName.Text.Trim();
                CustomerDTO customer = customerBLL.GetCustomerByPhone(phone, name);
            }
            catch (FormatException)
            {
                MessageBox.Show("Số lượng hoặc giá không hợp lệ.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
            }
            clear();

        }
        private void clear()
        {
            txtBookTitle.Clear();
            txtPoint.Clear();
            txtPrice.Clear();
            txtQuantity.Clear();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtClientName.Clear();
            clear();
            dgvBills.ClearSelection();
            dgvBills.Rows.Clear();
            txtPhone.Clear();
            LoadBooksToDataGridView();
        }

        private void PrintDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {

            
            Font titleFont = new Font("Times New Roman", 16, FontStyle.Bold);
            Font headerFont = new Font("Times New Roman", 14, FontStyle.Bold);
            Font bodyFont = new Font("Times New Roman", 12);

            float y = e.MarginBounds.Top;
            float x = e.MarginBounds.Left;

            // Header and title
            string title = "HÓA ĐƠN BÁN SÁCH";
            SizeF titleSize = e.Graphics.MeasureString(title, titleFont);
            float titleX = e.MarginBounds.Left + (e.MarginBounds.Width - titleSize.Width) / 2;
            e.Graphics.DrawString(title, titleFont, Brushes.Black, titleX, y);
            y += 40;

            string shopName = "BOOKSHOP";
            SizeF shopSize = e.Graphics.MeasureString(shopName, titleFont);
            float shopX = e.MarginBounds.Left + (e.MarginBounds.Width - shopSize.Width) / 2;
            e.Graphics.DrawString(shopName, titleFont, Brushes.Black, shopX, y);
            y += 40;

            // Customer info
            e.Graphics.DrawString($"Ngày: {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}", bodyFont, Brushes.Black, x, y);
            y += 25;
            e.Graphics.DrawString($"Khách hàng: {txtClientName.Text}", bodyFont, Brushes.Black, x, y);
            y += 25;

            // Table header
            y += 10;
            e.Graphics.DrawString("STT", headerFont, Brushes.Black, x, y);
            e.Graphics.DrawString("Tên sách", headerFont, Brushes.Black, x + 80, y);
            e.Graphics.DrawString("Giá", headerFont, Brushes.Black, x + 240, y);
            e.Graphics.DrawString("Số lượng", headerFont, Brushes.Black, x + 320, y);
            e.Graphics.DrawString("Thành tiền", headerFont, Brushes.Black, x + 420, y);
            y += 30;

            // Line separator
            string shopName0 = "________________________________________________________________";
            SizeF shopSize0 = e.Graphics.MeasureString(shopName0, titleFont);
            float shopX0 = e.MarginBounds.Left + (e.MarginBounds.Width - shopSize0.Width) / 2;
            e.Graphics.DrawString(shopName0, titleFont, Brushes.Black, shopX0, y);
            y += 40;

            // Bill details

            int stt = 1;
            foreach (DataGridViewRow row in dgvBills.Rows)
            {
                if (row.IsNewRow) continue;

                string bookName = row.Cells["Column2"].Value.ToString(); // sửa tên biến
                string price = Convert.ToDecimal(row.Cells["Column3"].Value).ToString("N0");
                string quantity = row.Cells["Column5"].Value.ToString();
                decimal itemTotal = Convert.ToDecimal(row.Cells["Column4"].Value);
                total += itemTotal;

                e.Graphics.DrawString(stt.ToString(), bodyFont, Brushes.Black, x, y);
                e.Graphics.DrawString(bookName, bodyFont, Brushes.Black, x + 80, y);
                e.Graphics.DrawString(price, bodyFont, Brushes.Black, x + 240, y);
                e.Graphics.DrawString(quantity, bodyFont, Brushes.Black, x + 320, y);
                e.Graphics.DrawString(itemTotal.ToString("N0"), bodyFont, Brushes.Black, x + 420, y);

                y += 25;
                stt++;
            }

            // Line separator
            string shopName1 = "----------------------------------------------------";
            SizeF shopSize1 = e.Graphics.MeasureString(shopName1, titleFont);
            float shopX1 = e.MarginBounds.Left + (e.MarginBounds.Width - shopSize1.Width) / 2;
            e.Graphics.DrawString(shopName1, titleFont, Brushes.Black, shopX1, y);
            y += 40;

            // Points logic for discount
            string phone = txtPhone.Text.Trim();
            string name = txtClientName.Text.Trim();
            CustomerDTO customer = customerBLL.GetCustomerByPhone(phone, name);



            if (customer != null)
            {
                if (customer.TotalPoints <= 0)
                {
                    ckUP.Checked = false; // Không có điểm thì không cho dùng
                }

                if (ckUP.Checked) // Nếu người dùng chọn sử dụng điểm
                {
                    pointsUsed = customer.TotalPoints;

                    if (pointsUsed > total)
                    {
                        pointsUsed = (int)total; // Không dùng quá số tiền
                    }

                    pointsEarned = (int)Math.Floor((total - pointsUsed) / 1000m);
                    customerBLL.UpdatePointsWithUsedPoints(phone, total, pointsUsed);
                }
            }

            e.Graphics.DrawString($"Tổng tiền sách: {(total)} VNĐ", headerFont, Brushes.Black, x, y);
            y += 30;

            // Display points and discount information
            if (pointsUsed > 0)
            {
                e.Graphics.DrawString($"Điểm đã sử dụng: {pointsUsed}", bodyFont, Brushes.Black, x, y);
                y += 25;
            }
            if (ckUP.Checked == true)
            {
                e.Graphics.DrawString($"Điểm tích lũy mới: {(int)pointsEarned}", bodyFont, Brushes.Black, x, y);
                y += 30;
            }
            else
            {
                e.Graphics.DrawString($"Điểm tích lũy mới: {total/1000+customer.TotalPoints}", bodyFont, Brushes.Black, x, y);
                y += 30;
            }
           

            

            // Final amount
            e.Graphics.DrawString($"Tổng tiền: {(total - pointsUsed).ToString("N0")} VNĐ", headerFont, Brushes.Black, x, y);
            y += 40;

            // Thank you message
            string camon = "Cảm ơn quý khách!";
            SizeF camonF = e.Graphics.MeasureString(camon, bodyFont);
            float camonX = e.MarginBounds.Left + (e.MarginBounds.Width - camonF.Width) / 2;
            e.Graphics.DrawString(camon, bodyFont, Brushes.Black, camonX, y);


        }

        private void txtPoint_Leave(object sender, EventArgs e)
        {
            //string phone = txtPhone.Text.Trim();
            //string name = txtClientName.Text.Trim();
            //if (string.IsNullOrEmpty(phone)) return;

            //CustomerDTO customer = customerBLL.GetCustomerByPhone(phone, name);
            //if (customer != null)
            //{
            //    txtClientName.Text = customer.Name;
            //    txtPoint.Text = customer.TotalPoints.ToString();
            //}
            //else
            //{
            //    txtClientName.Clear();
            //    txtPoint.Text = "0";
            //}
        }


        private void ckbP_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbP.Checked == true)
            {
                string phone = txtPhone.Text.Trim();
                string name = txtClientName.Text.Trim();

                // Kiểm tra nếu thiếu thông tin
                if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(name))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ 'Client Name' và 'Phone' trước khi kiểm tra điểm!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ckbP.Checked = false; // Bỏ check nếu thiếu
                    return;
                }

                CustomerDTO customer = customerBLL.GetCustomerByPhone(phone, name);
                if (customer != null)
                {
                    txtClientName.Text = customer.Name;
                    txtPoint.Tag = customer.TotalPoints;
                    txtPoint.Text = customer.TotalPoints.ToString();
                }
                else
                {
                    txtClientName.Clear();
                    txtPoint.Text = "0";
                    txtPoint.Tag = 0;
                }
            }
            else
            {
                txtPoint.Clear();
                txtPoint.Tag = null;
            }
        }

        private void ckUP_CheckedChanged(object sender, EventArgs e)
        {
            string phone = txtPhone.Text.Trim();
            string name = txtClientName.Text.Trim();

            // Kiểm tra nếu thiếu thông tin
            if (string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ 'Client Name' và 'Phone' trước khi kiểm tra điểm!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ckUP.Checked = false; // Bỏ check nếu thiếu
                return;
            }
            int pointToUse = 0;
            if (ckUP.Checked)
            {
                if (int.TryParse(txtPoint.Text.Trim(), out int availablePoints) && availablePoints > 0)
                {
                    pointToUse = availablePoints;
                }
                else
                {
                    MessageBox.Show("Khách hàng không có điểm để sử dụng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ckUP.Checked = false;
                    return;
                }
            }
        }
        private void txtPhone_Leave(object sender, EventArgs e)
        {
            string phone = txtPhone.Text.Trim();
            if (string.IsNullOrEmpty(phone)) return;

            CustomerDTO customer = customerBLL.GetCustomerByPhone(phone,Name);
            if (customer != null)
            {
                // Nếu số điện thoại tồn tại, tự động điền tên
                txtClientName.Text = customer.Name;
                txtPoint.Text = customer.TotalPoints.ToString();
            }
            else
            {
                // Nếu không tồn tại, để trống tên và điểm để người dùng nhập
                txtClientName.Clear();
                txtPoint.Text = "0";
            }
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            Customers Obj = new Customers();
            this.Hide();
            Obj.Show();
        }
    }
}
