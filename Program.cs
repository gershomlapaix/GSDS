using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using GsdsAuth.Models;
using GsdsAuth.Services;
using Gsds.Data;
using Microsoft.EntityFrameworkCore;
using Gsds.Controllers.Auth;

var builder = WebApplication.CreateBuilder(args);

// swagger service configuration
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme{
        Scheme = "Bearer",
        BearerFormat="JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "APIs for GSDS system",
        Type = SecuritySchemeType.Http
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {
            new OpenApiSecurityScheme{
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
            }
        },
        new List<string>()
        }
    });
});

try
{
    // for authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options=> {
                options.TokenValidationParameters = new TokenValidationParameters{
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
                };
               });
}
catch (Exception ex)
{
   Console.WriteLine(ex);
}

/*
// Configure the database
builder.Services.AddDbContext<GsdsDb>(options => 
options.UseSqlServer(builder.Configuration.GetConnectionString("GsdsDBConnection")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
*/

builder.Services.AddDbContext<GsdsDb>(opt => opt.UseInMemoryDatabase("GsdsDB"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddAuthorization();

// register the services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IMovieService, MovieService>(); 
builder.Services.AddSingleton<IUserService, UserService>();

var app = builder.Build();

app.UseSwagger();    // use swagger

// authenticate and authorization
app.UseAuthorization();
app.UseAuthentication();

// Auth endpoints
app.MapPost("/login", (UserLogin user, IUserService service)=> LoginController.Login(builder,user, service));
// app.MapPost("/register", () => UserController.UserRegister)
app.MapPost("/register", async(User user, GsdsDb db)=>{
     db.Users.Add(user);
            await db.SaveChangesAsync();

            return TypedResults.Created($"/api/users/{user.Email}", user);
})
.Accepts<UserLogin>("application/json")
.Produces<string>();


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

// provide swagger ui
app.UseSwaggerUI();
app.Run();
