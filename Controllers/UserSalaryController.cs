using DotnetAPI.Data;
using DotnetAPI.DTOs;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DotnetAPI.Controllers;
[ApiController]
[Route("[controller]")]
public class UserSalaryController : ControllerBase
{
    private DataContextDapper _dataContextDapper;
    public UserSalaryController(IConfiguration config)
    {
        Console.WriteLine(config .GetConnectionString("DefaultConnection"));
        _dataContextDapper = new DataContextDapper(config);
    }
    

    [HttpGet("GetUsersSalary")] 
    public IEnumerable<UserSalary> GetUserSalary()
    {
        string sql = @"SELECT [UserId],
                            [Salary],
                            [AvgSalary]
                    from TutorialAppSchema.UserSalary";
        IEnumerable<UserSalary> userSalary = _dataContextDapper.LoadData<UserSalary>(sql);
        return userSalary;
    }
    [HttpGet("GetSingleUserSalary/{userId}")] 
    public UserSalary GetSingleUserSalary(int userId)
    {
        string sql = @"SELECT [UserId],
                            [Salary],
                            [AvgSalary]
                    from TutorialAppSchema.UserSalary where UserId = " + userId.ToString();

        UserSalary userSalary = _dataContextDapper.LoadDataSinge<UserSalary>(sql);
        return userSalary;
        
    }

    [HttpPost("AddNewUserSalary")]
    public IActionResult AddUserSalary(AddUseSalaryDtos userSalary)
    {
        string sql = @"INSERT INTO TutorialAppSchema.UserSalary (
                           [Salary],
                            [AvgSalary]
                             ) values('" + userSalary.Salary
                                        +  "','" + userSalary.AvgSalary   
                            + "')";
                            Console.WriteLine(sql);
        if(_dataContextDapper.ExecuteSql(sql))
        return Ok();
        throw new Exception("Failed To Update user");
        
    }

    // [HttpPost("AddNewUserSalaryDTOs")]
    // public IActionResult AddUserDtos(AddUseSalaryDtos userSalaryDtos)
    // {
    //     string sql = @"INSERT INTO TutorialAppSchema.UserSalary (
    //                         [UserId],
    //                        [Salary],
    //                         [AvgSalary]
    //                          ) values('" + userSalaryDtos.UserId
    //                                     + "','" + userSalaryDtos.Salary
    //                                     +  "','" + userSalaryDtos.AvgSalary   
    //                         + "')";
        
    //     Console.WriteLine(sql);
    //     if(_dataContextDapper.ExecuteSql(sql))
    //     return Ok();
    //     throw new Exception("Failed To Update user");
        
    // }

    [HttpPut("EditUserSalary")]
     public IActionResult UpdateUserSalary(UserSalary userSalary )
    {
        string sql = @"UPDATE TutorialAppSchema.UserSalary 
                                SET 
                                    [Salary] = '" + userSalary.Salary +
                                    "',[AvgSalary] = '" + userSalary.AvgSalary +
                                    "' where UserId = " + userSalary.UserId;
                                    Console.WriteLine(sql);
        Console.WriteLine(sql);
        if(_dataContextDapper.ExecuteSql(sql))
        return Ok();
        throw new Exception("Failed To Update user");
        
    }

     [HttpDelete("DeleteUserSalary/{userId}")] 
    public IActionResult DeleteUserSalary(int userId)
    {
        string sql = @"Delete from TutorialAppSchema.UserSalary where UserId = " + userId.ToString();
        _dataContextDapper.ExecuteSql(sql);
        return Ok();
        
    }

}