using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookish.DataAccess
{
    public class Loan
    {
        public String Barcode { get; private set; }
        public String UserId { get; private set; }
        public DateTime DueDate { get; private set; }
        public bool Completed { get; set; }
    }
}
