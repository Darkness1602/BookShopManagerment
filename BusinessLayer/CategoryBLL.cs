using DataLayer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferObject;

namespace BussinessLayer
{
    public class CategoryBLL
    {

        private CategoryDAL categoryDAL;

        public CategoryBLL()
        {
            categoryDAL = new CategoryDAL();
        }

        public List<CategoryDTO> GetAllCategories()
        {
            return categoryDAL.GetCategories();
        }
    }
} 

