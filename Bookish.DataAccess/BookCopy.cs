using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookish.DataAccess
{
    public class BookCopy
    {
        public int Barcode { get; private set; }
        public string ISBN { get; private set; }
        public bool Borrowed { get; private set; }
        public Loan Loan { get; set; } = null;
    }
}
