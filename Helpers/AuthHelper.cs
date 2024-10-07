using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;

namespace DotnetAPI.Helpers
{
    public class AuthHelper
    {
        private readonly IConfiguration _config;
        public AuthHelper(IConfiguration config)
        {
            _config = config;
        }

    public byte[] GetPasswordHash(string password,byte[] passwordSalt)
    {
        string tempPasswordSaltPlusPasswordKey = _config.GetSection( "AppSettings:PasswordKey").Value + Convert.ToBase64String(passwordSalt);
                //now we are going to create actual password hash

                byte[] tempPasswordHash = KeyDerivation.Pbkdf2(
                            password : password,
                            salt : Encoding.ASCII.GetBytes(tempPasswordSaltPlusPasswordKey),
                            prf : KeyDerivationPrf.HMACSHA256,  //prf : pseudo random functionality it rendomise the hash
                            iterationCount : 1000000,
                            numBytesRequested : 256/8
                );
                return tempPasswordHash;
    }



    //creating the token
    public string CreateToken(int userId)
    {

        Claim[] claims = new Claim[]{
            new Claim ("UserId",userId.ToString())
        };

        // string? tokenKeyValue = _config.GetSection("Appsettings:TokenKey").Value;

        //signature
        SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _config.GetSection("Appsettings:TokenKey").Value
                )
            );
        
        SigningCredentials credentials = new SigningCredentials(
            tokenKey,
            SecurityAlgorithms.HmacSha512Signature
        );

        SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = credentials,
            Expires = DateTime.Now.AddDays(1)
        };

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        
        SecurityToken token = tokenHandler.CreateToken(descriptor);

        return tokenHandler.WriteToken(token);

    }
    }
}