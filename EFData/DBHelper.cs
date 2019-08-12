using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using CommonTools;

namespace EFData
{
    public class DBHelper : EF
    {
        public dt_users GetUser(LoginRequestVO vo)
        {
            var data = DbContextRead.dt_users.Where(i => i.user_name.Equals(vo.UserName)).FirstOrDefault();
            if (null != data)
            {
                string key = data.salt;
                string password = EncryptHelper.Encrypt(vo.Password, key);
                if (data.password != password)
                {
                    return null;
                }
            }
            return data;
        }
    }
}
