using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.IO.Compression;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DotnetAPI.Data;
using DotnetAPI.DTOs;
using DotnetAPI.Helpers;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DotnetAPI.Controllers;
[Authorize]
[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private DataContextDapper _dataContextDapper;
    private readonly IConfiguration _config;
    private readonly AuthHelper _authHelper;
    public AuthController(IConfiguration config)
    {
        _config = config;
        _dataContextDapper = new DataContextDapper(config);
        _authHelper = new AuthHelper(config);
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    public IActionResult Register(UserForRegistrationDtos userForRegistration)
    {
        if(userForRegistration.Password == userForRegistration.PasswordConfirm)
        {
            string sqlCheckForUserString = "select Email FROM TutorialAppSchema.Auth WHERE Email = '"+ userForRegistration.Email +"'";
            IEnumerable<string> existingUsers = _dataContextDapper.LoadData<string>(sqlCheckForUserString);
            if(existingUsers.Count() == 0)
            {
                byte[] tempPasswordSalt = new byte[128/8];
                using(RandomNumberGenerator rng = RandomNumberGenerator.Create())
                {
                    rng.GetNonZeroBytes(tempPasswordSalt);
                }
                // string tempPasswordSaltPlusPasswordKey = _config.GetSection( "AppSettings:PasswordKey").Value + Convert.ToBase64String(tempPasswordSalt);
                //now we are going to create actual password hash
                byte[] tempPasswordHash = _authHelper.GetPasswordHash(userForRegistration.Password,tempPasswordSalt);
                string sqlAddAuth = @"INSERT INTO TutorialAppSchema.Auth (
                                                Email, 
                                                PasswordHash, 
                                                PasswordSalt) 
                                                    VALUES ('"+userForRegistration.Email +
                                                    "', @PasswordHash, @PasswordSalt)";    // here @PasswordHash ans @PassworSalt are
                                                                                            // just placeholder

                Console.WriteLine(sqlAddAuth);
                List<SqlParameter> sqlParameters = new List<SqlParameter>();

                SqlParameter passwordSaltParameter = new SqlParameter("@PasswordSalt",SqlDbType.VarBinary);
                passwordSaltParameter.Value = tempPasswordSalt;

                 SqlParameter passwordHashParameter = new SqlParameter("@PasswordHash",SqlDbType.VarBinary);
                passwordHashParameter.Value = tempPasswordHash;

                sqlParameters.Add(passwordSaltParameter);
                sqlParameters.Add(passwordHashParameter);

                if(_dataContextDapper.ExecuteSqlWithParameteres(sqlAddAuth,sqlParameters))
                {
                    string sqlAddUser = @"INSERT INTO TutorialAppSchema.Users (
                            [FirstName],
                            [LastName],
                            [Email],
                            [Gender],
                            [Active] ) values('" + userForRegistration.FirstName?.Replace("'","''") 
                                            +  "','" + userForRegistration.LastName?.Replace("'","''") 
                                            +  "','" + userForRegistration.Email
                                            +  "','" +userForRegistration.Gender
                                            +  "','1')";
                    _dataContextDapper.ExecuteSql(sqlAddUser);
                    return Ok();
                }
                throw new Exception("Failed to Register User");
            }
            throw new Exception("User with this email already exist");
        }
        throw new Exception("Password didn't match");
    }

    
    [AllowAnonymous]
    [HttpPost("Login")]
    public IActionResult Login(UserForLoginDtos userForLogin)
    {
        string sqlForHashAndSalt = @"select
                                [PasswordHash],
                                [PasswordSalt] from TutorialAppSchema.Auth where Email = '" + userForLogin.Email +"'";
        UserForLoginConfirmationDtos  userForLoginConfirmationDtos = _dataContextDapper.LoadDataSinge<UserForLoginConfirmationDtos>(sqlForHashAndSalt);
        
        byte[] tempPasswordHash = _authHelper.GetPasswordHash(userForLogin.Password,userForLoginConfirmationDtos.PasswordSalt);

        for(int index = 0; index < tempPasswordHash.Length;index++)
        {
            if(tempPasswordHash.ElementAt(index) != userForLoginConfirmationDtos.PasswordHash[index]){
                return StatusCode(401,"Incorrect Password");
            }
        }

         string userIdSql = @"select UserId FROM TutorialAppSchema.Users WHERE Email = '"+userForLogin.Email+"'";

        int userId = _dataContextDapper.LoadDataSinge<int>(userIdSql);

        // return Ok();
        return Ok(new Dictionary<string,string> {
            {"Token : ", _authHelper.CreateToken(userId)}
        });
    }

    [HttpGet("RefreshToken")]
    public IActionResult RefreshToken()
    {
        //this use is from our controller base
        string userId = User.FindFirst("userId")?.Value +"";
        Console.WriteLine("Hi : "+userId);
        string sqlUserId = @"SELECT UserId from TutorialAppSchema.Users WHERE UserId = " +userId;

        int userIdDB = _dataContextDapper.LoadDataSinge<int>(sqlUserId);

        return Ok(new Dictionary<string,string> {
            {"Token : ", _authHelper.CreateToken(userIdDB)}
        });
    }
   

    //creating the token
    
    
}