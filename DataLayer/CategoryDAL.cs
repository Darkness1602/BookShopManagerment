using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferObject;


namespace DataLayer
{
    public class CategoryDAL:DataProvider
    {
        public List<CategoryDTO> GetCategories()
        {
            
            string sql = "SELECT CategoryId, CategoryName FROM CategoriesTbl";
            List<CategoryDTO> categories = new List<CategoryDTO>();

            try
            {
                Connect();
                SqlDataReader reader = MyExecuteReader(sql, CommandType.Text);
                while (reader.Read())
                {
                    string id = reader["CategoryId"].ToString();
                    string name = reader["CategoryName"].ToString();
                    categories.Add(new CategoryDTO(id, name));
                }
                reader.Close();
                return categories;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
