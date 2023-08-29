using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using GsdsAuth.Models;
using GsdsAuth.Services;

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

Console.WriteLine(builder.Configuration["Jwt:SecretKey"]);
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
app.MapPost("/login", (UserLogin user, IUserService service)=> Login(user, service))
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


// Auth methods definition
IResult Login(UserLogin user, IUserService service){
    if(!string.IsNullOrEmpty(user.Username) && !string.IsNullOrEmpty(user.Password)){
        var loggerInUser = service.GetUser(user);

        if(loggerInUser is null) {
            return Results.NotFound("User not found");
        }

        // else

        var claims = new[]{
            new Claim(ClaimTypes.NameIdentifier, loggerInUser.Username),
            new Claim(ClaimTypes.Email, loggerInUser.Email),
            new Claim(ClaimTypes.GivenName, loggerInUser.FirstName),
            new Claim(ClaimTypes.Surname, loggerInUser.LastName),
            new Claim(ClaimTypes.Role, loggerInUser.Role)
        };

        // var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]));
        // var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
        issuer: builder.Configuration["Jwt:Issuer"],
        audience: builder.Configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.Now.AddMinutes(15),
        notBefore:DateTime.UtcNow,
        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])),
        SecurityAlgorithms.HmacSha256)
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return Results.Ok(tokenString);
    }
    else{
        return Results.BadRequest("Enter the data");
    }
}

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
