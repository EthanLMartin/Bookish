using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookish.DataAccess
{
    class BookCopy
    {
        public string Barcode { get; private set; }
        public Loan Loan { get; set; } = null;
    }
}
