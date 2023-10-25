using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using aspnet_blog_application.Models;
using aspnet_blog_application.Models.ViewModels;
using Microsoft.Data.Sqlite;

namespace aspnet_blog_application.Controllers;

public class PostsController : Controller
{
    private readonly ILogger<PostsController> _logger;

    private readonly IConfiguration _configuration;

    public PostsController(ILogger<PostsController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        var postListViewModel = GetAllPosts();
        return View(postListViewModel);
    }

    public IActionResult NewPost()
    {
        return View();
    }

    public IActionResult EditPost(int id) 
    {
        var post = GetPostById(id);
        var postViewModel = new PostViewModel();
        postViewModel.Post = post;
        return View(postViewModel);
    }

    public IActionResult ViewPost(int id) 
    {
        var post = GetPostById(id);
        var postViewModel = new PostViewModel();
        postViewModel.Post = post;
        return View(postViewModel);
    }

    internal PostModel GetPostById(int id)
    {
        PostModel post = new();

        using (var connection = new SqliteConnection(_configuration.GetConnectionString("BlogDataContext")))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = $"SELECT * FROM post Where Id = '{id}'";

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        post.Id = reader.GetInt32(0);
                        post.Title = reader.GetString(1);
                        post.Body = reader.GetString(2);
                    }
                    else
                    {
                        return post;
                    }
                };
            }
        }

        return post;
    }

    internal PostViewModel GetAllPosts()
    {
        List<PostModel> postList = new();

        using (SqliteConnection connection = new SqliteConnection(_configuration.GetConnectionString("BlogDataContext")))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = "SELECT * FROM post";

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            postList.Add(
                                new PostModel
                                {
                                    Id = reader.GetInt32(0),
                                    Title = reader.GetString(1),
                                    Body = reader.GetString(2),
                                });
                        }
                    }
                    else
                    {
                        return new PostViewModel
                        {
                            PostList = postList
                        };
                    }
                };
            }
        }

        return new PostViewModel
        {
            PostList = postList
        };
    }

    public ActionResult Insert(PostModel post)
    {
        // Validate the length of post.Body
        if (post.Body.Length > 5000)
        {
            // Body length exceeds the limit, redirect with failure flag
            return RedirectToAction("NewPost", new { inserted = "false" });
        }

        post.CreatedAt = DateTime.Now;
        post.UpdatedAt = DateTime.Now;

        using (SqliteConnection connection = new SqliteConnection(_configuration.GetConnectionString("BlogDataContext")))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = $"INSERT INTO post (Title, Body, CreatedAt, UpdatedAt) VALUES ('{post.Title}', '{post.Body}', '{post.CreatedAt}', '{post.UpdatedAt}')";
                try
                {
                    command.ExecuteNonQuery();
                    // Post added successfully, redirect with success flag
                    return RedirectToAction("NewPost", new { inserted = "true" });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    // Failed to add post, redirect with failure flag
                    return RedirectToAction("NewPost", new { inserted = "false" });
                }
            }
        }
    }



    public IActionResult Update(PostModel post)
    {
        post.UpdatedAt = DateTime.Now;

        using (SqliteConnection connection = new SqliteConnection(_configuration.GetConnectionString("BlogDataContext")))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = $"UPDATE post SET Title = '{post.Title}', Body = '{post.Body}', UpdatedAt = '{post.UpdatedAt}' WHERE Id = '{post.Id}'";
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    // Optionally handle errors here
                    return RedirectToAction("EditPost", new { id = post.Id, updated = "false" });
                }
            }
        }

        return RedirectToAction("ViewPost", new { id = post.Id, updated = "true" });
    }



    [HttpPost]
    public JsonResult Delete(int id)
    {
        using (SqliteConnection connection = new SqliteConnection(_configuration.GetConnectionString("BlogDataContext")))
        {
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = $"DELETE from post WHERE Id = {id}";
                command.ExecuteNonQuery();
            }
        }

        return Json(new { success = true });
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
