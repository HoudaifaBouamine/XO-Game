﻿using System.Text;
using System.Security.Cryptography;

namespace xo.Api.NewFolder
{
    public class SecurityService
    {
        public static bool VerifyPassword(string hashedPassword, string password)
        {
            return hashedPassword == sha256(password);
        }

        public static string HashPassword(string password)
        {
            return sha256(password);
        }
        private static string sha256(string randomString)
        {
            var crypt = new SHA256Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString("x2");
            }
            return hash;
        }
    }
}


