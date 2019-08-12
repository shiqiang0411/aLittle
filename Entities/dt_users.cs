using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    public partial class dt_users
    {
        public int id { get; set; }
        public Nullable<int> site_id { get; set; }
        public Nullable<int> group_id { get; set; }
        public string user_name { get; set; }
        public string salt { get; set; }
        public string password { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string avatar { get; set; }
        public string nick_name { get; set; }
        public string sex { get; set; }
        public Nullable<System.DateTime> birthday { get; set; }
        public string telphone { get; set; }
        public string area { get; set; }
        public string address { get; set; }
        public string qq { get; set; }
        public string msn { get; set; }
        public Nullable<decimal> amount { get; set; }
        public Nullable<int> point { get; set; }
        public Nullable<int> exp { get; set; }
        public Nullable<byte> status { get; set; }
        public Nullable<System.DateTime> reg_time { get; set; }
        public string reg_ip { get; set; }
    }
}
