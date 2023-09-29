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
using Microsoft.AspNetCore.Authentication.Certificate;
using GsdsV2.Controllers.Dossier.HelperControllers;
using GsdsV2.Models.HelperModels;
using GsdsV2.Services;
using GsdsV2.Controllers;

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
            builder.Services.AddAuthentication(
                CertificateAuthenticationDefaults.AuthenticationScheme)
            .AddCertificate();

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
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSingleton<IUserService, UserService>();
        builder.Services.AddScoped<IEmailService, EmailService>();

        var app = builder.Build();

        // -------------- OTHER CONFIGURATIONS
        app.UseSwagger();    // use swagger

        app.UseCors(MyAllowSpecificOrigins);

        // authenticate and authorization
        app.UseAuthorization();
        app.UseAuthentication();


        // ------------- AUTH ROUTES
        RouteGroupBuilder appRoutes = app.MapGroup("/api");

        appRoutes.MapGet("/", () => "Gsds Apis");

        appRoutes.MapGet("/test", async Task<IResult> (GsdsDb db) =>
        {
            return TypedResults.Ok(await db.Provinces.Include(p => p.Complaints).ToArrayAsync());
        }).WithTags("Test");

        appRoutes.MapPost("/auth/signup", UserController.UserRegister).WithTags("User");

        appRoutes.MapPost("/auth/login", (UserLogin user, GsdsDb db) => LoginController.Login(builder, user, db)).WithTags("Auth");

        // -------- For Users actions
        appRoutes.MapPost("/users/complaints",
                [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43,00049")]
        (ClaimsPrincipal user, GsdsDb db) => UserController.getMyRoleComplaints(user, db)).WithTags("UserActions");

        appRoutes.MapGet("/users/loggedin",
               [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43,00049")]
        (ClaimsPrincipal user, GsdsDb db) => UserController.getLoggedInUser(user, db)).WithTags("UserActions");

        // ------------- COMPLAINER ROUTES
        appRoutes.MapGet("/complainer",
               [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
        (GsdsDb db) => ComplainerController.GetAllComplainers(db)).WithTags("Complainer");

        appRoutes.MapPost("/complainer",
                [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
        (ComplainerDto complainerDto, ClaimsPrincipal user, GsdsDb db) => ComplainerController.RegisterComplainer(complainerDto, user, db)).WithTags("Complainer");


        // ------------- ACCUSED ROUTES
        appRoutes.MapPost("/accused",
               [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
        (AccusedDto accusedDto, GsdsDb db) => AccusedController.RegisterAccused(accusedDto, db)).WithTags("Accused");

        appRoutes.MapGet("/accused",
              [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43,00049")]
        (GsdsDb db) => AccusedController.getAllAccuseds(db)).WithTags("Accused");

        // ------------ COMPLAINT
        appRoutes.MapGet("/complaint",
             [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43, 00049")]
        (GsdsDb db) => ComplaintController.getAllComplaints(db)).WithTags("Complaint");

        appRoutes.MapGet("/complaint/{complaintCode}",
             [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43, 00049")]
        (string complaintCode, GsdsDb db) => ComplaintController.getOneComplaint(complaintCode, db)).WithTags("Complaint");

        appRoutes.MapGet("/complaint/{complaintCode}/files",
             [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43,00049")]
        (string complaintCode, GsdsDb db) => ComplaintController.getComplaintFiles(complaintCode, db)).WithTags("Complaint");

        appRoutes.MapPost("/complaint",
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43,00049")]
        (ComplaintDto complaint, GsdsDb db) => ComplaintController.createComplaint(complaint, db)).WithTags("Complaint");


        // ------------- For files
        appRoutes.MapPost("/file/upload",
          [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43,00049")]
        (IFormFileCollection files, string complaintCode, GsdsDb db) => FileHandlingController.uploadFiles(files, complaintCode, db)
        ).WithTags("File");

        appRoutes.MapGet("/files",
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43,00049")]
        (GsdsDb db) => FileHandlingController.getFiles(db)).WithTags("File");

        appRoutes.MapGet("/files/{fileId}",
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43,00049")]
        (int fileId, GsdsDb db) => FileHandlingController.downloadFile(fileId, db)).WithTags("File");


        // ---------- For country

        appRoutes.MapGet("/country",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
            (GsdsDb db)=> CountryController.getAllCountries(db)
            ).WithTags("Country");

        appRoutes.MapGet("/country/{countryId}",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
            (string countryId, GsdsDb db) => CountryController.getSingleCountry(countryId, db)).WithTags("Country");

        appRoutes.MapGet("/country/countryName/{countryName}",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
            (string countryName, GsdsDb db) => CountryController.getCountryByName(countryName, db)).WithTags("Country");


        // ---------- For Provinces

        appRoutes.MapGet("/province/{provinceId}/districts",
            //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
        (string provinceId, GsdsDb db) => ProvinceController.getDistrictsByProvince(provinceId, db)
            ).WithTags("Province");

        appRoutes.MapGet("/province/{provinceId}/complaints",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
        (string provinceId, GsdsDb db) => ProvinceController.getComplaintsByProvince(provinceId, db)
            ).WithTags("Province");

        appRoutes.MapGet("/province/{provinceId}/complainers",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
        (string provinceId, GsdsDb db) => ProvinceController.getComplainersByProvince(provinceId, db)
            ).WithTags("Province");

        appRoutes.MapGet("/province/{provinceId}/accuseds",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
        (string provinceId, GsdsDb db) => ProvinceController.getAccusedByProvince(provinceId, db)
            ).WithTags("Province");


        // ---------- For districts
        appRoutes.MapGet("/district/{districtId}/sectors",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
        (string districtId, GsdsDb db) => DistrictController.getSectorByDistrict(districtId, db)
            ).WithTags("District");

        appRoutes.MapGet("/district/{districtId}/complaints",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
        (string districtId, GsdsDb db) => DistrictController.getComplaintsByDistrict(districtId, db)
            ).WithTags("District");

        appRoutes.MapGet("/district/{districtId}/complainers",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
        (string districtId, GsdsDb db) => DistrictController.getComplainersByDistrict(districtId, db)
            ).WithTags("District");

        appRoutes.MapGet("/district/{districtId}/accuseds",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
        (string districtId, GsdsDb db) => DistrictController.getAccusedByDistrict(districtId, db)
            ).WithTags("District");

        // ---------- For sectors
        appRoutes.MapGet("/sector/{sectorId}/cells",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
        (string sectorId, GsdsDb db) => SectorController.getCellBySector(sectorId, db)
            ).WithTags("Sector");

        appRoutes.MapGet("/sector/{sectorId}/complaints",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
        (string sectorId, GsdsDb db) => SectorController.getComplaintsBySector(sectorId, db)
            ).WithTags("Sector");

        appRoutes.MapGet("/sector/{sectorId}/complainers",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
        (string sectorId, GsdsDb db) => SectorController.getComplainersBySector(sectorId, db)
            ).WithTags("Sector");

        appRoutes.MapGet("/sector/{sectorId}/accuseds",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
        (string sectorId, GsdsDb db) => SectorController.getAccusedBySector(sectorId, db)
            ).WithTags("Sector");

        // ---------- For Cellls
        appRoutes.MapGet("/cell/{cellId}/complaints",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
        (string cellId, GsdsDb db) => CellController.getComplaintsByCell(cellId, db)
            ).WithTags("Cell");

        appRoutes.MapGet("/cell/{cellId}/complainers",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
        (string cellId, GsdsDb db) => CellController.getComplainersByCell(cellId, db)
            ).WithTags("Cell");

        appRoutes.MapGet("/cell/{cellId}/accuseds",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
        (string cellId, GsdsDb db) => CellController.getAccusedByCell(cellId, db)
            ).WithTags("Cell");

        // --------- OTHER HELPERS
        appRoutes.MapGet("/marital-status",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
        (GsdsDb db) => MaritalStatusController.getallMaritalStatus(db)
            ).WithTags("MaritalStatus");

        appRoutes.MapGet("/marital-status/{id}",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
            (string id, GsdsDb db) => MaritalStatusController.getMaritalStatusById(id, db)).WithTags("MaritalStatus");


        appRoutes.MapGet("/person-type",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
        (GsdsDb db) => PersonTypeController.getPersonTypes(db)
            ).WithTags("PersonType");

        appRoutes.MapGet("/identifier-type",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
        (GsdsDb db) => IdentifierTypeController.getAllIdentifierTypes(db)
            ).WithTags("IdentifierType");

        // ------------ For roles
        appRoutes.MapGet("/roles",
       [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001, 43")]
        (GsdsDb db) => ManagerRolesController.getAllRoles(db)
           ).WithTags("ManagerRoles");

        appRoutes.MapGet("/roles/{roleId}/complaints",
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
        (string roleId, GsdsDb db) => ManagerRolesController.getRoleComplaints(roleId, db)
            ).WithTags("ManagerRoles");


        // ---------- For Institutions
        appRoutes.MapGet("/institution",
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "43")]
        (GsdsDb db) => InstitutionController.getInstitutions(db)
            ).WithTags("Institution");

        appRoutes.MapPost("/email", (EmailDto request, IEmailService emailService) =>
        {
            EmailController.sendEmail(request, emailService);
        }).WithTags("Email");

        // provide swagger ui
        app.UseSwaggerUI();
        app.Run();
    }
}