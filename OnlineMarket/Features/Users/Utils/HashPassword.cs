using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace OnlineMarket.Features.Users.Utils;

public class HashPassword
{
    private string salt;
    private string hashed;

    public string GetSalt()
    {
        return this.salt;
    }

    public string GetHashed()
    {
        return this.hashed;
    }
    public HashPassword(string _password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
        var returnSalt = Convert.ToBase64String(salt);

        // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: _password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

        this.hashed = hashed;
        this.salt = returnSalt;
    }

    public static bool isValid(string inputPassword, string UserSalt, string Userhashed)
    {
        byte[] pass = Convert.FromBase64String(UserSalt);
        
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: inputPassword,
            salt: pass,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
        
        Console.WriteLine("!!! hashed=" + hashed);

        return hashed == Userhashed;
    }
}