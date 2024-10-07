using DotnetAPI.Data;
using DotnetAPI.DTOs;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;
[ApiController]
[Route("[controller]")]
public class UserEFController : ControllerBase
{
    private DataContextEF _entityFramework;
    public UserEFController(IConfiguration config)
    {
        Console.WriteLine(config .GetConnectionString("DefaultConnection"));
        _entityFramework = new DataContextEF(config);
    }

    [HttpGet("GetUsers")] 
    public IEnumerable<User> GetUsers()
    {
        IEnumerable<User> usersDB = _entityFramework.Users.ToList<User>();
        return usersDB;
    }

    [HttpGet("GetSingleUsers/{userId}")] 
    public User GetSingleUser(int userId)
    {
        User? userDB = _entityFramework.Users.Where(u => u.UserId == userId).FirstOrDefault();
        if(userDB != null)
        return userDB;
        throw new Exception(userId + " doesn't exist into the database");
        
    }

    [HttpPost("AddNewUser")]
    public IActionResult AddUser(AddUserDtos user)
    {
        
        User userDB = new User();
        if(userDB != null)
        {
            userDB.FirstName = user.FirstName;
            userDB.LastName = user.LastName;
            userDB.Email = user.Email;
            userDB.Gender = user.Gender;
            userDB.Active = user.Active;
           _entityFramework.Add(userDB);
           if( _entityFramework.SaveChanges() > 0)
            return Ok();

        }
        throw new Exception("Failed To Add user");
        
    }

    [HttpPut("EditUser")]
     public IActionResult UpdateUser(User user )
    {
        User? userDB = _entityFramework.Users.Where(u => u.UserId == user.UserId).FirstOrDefault();
        if(userDB != null)
        {
            userDB.FirstName = user.FirstName;
            userDB.LastName = user.LastName;
            userDB.Email = user.Email;
            userDB.Gender = user.Gender;
            userDB.Active = user.Active;
           if( _entityFramework.SaveChanges() > 0)
            return Ok();

        }
        throw new Exception("Failed To Update the user");
        
    }

     [HttpDelete("DeleteUser/{userId}")] 
    public IActionResult DeleteUser(int userId)
    {
        User? userDB = _entityFramework.Users.Where(u => u.UserId == userId).FirstOrDefault();
        if(userDB != null)
        {   
            _entityFramework.Users.Remove(userDB);
            if( _entityFramework.SaveChanges() > 0 )
             return Ok();
        }
        throw new Exception("Failed to Delete the user");
        
    }

}