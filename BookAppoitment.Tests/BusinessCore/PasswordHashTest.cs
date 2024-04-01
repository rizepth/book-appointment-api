using BookAppointment.Core.Interfaces.Common;
using BookAppointment.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookAppoitment.Tests.BusinessCore
{
    public class PasswordHashTest
    {
        private readonly IPasswordHash _passwordHash;

        public PasswordHashTest()
        {
            _passwordHash = new PasswordHash();
        }

        [Fact]
        public void HashPassword_ValidPassword_ReturnsHashedPassword()
        {
            // Arrange
            string password = "password123";

            // Act
            string hashedPassword = _passwordHash.Hash(password);

            // Assert
            Assert.NotNull(hashedPassword);
            Assert.NotEqual(password, hashedPassword);
        }

        [Fact]
        public void VerifyPassword_CorrectPassword_ReturnsTrue()
        {
            // Arrange
            string password = "password123";
            string hashedPassword = _passwordHash.Hash(password);

            // Act
            bool isValid = _passwordHash.Verify(password, hashedPassword);

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void VerifyPassword_IncorrectPassword_ReturnsFalse()
        {
            // Arrange
            string password = "password123";
            string incorrectPassword = "wrongpassword";
            string hashedPassword = _passwordHash.Hash(password);

            // Act
            bool isValid = _passwordHash.Verify(incorrectPassword, hashedPassword);

            // Assert
            Assert.False(isValid);
        }
    }
}
