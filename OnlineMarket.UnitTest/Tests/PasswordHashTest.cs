using OnlineMarket.Features.Users.Utils;
using Xunit;

namespace OnlineMarket.UnitTest.Tests;

public class PasswordHashTest
{
    [Fact]
    public void MatchPassword()
    {
        var password = "1234"; 
        HashPassword hashPassword = new HashPassword(password);
        var salt = hashPassword.GetSalt();
        var hashed = hashPassword.GetHashed();
        
        Assert.True(HashPassword.isValid(password, salt, hashed));
    }
    
    [Fact]
    public void MatchPasswordWrong()
    {
        var password = "qwerty"; 
        HashPassword hashPassword = new HashPassword(password);
        var salt = hashPassword.GetSalt();
        var hashed = hashPassword.GetHashed();
        
        Assert.False(HashPassword.isValid(password.ToUpper(), salt, hashed));
    }

    [Fact]
    public void MathPasswordEmpty()
    {
        Assert.Throws<Exception>(() => new HashPassword(""));
    }
}