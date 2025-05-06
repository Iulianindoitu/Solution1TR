using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace SkillForge
{
     public class LoginRegister
     {
          private string HashGen(string password)
          {
               // Initialize MD5 hash provider
               using (MD5 md5 = MD5.Create())
               {
                    // Combine password with a salt (if needed, replace "SALT" with your actual salt)
                    string saltedPassword = password + "SALT";

                    // Convert the salted password to bytes
                    byte[] inputBytes = Encoding.Default.GetBytes(saltedPassword);

                    // Compute the hash
                    byte[] hashBytes = md5.ComputeHash(inputBytes);

                    // Convert the hash bytes to a hexadecimal string
                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in hashBytes)
                    {
                         sb.Append(b.ToString("x2"));
                    }

                    // Return the hash as a string
                    return sb.ToString();
               }
          }
     }
}