using DotnetAPI.Data;
using DotnetAPI.DTOs;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;
[Authorize]
[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
    private readonly DataContextDapper _dataContextDapper;
    public PostController(IConfiguration config)
    {
        _dataContextDapper = new DataContextDapper(config);
    }
    [HttpGet("Posts")]
    public IEnumerable<Post> GetPost()
    {
        string sql = @"SELECT [PostId],
                            [UserId],
                            [PostTitle],
                            [PostCOntent],
                            [PostCreated],
                        [PostUpdated] from TutorialAppSchema.Posts";
        return _dataContextDapper.LoadData<Post>(sql);
    }

    [HttpGet("PostSingle/{postId}")]
    public Post GetPostSingle(int postId)
    {
        string sql = @"SELECT [PostId],
                            [UserId],
                            [PostTitle],
                            [PostCOntent],
                            [PostCreated],
                        [PostUpdated] from TutorialAppSchema.Posts where PostId = " + postId.ToString();
        return _dataContextDapper.LoadDataSinge<Post>(sql);
    }

    [HttpGet("PostByUser/{userId}")]
    public IEnumerable<Post> GetPostByUser(int userId)
    {
        string sql = @"SELECT [PostId],
                            [UserId],
                            [PostTitle],
                            [PostCOntent],
                            [PostCreated],
                        [PostUpdated] from TutorialAppSchema.Posts where UserId = " + userId;
                        Console.WriteLine(sql);
        return _dataContextDapper.LoadData<Post>(sql);
    }

    [HttpGet("MyPosts")]
    public IEnumerable<Post> GetMyPosts()
    {
        string sql = @"SELECT [PostId],
                            [UserId],
                            [PostTitle],
                            [PostCOntent],
                            [PostCreated],
                        [PostUpdated] from TutorialAppSchema.Posts where UserId = " + this.User.FindFirst("userId")?.Value;
        Console.WriteLine(sql);
        return _dataContextDapper.LoadData<Post>(sql);
    }

    [HttpGet("PostBySearch/{searchParam}")]
    public IEnumerable<Post> PostBySearch(string searchParam)
    {
        string sql = @"SELECT * FROM TutorialAppSchema.Posts 
                            where 
                                PostTitle LIKE '% " + searchParam + 
                                " %' OR PostContent LIKE '% " + searchParam + "%'";
        return _dataContextDapper.LoadData<Post>(sql);
    }

    [HttpPost("AddPost")]
    public IActionResult AddPost(PostToAddDtos postToAddDtos)
    {
        string sql = @"INSERT INTO TutorialAppSchema.Posts (
                                [UserId],
                                [PostTitle],
                                [PostContent],
                                [PostCreated],
                                [PostUpdated] ) VALUES ('"+ this.User.FindFirst("userId")?.Value +
                                                        "','" + postToAddDtos.PostTitle + 
                                                        "','" + postToAddDtos.PostContent + 
                                                        "',GETDATE(),GETDATE())";

        if(_dataContextDapper.ExecuteSql(sql))
         return Ok();
        throw new Exception("Failed to Create new post");
                                                        
    }

    [HttpPut("EditPost")]
    public IActionResult EditToPost(PostToEditDtos postToEditDtos)
    {
        string sql = @"UPDATE TutorialAppSchema.Posts 
                        SET PostContent = '" + postToEditDtos.PostContent 
                        + "',PostTitle = '"+ postToEditDtos.PostTitle 
                        + "',PostUpdated = GETDATE() "
                        + "WHERE PostId = " + postToEditDtos.PostId+ 
                        "AND UserId = " + this.User.FindFirst("userId")?.Value;
            //                                             "','" + postToEditDtos.PostTitle + 
            //                                             "','" + postToEditDtos.PostContent + 
            //                                             "','GETDATE()','GETDATE()')";
        Console.WriteLine(sql);
        if(_dataContextDapper.ExecuteSql(sql))
         return Ok();
        throw new Exception("Failed to edit post");                                              
    }

    [HttpDelete("DeletePost/{postId}")]
    public IActionResult DeletePost(int postId)
    {
        string sql = @"DELETE FROM TutorialAppSchema.Posts WHERE PostId = " + postId.ToString() + 
                        "AND UserId = " + this.User.FindFirst("userId")?.Value;
        Console.WriteLine(sql);
        if(_dataContextDapper.ExecuteSql(sql))
         return Ok();
        throw new Exception("Failed to delete post");      
    }
}