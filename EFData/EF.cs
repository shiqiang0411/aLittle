using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFData;

namespace EFData
{
    public class EF
    {
        private DTcmsdb5Entities dbContextWrite = null;
        protected DTcmsdb5Entities DbContextWrite
        {
            get
            {
                if (dbContextWrite == null)
                    dbContextWrite = new DTcmsdb5Entities();
                return dbContextWrite;
            }
        }
        private DTcmsdb5Entities dbContextRead = null;
        protected DTcmsdb5Entities DbContextRead
        {
            get
            {
                if (dbContextRead == null)
                    dbContextRead = new DTcmsdb5Entities();
                return dbContextRead;
            }
        }
    }
}
