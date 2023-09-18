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
using Gsds.Controllers.Dossier;
using Gsds.Models.Dossier;
using System.Security.Claims;
using GsdsV2.Utils;
using Microsoft.AspNetCore.Mvc;
using GsdsV2.DTO;
using GsdsV2.DTO.Dossier;
using System.Data;
using GsdsV2.Controllers.Dossier;
using System.Text.Json.Serialization;

public class Program
{
    public static void Main(string[] args)
    {
        var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: MyAllowSpecificOrigins,
                              policy =>
                              {
                                  policy.WithOrigins("https://ombudsman-frontend.vercel.app");
                              });
        });

        // For serialization and deserialization
        //builder.Services.AddControllers().AddNewtonsoftJson();
        builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy = null;
            options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });
        builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

        // swagger service configuration
        builder.Services.AddSwaggerGen(options => {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Scheme = "Bearer",
                BearerFormat = "JWT",
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
                        .AddJwtBearer(options => {
                            options.TokenValidationParameters = new TokenValidationParameters
                            {
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


        // Configure the database
        builder.Services.AddDbContext<GsdsDb>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("GsdsDBConnection")));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddAuthorization();

        // register the services
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSingleton<IUserService, UserService>();

        var app = builder.Build();

        // -------------- OTHER CONFIGURATIONS
        app.UseSwagger();    // use swagger

        app.UseCors(MyAllowSpecificOrigins);

        // authenticate and authorization
        app.UseAuthorization();
        app.UseAuthentication();


        // ------------- AUTH ROUTES
        RouteGroupBuilder appRoutes = app.MapGroup("/api");

        appRoutes.MapPost("/auth/signup", UserController.UserRegister);
        //.Accepts<UserDto>("application/json");

        appRoutes.MapPost("/auth/login", (UserLogin user, GsdsDb db) => LoginController.Login(builder, user, db));

        appRoutes.MapGet("/test", async Task<IResult> (GsdsDb db) =>
        {
            return TypedResults.Ok(await db.Provinces.Include(p=> p.Complaints).ToArrayAsync());
        });


        // ------------- COMPLAINER ROUTES
        appRoutes.MapGet("/complainer",
               [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
        (GsdsDb db) => ComplainerController.GetAllComplainers(db));

        appRoutes.MapPost("/complainer",
                [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
        (ComplainerDto complainerDto, ClaimsPrincipal user, GsdsDb db) => ComplainerController.RegisterComplainer(complainerDto, user, db));


        // ------------- ACCUSED ROUTES
        appRoutes.MapPost("/accused",
               [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
        (AccusedDto accusedDto, GsdsDb db) => AccusedController.RegisterAccused(accusedDto, db));

        appRoutes.MapGet("/accused",
              [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
        (GsdsDb db) => AccusedController.getAllAccuseds(db));

        // ------------ COMPLAINT
        appRoutes.MapGet("/complaint",
             [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
        (GsdsDb db) => ComplaintController.getAllComplaints(db));

        appRoutes.MapGet("/complaint/{complaintCode}",
             [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
        (string complaintCode, GsdsDb db) => ComplaintController.getOneComplaint(complaintCode, db));

        appRoutes.MapPost("/complaint",
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
        (ComplaintDto complaint, GsdsDb db) => ComplaintController.createComplaint(complaint, db));
        // provide swagger ui
        app.UseSwaggerUI();
        app.Run();
    }
}