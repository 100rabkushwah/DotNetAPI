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


    /// <summary>
    /// This is the post API which is used for getting all the post from the table Posts 
    /// this API is using the JWT token
    /// </summary>
    /// <returns></returns>
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



    /// <summary>
    /// Will return the single post 
    /// but you will have to give the postId (int)
    /// </summary>
    /// <param name="postId"></param>
    /// <returns></returns>

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


    /// <summary>
    /// This API will retun the array of post using user Id it is using the JWT token also
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
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



    /// <summary>
    /// as you will call this API then this will only retirn the your post insted of returning other post... if you will try to access
    /// other user post then it will give error because it is using JWT token
    /// </summary>
    /// <returns></returns>
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


    /// <summary>
    /// Search the user post using key word used in posttile and postcontent 
    /// </summary>
    /// <param name="searchParam"></param>
    /// <returns></returns>

    [HttpGet("PostBySearch/{searchParam}")]
    public IEnumerable<Post> PostBySearch(string searchParam)
    {
        string sql = @"SELECT * FROM TutorialAppSchema.Posts 
                            where 
                                PostTitle LIKE '% " + searchParam + 
                                " %' OR PostContent LIKE '% " + searchParam + "%'";
        return _dataContextDapper.LoadData<Post>(sql);
    }


    /// <summary>
    /// this will only add the post for JWT token contain user ID
    /// </summary>
    /// <param name="postToAddDtos"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
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


    /// <summary>
    /// this will only update your post only for your user id
    /// </summary>
    /// <param name="postToEditDtos"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
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