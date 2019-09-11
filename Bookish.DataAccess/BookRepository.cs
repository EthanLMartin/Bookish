using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookish.DataAccess
{
    public class BookRepository
    {
        public List<Book> GetAllBooks()
        {
            string SqlString = "SELECT * FROM [Books]";

            using (System.Data.IDbConnection db = 
                new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                return (List<Book>)db.Query<Book>(SqlString);
            }
        }
    }
}
