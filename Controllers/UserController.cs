using DotnetAPI.Data;
using DotnetAPI.DTOs;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private DataContextDapper _dataContextDapper;
    public UserController(IConfiguration config)
    {
        Console.WriteLine(config .GetConnectionString("DefaultConnection"));
        _dataContextDapper = new DataContextDapper(config);
    }
    // [HttpGet("Test")]
    // public string[] Test()
    // {
    //     string[] UserResponse = new string[]{
    //         "test1",
    //         "test2"
    //     };
    //     return UserResponse;
    // }

    // [HttpGet("TestValue/{testValue}")]  //{this is showing value is required}
    // public string[] TestValue(string testValue)
    // {
    //     string[] UserResponse = [
    //         "test1",
    //         "test2",
    //         testValue
    //     ];
    //     UserResponse.Append(testValue);
    //     return UserResponse;
    // }

    // [HttpGet(Name = "TestValue")]
    // public string[] TestValue(string testValue)
    // {

    //     string[] UserResponse = new string[]{
    //         "test1",
    //         "test2"
    //     };
    //     UserResponse[1] = testValue;
    //     return UserResponse;
    // }

    [HttpGet("GetUsers")] 
    public IEnumerable<User> GetUsers()
    {
        string sql = @"SELECT [UserId],
                        [FirstName],
                        [LastName],
                        [Email],
                        [Gender],
                        [Active] 
                    from TutorialAppSchema.Users";
        IEnumerable<User> users = _dataContextDapper.LoadData<User>(sql);
        return users;
    }
    [HttpGet("GetSingleUsers/{userId}")] 
    public User GetSingleUser(int userId)
    {
        string sql = @"SELECT [UserId],
                        [FirstName],
                        [LastName],
                        [Email],
                        [Gender],
                        [Active] 
                    from TutorialAppSchema.Users where UserId = " + userId.ToString();

        User user = _dataContextDapper.LoadDataSinge<User>(sql);
        return user;
        
    }

    [HttpPost("AddNewUser")]
    public IActionResult AddUser(User user)
    {
        string sql = @"INSERT INTO TutorialAppSchema.Users (
                            [FirstName],
                            [LastName],
                            [Email],
                            [Gender],
                            [Active] ) values('" + user.FirstName?.Replace("'","''") 
                                            +  "','" + user.LastName?.Replace("'","''") 
                                            +  "','" +user.Email
                                            +  "','" +user.Gender
                                            +  "'," + Convert.ToInt32(user.Active)
                            
                            + ")";
        if(_dataContextDapper.ExecuteSql(sql))
        return Ok();
        throw new Exception("Failed To Update user");
        
    }

    [HttpPost("AddNewUserDTOs")]
    public IActionResult AddUserDtos(AddUserDtos user)
    {
        string sql = @"INSERT INTO TutorialAppSchema.Users (
                            [FirstName],
                            [LastName],
                            [Email],
                            [Gender],
                            [Active] ) values('" + user.FirstName?.Replace("'","''") 
                                            +  "','" + user.LastName?.Replace("'","''") 
                                            +  "','" +user.Email
                                            +  "','" +user.Gender
                                            +  "'," + Convert.ToInt32(user.Active)
                            
                            + ")";
        if(_dataContextDapper.ExecuteSql(sql))
        return Ok();
        throw new Exception("Failed To Update user");
        
    }

    [HttpPut("EditUser")]
     public IActionResult UpdateUser(User user )
    {
        string sql = @"UPDATE TutorialAppSchema.Users 
                                SET 
                                    [FirstName] = '" + user.FirstName?.Replace("'","''") +
                                    "',[LastName] = '" + user.LastName?.Replace("'","''") +
                                    "',[Email] = '" + user.Email +
                                    "',[Gender] = '" + user.Gender +
                                    "',[Active] = '" + user.Active +
                                    "' where UserId = " + user.UserId;
                                    Console.WriteLine(sql);
        // Console.WriteLine(sql);
        if(_dataContextDapper.ExecuteSql(sql))
        return Ok();
        throw new Exception("Failed To Update user");
        
    }

     [HttpDelete("DeleteUser/{userId}")] 
    public IActionResult DeleteUser(int userId)
    {
        string sql = @"Delete from TutorialAppSchema.Users where UserId = " + userId.ToString();
        _dataContextDapper.ExecuteSql(sql);
        return Ok();
        
    }

}