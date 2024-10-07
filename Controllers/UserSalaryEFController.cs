using DotnetAPI.Data;
using DotnetAPI.DTOs;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;
[ApiController]
[Route("[controller]")]
public class UserSalaryEFController : ControllerBase
{
    private DataContextEF _entityFramework;
    public UserSalaryEFController(IConfiguration config)
    {
        Console.WriteLine(config .GetConnectionString("DefaultConnection"));
        _entityFramework = new DataContextEF(config);
    }

    [HttpGet("GetUserSalary")] 
    public IEnumerable<UserSalary> GetUserSalary()
    {
        IEnumerable<UserSalary> userSalaryDB = _entityFramework.UserSalary.Where(u => u.UserId != 0).ToList<UserSalary>();
        return userSalaryDB;
    }

    [HttpGet("GetSingleUserSalary/{userId}")] 
    public UserSalary GetSingleUser(int userId)
    {
        UserSalary? UserSalaryDB = _entityFramework.UserSalary.Where(u => u.UserId == userId).FirstOrDefault();
        if(UserSalaryDB != null)
        return UserSalaryDB;
        throw new Exception(userId + " doesn't exist into the database");
        
    }

    [HttpPost("AddNewUserSalary")]
    public IActionResult AddNewUserSalary(UserSalary user)
    {
         UserSalary userDB = new UserSalary();
        if(userDB != null)
        {   userDB.Salary = user.Salary;
            userDB.AvgSalary = user.AvgSalary;
           _entityFramework.Add(userDB);
           if( _entityFramework.SaveChanges() > 0)
            return Ok();

        }
        throw new Exception("Failed To Add user");
        
    }

    [HttpPut("EditUser")]
     public IActionResult UpdateUser(UserSalary user )
    {
        UserSalary? UserSalaryEF = _entityFramework.UserSalary.Where(u => u.UserId == user.UserId).FirstOrDefault();
        if(UserSalaryEF != null)
        {
            UserSalaryEF.Salary = user.Salary;
            UserSalaryEF.AvgSalary = user.AvgSalary;
           if( _entityFramework.SaveChanges() > 0)
            return Ok();

        }
        throw new Exception("Failed To Update the user");
        
    }

     [HttpDelete("DeleteUser/{userId}")] 
    public IActionResult DeleteUser(int userId)
    {
        UserSalary? UserSalaryEF = _entityFramework.UserSalary.Where(u => u.UserId == userId).FirstOrDefault();
        if(UserSalaryEF != null)
        {   
            _entityFramework.UserSalary.Remove(UserSalaryEF);
            if( _entityFramework.SaveChanges() > 0 )
             return Ok();
        }
        throw new Exception("Failed to Delete the user");
        
    }

}