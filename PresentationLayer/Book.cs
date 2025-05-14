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
using DataLayer;
using TransferObject;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Net;
using BussinessLayer;


namespace PresentationLayer
{
    public partial class Book : Form
    {
        private BookBLL bookBLL;
        private CategoryBLL categoryBLL;
       
        public Book()
        {
            InitializeComponent();
            bookBLL = new BookBLL();
            categoryBLL = new CategoryBLL();
        }


        private void Book_Load(object sender, EventArgs e)
        {
            try
            {
                LoadBooks();
                CbSelectCategory.DataSource = categoryBLL.GetAllCategories();
                CbSelectCategory.DisplayMember = "CategoryName";
                CbSelectCategory.ValueMember = "CategoryId";
                CbSelectCategory.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh mục: " + ex.Message);
            }
            dgvBooks.ReadOnly = true;
        }
        private void LoadBooks()
        {
            try
            {
                dgvBooks.DataSource = bookBLL.GetBookDTOs();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách sách: " + ex.Message);
            }
        }
        private void btnLogout_Click(object sender, EventArgs e)
        {
            UserLogin Obj = new UserLogin();
            this.Hide();
            Obj.Show();
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            Dashboard Obj = new Dashboard();
            this.Hide();
            Obj.Show();
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            User Obj = new User();
            this.Hide();
            Obj.Show();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtBookTitle.Text) ||
        string.IsNullOrWhiteSpace(txtAuthorName.Text) ||
        string.IsNullOrWhiteSpace(txtQuantity.Text) ||
        string.IsNullOrWhiteSpace(txtPrice.Text) ||
        CbSelectCategory.SelectedIndex == -1 || CbSelectCategory.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin và chọn thể loại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string booktitle, author, categoryId;
            int bookquantity;
            decimal bookprice;

            try
            {
                booktitle = txtBookTitle.Text.Trim();
                author = txtAuthorName.Text.Trim();
                categoryId = CbSelectCategory.SelectedValue.ToString();
                bookquantity = Convert.ToInt32(txtQuantity.Text.Trim());
                bookprice = Convert.ToDecimal(txtPrice.Text.Trim());

                BookDTO book = new BookDTO(booktitle, author, categoryId, bookquantity, bookprice);

                bool numberOfRows = bookBLL.AddBook(book);
                if (numberOfRows)
                {
                    this.DialogResult = DialogResult.OK;
                    clear();
                    MessageBox.Show("Thêm sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    this.DialogResult = DialogResult.Cancel;
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message + "\nInput bị lỗi, kiểm tra lại Quantity hoặc Price" , "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message, "Lỗi cơ sở dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi không xác định", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadBooks();
        }
        public void clear()
        {
            txtAuthorName.Clear();
            txtBookTitle.Clear();
            txtPrice.Clear();
            txtQuantity.Clear();
            CbSelectCategory.SelectedIndex = -1;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra xem các trường có hợp lệ không
                if (string.IsNullOrEmpty(txtBookTitle.Text) || string.IsNullOrEmpty(txtAuthorName.Text) ||
                    string.IsNullOrEmpty(txtPrice.Text) || string.IsNullOrEmpty(txtQuantity.Text) ||
                    CbSelectCategory.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy thông tin sách từ các trường nhập liệu
                int bookId = int.Parse(dgvBooks.SelectedRows[0].Cells["BookId"].Value.ToString());
                string bookTitle = txtBookTitle.Text;
                string authorName = txtAuthorName.Text;
                string categoryId = CbSelectCategory.SelectedValue.ToString();
                int quantity = int.Parse(txtQuantity.Text);
                decimal price = decimal.Parse(txtPrice.Text);

                // Tạo đối tượng BookDTO với thông tin đã cập nhật
                BookDTO updatedBook = new BookDTO(bookTitle, authorName, categoryId, quantity, price)
                {
                    BookId = bookId // Gán lại BookId cho sách cần sửa
                };

                // Gọi Business Logic Layer để cập nhật thông tin sách vào database
                bookBLL.UpdateBook(updatedBook);

                // Hiển thị lại thông tin sách đã được cập nhật trong DataGridView
                MessageBox.Show("Cập nhật sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadBooks();
                // Clear các trường nhập liệu
                clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message+"\nSố lượng hoặc giá không hợp lệ! Vui lòng nhập lại", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var bookDelete = dgvBooks.SelectedRows[0];
            int bookID = Convert.ToInt32(bookDelete.Cells[0].Value);
            // Hiển thị hộp thoại xác nhận
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                try
                {
                    bookBLL.DeleteBook(bookID);
                    MessageBox.Show("Đã xoá thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Không thể xóa vì dữ liệu đã được lưu trước", "cảnh báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                }
            }

        }
    

        private void dgvBooks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Lấy thông tin từ dòng được chọn
                var selectedBook = dgvBooks.Rows[e.RowIndex].DataBoundItem as BookDTO;

                // Điền thông tin sách vào các trường nhập liệu
                if (selectedBook != null)
                {
                    txtBookTitle.Text = selectedBook.Title;
                    txtAuthorName.Text = selectedBook.Author;
                    txtPrice.Text = selectedBook.Price.ToString();
                    txtQuantity.Text = selectedBook.Quantity.ToString();
                    CbSelectCategory.SelectedValue = selectedBook.CategoryId; // Đảm bảo CategoryId được chọn đúng
                }
            }
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
    }
}

