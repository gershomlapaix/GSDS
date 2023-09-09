namespace GsdsV2.Utils
{
    public class Tobeused
    {
        //using var scope = app.Services.CreateScope();
        //{
        //    using (var context = scope.ServiceProvider.GetRequiredService<GsdsDb>())
        //    {
        //        //Get command for non entity types
        //        //var User = context.Database.SqlQuery<User>($"select * from ADMIN.dbo.Users where UserID = 101 order by userid DESC OFFSET 0 ROWS").FirstOrDefault();
        //        //return TypedResults.Ok(User);
        //        //
        //        var users = context.Users.FromSql($"SELECT TOP 10 PERCENT * FROM ADMIN.dbo.Users").ToList<User>();
        //        return TypedResults.Ok(users);
        //    }
        //}




        //return TypedResults.Ok(PasswordEncoder.passwordDecrypt(PasswordEncoder.passwordEncrypt("mypass_!!word")));




        /*-----------------------------------------
        // TO BE DISCUSSED ON
        app.MapGet("/getit", async(HttpContext context) => {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return TypedResults.Ok(userId);
        });

        app.MapGet("/getit", async (ClaimsPrincipal user) =>
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier).Value;
            return TypedResults.Ok(userId);
        }).RequireAuthorization();


        // app.MapPost("/register", async(User user, GsdsDb db)=>{
        //     db.Users.Add(user);
        //             await db.SaveChangesAsync();

        //             return TypedResults.Created($"/api/users/{user.Email}", user);
        // })
        // .Accepts<UserLogin>("application/json")
        // .Produces<string>();

        // Movie endpoints
        app.MapPost("/", 
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles ="Administrator")]
        (Movie movie, IMovieService service) => CreateMovie(movie, service))
        .Accepts<Movie>("/application/json")
        .Produces<Movie>(statusCode:201, contentType: "application/json");

        app.MapGet("/",
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles ="Administrator, User")]
        (IMovieService service)=> GetAllMovies(service));

        app.MapGet("/:id", (int id, IMovieService service) => GetOne(id, service));

        app.MapPatch("/:id",
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles ="Administrator")]
        (int id, IMovieService service, Movie newMovie)=> UpdateMovie(id, service,newMovie));

        // ---------------------

        // Movie methods definition
        // create
        IResult CreateMovie(Movie movie, IMovieService service){
            var result = service.Create(movie);

            return Results.Ok(result);
        } 

        // get movies
        IResult GetAllMovies(IMovieService service){
            var movies = service.AllMovies();
            return Results.Ok(movies);
        }

        // get one movie
        IResult GetOne(int id, IMovieService service){
            var movie = service.Get(id);
            if(movie is not null) return Results.Ok(movie);
            return Results.NotFound("Movie with such id is not found");

        }

        // update a movie
        IResult UpdateMovie(int id, IMovieService service, Movie newMovie){
            var movie = service.Update(id, newMovie);
            if(movie is not null) {
                return Results.Ok(movie);
            }   

            return Results.NotFound("Movie is not found");
        }
       */


    }
}
