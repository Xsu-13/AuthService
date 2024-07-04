using Auth.AuthService.Contracts;
using Auth.AuthService.Services;

namespace Auth.AuthService.Endpoints
{
    public static class PostsEndpoints
    {

        static List<Post> posts = new List<Post>();

        public static IEndpointRouteBuilder MapPostEndpoints(this IEndpointRouteBuilder builder)
        {
            var endpoints = builder.MapGroup("Post").RequireAuthorization();

            endpoints.MapGet("/", GetAll);
            endpoints.MapPost("add", Add);

            return builder;
        }

        private static async Task<IResult> GetAll()
        {
            return Results.Ok(posts);
        }

        private static async Task<IResult> Add(string title, string description)
        {
            Post post = new Post(title, description);

            posts.Add(post);

            return Results.Ok();
        }
    }


    record Post(string  _title = "", string _description = "")
    {
        string title = _title;
        string description = _description;
    }
}
