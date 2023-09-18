using Gsds.Controllers.Auth;
using Gsds.Controllers.Dossier;
using Gsds.Data;
using GsdsAuth.Models;
using GsdsAuth.Services;
using GsdsV2.Controllers.Dossier;
using GsdsV2.DTO.Dossier;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

public class Startup{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        // Configure the database
        //services.AddDbContext<GsdsDb>(options =>
        //options.UseSqlServer(Configuration.GetConnectionString("GsdsDBConnection")));
        //services.AddDatabaseDeveloperPageExceptionFilter();

       services.AddDbContext<GsdsDb>(
       options => options.UseSqlServer("name=ConnectionStrings:GsdsDBConnection"));

        // For serialization and deserialization
        //services.AddControllers().AddNewtonsoftJson(options =>
        //    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

        // swagger service configuration
        services.AddSwaggerGen(options => {
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(options => {
                            options.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateIssuer = false,
                                ValidateAudience = false,
                                ValidateLifetime = true,
                                ValidateIssuerSigningKey = true,
                                ValidIssuer = Configuration["Jwt:Issuer"],
                                ValidAudience = Configuration["Jwt:Audience"],
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"]))
                            };
                        });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        services.AddAuthorization();

        // register the services
        services.AddEndpointsApiExplorer();
        services.AddSingleton<IUserService, UserService>();

    }

    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
        }

        // authenticate and authorization
        app.UseAuthorization();
        app.UseAuthentication();


        // provide swagger ui
        app.UseSwaggerUI();

        // ------------- AUTH ROUTES
        RouteGroupBuilder appRoutes = app.MapGroup("/api");

        appRoutes.MapPost("/auth/signup", UserController.UserRegister);
        //.Accepts<UserDto>("application/json");

        //appRoutes.MapPost("/auth/login", (UserLogin user, GsdsDb db) => LoginController.Login(builder, user, db));

        appRoutes.MapGet("/test", async Task<IResult> (GsdsDb db) =>
        {
            return TypedResults.Ok(await db.ComplaintTypes.ToArrayAsync());
        });


        // ------------- COMPLAINER ROUTES
        appRoutes.MapGet("/complainer",
               //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")] 
               (GsdsDb db) =>  ComplainerController.GetAllComplainers(db));

        appRoutes.MapGet("/complainer/{complainerNId}",
       //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
       (GsdsDb db) => ComplainerController.getSingleComplainer);

        //appRoutes.MapPost("/complainer",
        //        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
        //        (ComplainerDto complainerDto, ClaimsPrincipal user, GsdsDb db) => ComplainerController.RegisterComplainer(complainerDto, user, db));


        // ------------- ACCUSED ROUTES
        appRoutes.MapPost("/accused",
               //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
               (AccusedDto accusedDto, GsdsDb db) => AccusedController.RegisterAccused(accusedDto, db));

        appRoutes.MapGet("/accused",
              //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
              (GsdsDb db) => AccusedController.getAllAccuseds(db));

        // ------------ COMPLAINT
        appRoutes.MapGet("/complaint",
             //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
             (GsdsDb db) => ComplaintController.getAllComplaints(db));

        appRoutes.MapGet("/complaint/{complaintCode}", ComplaintController.getOneComplaint);

        appRoutes.MapPost("/complaint",
            //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
            (ComplaintDto complaint, GsdsDb db) => ComplaintController.createComplaint(complaint, db));


        app.Run();
    }
}
