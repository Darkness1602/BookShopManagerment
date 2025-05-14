using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransferObject
{
    public class CategoryDTO
    {
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }

        public CategoryDTO(string categoryId, string categoryName)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
        }

        //public override string ToString()
        //{
        //    return CategoryName; // Để hiển thị tên trong ComboBox
        //}
    }
}
