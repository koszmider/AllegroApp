using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace LajtIt.Bll
{
    public class AdminUserHelper
    {
        public Dal.AdminUser IsAuthenticated(string userName, string password)
        {
            Dal.AdminUserHelper auh = new Dal.AdminUserHelper();
            Dal.AdminUser au = auh.IsAuthenticated(userName, GetMD5HashData(password));
            return au;
        }
        public Dal.AdminUser GetUser(string userName)
        {
            Dal.AdminUserHelper auh = new Dal.AdminUserHelper();
            Dal.AdminUser au = auh.GetUser(userName);
            return au;
        }
        public bool ChangePassword(string userName, string passwordOld, string passwordNew)
        {

            Dal.AdminUserHelper auh = new Dal.AdminUserHelper();
            return auh.ChangePassword(userName, GetMD5HashData(passwordOld), GetMD5HashData(passwordNew));
        }

        public static string GetMD5HashData(string data)
        {
            //create new instance of md5
            MD5 md5 = MD5.Create();

            //convert the input text to array of bytes
            byte[] hashData = md5.ComputeHash(Encoding.Default.GetBytes(data));

            //create new instance of StringBuilder to save hashed data
            StringBuilder returnValue = new StringBuilder();

            //loop for each byte and add it to StringBuilder
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }

            // return hexadecimal string
            return returnValue.ToString();

        }

     
    }
}
