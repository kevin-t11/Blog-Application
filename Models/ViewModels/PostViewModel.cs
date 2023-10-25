namespace aspnet_blog_application.Models.ViewModels;

public class PostViewModel
{
    
    public int Id {  get; set; }
    public List<PostModel> PostList { get; set; }
public PostModel Post {get; set;}

}