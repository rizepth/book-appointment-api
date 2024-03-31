using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookAppointment.Core.Interfaces.Common
{
    public interface IPasswordHash
    {
        string Hash(string password);
        bool Verify(string password, string hashedPassword);
    }
}
