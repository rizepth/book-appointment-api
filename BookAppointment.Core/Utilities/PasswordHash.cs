using BookAppointment.Core.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BookAppointment.Core.Utilities
{
    public class PasswordHash : IPasswordHash
    {
        public string Hash(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public bool Verify(string password, string hashedPassword)
        {
            var hashedInputPassword = Hash(password);
            return string.Equals(hashedInputPassword, hashedPassword, StringComparison.OrdinalIgnoreCase);
        }
    }
}
