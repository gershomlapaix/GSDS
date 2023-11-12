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
using Microsoft.Extensions.FileProviders;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using GsdsV2.Controllers.UserRelated;

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

        builder.Services.AddMvc()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
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
        builder.Services.AddScoped<IPathProvider, PathProvider>();
        builder.Services.AddScoped<IEmailService, EmailService>();
        var app = builder.Build();

        // -------------- OTHER CONFIGURATIONS
        app.UseSwagger();    // use swagger

        app.UseCors(MyAllowSpecificOrigins);

        // authenticate and authorization
        app.UseAuthorization();
        app.UseAuthentication();

        app.UseStaticFiles();
        //app.UseDirectoryBrowser(new DirectoryBrowserOptions
        //{
        //    FileProvider = new PhysicalFileProvider(
        //        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
        //    RequestPath = "/wwwroot"
        //});


        // ------------- AUTH ROUTES
        RouteGroupBuilder appRoutes = app.MapGroup("/api");

        appRoutes.MapGet("/", () => "Gsds Apis");

        appRoutes.MapGet("/test", async Task<IResult> (GsdsDb db) =>
        {
            return TypedResults.Ok(await db.Provinces.Include(p => p.Complaints).ToArrayAsync());
        }).WithTags("Test");

        appRoutes.MapPost("/auth/signup", UserController.UserRegister).WithTags("User");

        appRoutes.MapPost("/auth/login", (UserLogin user, GsdsDb db) => LoginController.Login(builder, user, db)).WithTags("Auth");

        // --------- Users
        appRoutes.MapGet("/users/all", (GsdsDb db) => UserController.GetAllUsers(db)).WithTags("UserController");

        appRoutes.MapPost("/auth/signup/admin",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00043, 00004")]
        (UserRegisterDtoAdmin dto, ClaimsPrincipal loggedInUser, IEmailService emailService, GsdsDb db) =>
    UserController.UserRegisterByAdmin(dto, loggedInUser, emailService, db)).WithTags("User");


        // -------- For Users actions
        appRoutes.MapPost("/department/complaints",
                [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00003,00004,00013,00016,00039,00043,00044,00051,00052,00055,00056,00057,00058")]
        (ClaimsPrincipal user, GsdsDb db) => UserController.getMyRoleComplaints(user, db)).WithTags("UserActions");

        appRoutes.MapGet("/users/loggedin",
               [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00003,00004,00013,00016,00039,00043,00044,00051,00052,00055,00056,00057,00058")]
        (ClaimsPrincipal user, GsdsDb db) => UserController.getLoggedInUser(user, db)).WithTags("UserActions");

        //   ----------- USER GROUPS
        appRoutes.MapGet("/users/groups", (GsdsDb db) => GroupsController.GetAllGroups(db)).WithTags("UserGroups");
        appRoutes.MapGet("/users/groups/{groupId}", (string groupId, GsdsDb db) => GroupsController.GetgroupById(groupId, db)).WithTags("UserGroups");

        // ----------- USER DEPARTMENTS
        appRoutes.MapGet("/users/departments", (GsdsDb db) => DepartmentController.GetAllDepartments(db)).WithTags("UserDepartments");
        appRoutes.MapGet("/users/departments/{departmentId}", (string departmentId, GsdsDb db) => DepartmentController.GetDepartmentById(departmentId, db)).WithTags("UserDepartments");

        // ------------- COMPLAINER ROUTES
        appRoutes.MapGet("/complainer",
               [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00003,00004,00013,00016,00039,00043,00044,00051,00052,00055,00056,00057,00058")]
        (GsdsDb db) => ComplainerController.GetAllComplainers(db)).WithTags("Complainer");

        appRoutes.MapGet("/complainer/{complainerId}",
              [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00003,00004,00013,00016,00039,00043,00044,00051,00052,00055,00056,00057,00058")]
        (string complainerId, GsdsDb db) => ComplainerController.GetComplainerById(complainerId, db)).WithTags("Complainer");

        appRoutes.MapGet("/complainer/mydetails",
               [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00003,00004,00013,00016,00039,00043,00044,00051,00052,00055,00056,00057,00058")]
        (ClaimsPrincipal user, GsdsDb db) => ComplainerController.GetMyDetailsByUsername(user, db)).WithTags("Complainer");

        appRoutes.MapGet("/complainer/my-files",
              [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00003,00004,00013,00016,00039,00043,00044,00051,00052,00055,00056,00057,00058")]
        (ClaimsPrincipal user, GsdsDb db) => ComplainerController.GetMyUploadedFiles(user, db)).WithTags("Complainer");

        appRoutes.MapPost("/complainer",
                [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00003,00004,00013,00016,00039,00043,00044,00051,00052,00055,00056,00057,00058")]
        (ComplainerDto complainerDto, ClaimsPrincipal user, GsdsDb db) => ComplainerController.RegisterComplainer(complainerDto, user, db)).WithTags("Complainer");


        // ------------- ACCUSED ROUTES
        appRoutes.MapPost("/accused",
               [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00003,00004,00013,00016,00039,00043,00044,00051,00052,00055,00056,00057,00058")]
        (AccusedDto accusedDto, GsdsDb db) => AccusedController.RegisterAccused(accusedDto, db)).WithTags("Accused");

        appRoutes.MapGet("/accused",
              [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00003,00004,00013,00016,00039,00043,00044,00051,00052,00055,00056,00057,00058")]
        (GsdsDb db) => AccusedController.getAllAccuseds(db)).WithTags("Accused");

        appRoutes.MapGet("/accused/{complaintCode}",
              [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00003,00004,00013,00016,00039,00043,00044,00051,00052,00055,00056,00057,00058")]
        (string complaintCode, GsdsDb db) => AccusedController.getAccusedByComplaintCode(complaintCode, db)).WithTags("Accused");

        // ------------ COMPLAINT
        appRoutes.MapGet("/complaint",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00043, 00052,00012")]
        (GsdsDb db) => ComplaintController.getAllComplaints(db)).WithTags("Complaint");

        appRoutes.MapGet("/complaint/{complaintCode}",
             [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00003,00004,00013,00016,00039,00043,00044,00051,00052,00055,00056,00057,00058")]
        (string complaintCode, GsdsDb db) => ComplaintController.getOneComplaint(complaintCode, db)).WithTags("Complaint");

        appRoutes.MapGet("/complaint/category/{cmpltCategory}",
             [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00003,00004,00013,00016,00039,00043,00044,00051,00052,00055,00056,00057,00058")]
        (string cmpltCategory, GsdsDb db) => ComplaintController.getComplaintByCategory(cmpltCategory, db)).WithTags("Complaint");

        appRoutes.MapGet("/complaint/files/{complaintCode}",
             [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00003,00004,00013,00016,00039,00043,00044,00051,00052,00055,00056,00057,00058")]
        (string complaintCode, GsdsDb db) => ComplaintController.getComplaintFiles(complaintCode, db)).WithTags("Complaint");

        appRoutes.MapGet("/complaint/roles/{complaintCode}",
             [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00003,00004,00013,00016,00039,00043,00044,00051,00052,00055,00056,00057,00058")]
        (string complaintCode, GsdsDb db) => ComplaintController.getComplaintRoles(complaintCode, db)).WithTags("Complaint");

        appRoutes.MapGet("/complaint/loggedin",
             [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00003,00004,00013,00016,00039,00043,00044,00051,00052,00055,00056,00057,00058")]
        (ClaimsPrincipal user, GsdsDb db) => ComplaintController.getLoggedInUserComplaints(user, db)).WithTags("Complaint");

        appRoutes.MapPost("/complaint",
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00003,00004,00013,00016,00039,00043,00044,00051,00052,00055,00056,00057,00058")]
        (ComplaintDto complaint, ClaimsPrincipal user, IEmailService emailService, GsdsDb db) => ComplaintController.createComplaint(complaint, user, emailService, db)).WithTags("Complaint");

        // ------------ COMPLAINT MEMO
        appRoutes.MapPost("/complaint-memo/{complaintCode}", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00003,00004,00013,00016,00039,00043,00044,00051,00055,00056,00057,00058")]
        (string complaintCode, ComplaintMemoDto dto, ClaimsPrincipal user, GsdsDb db) => ComplaintMemoController.AddComplaintMemo(complaintCode,dto, user, db)).WithTags("Complaint-memo");

        appRoutes.MapGet("/complaint-memo/all/{complaintCode}", [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00003,00004,00013,00016,00039,00043,00044,00051,00055,00056,00057,00058")]
        (string complaintCode, GsdsDb db) => ComplaintMemoController.GetAllComplaintMemos(complaintCode, db)).WithTags("Complaint-memo");

        // -------------- COMPLAINT STATUS
        appRoutes.MapGet("/complaint-status",
        ComplaintStatusController.GetAll).WithTags("ComplaintStatus");

        appRoutes.MapGet("/complaint-status/{statusCode}",
        ComplaintStatusController.GetByCode).WithTags("ComplaintStatus");

        // -------------- FOR COMPLAINT MANAGEMENT
        appRoutes.MapPost("/complaint-management/{complaintCode}",
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00003,00004,00013,00016,00039,00043,00044,00051,00052,00055,00056,00057,00058")]
        (string complaintCode, ComplaintManagementDto cmpltMngDto, ClaimsPrincipal user, IEmailService emailService, GsdsDb db) =>
            ComplaintManagementController.ForwardingComplaint(complaintCode, cmpltMngDto, user, emailService, db)).WithTags("ComplaintManagement");

        appRoutes.MapGet("/complaint-management/forwarded",
           [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00003,00004,00013,00016,00039,00043,00044,00051,00052,00055,00056,00057,00058")]
        (GsdsDb db) =>
           ComplaintManagementController.GetAllForwarded(db)).WithTags("ComplaintManagement");

        appRoutes.MapGet("/complaint-management/forwarded/{complaintCode}",
           [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00003,00004,00013,00016,00039,00043,00044,00051,00052,00055,00056,00057,00058")]
        (string complaintCode, GsdsDb db) =>
           ComplaintManagementController.GetForwardedByComplaintCode(complaintCode, db)).WithTags("ComplaintManagement");

        // -------------- FOR COMPLAINT ADDITIONAL DATA
        appRoutes.MapPost("/complaint-additional-data",
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00003,00004,00013,00016,00039,00043,00044,00051,00052,00055,00056,00057,00058")]
        (ComplaintAdditionalDataDto additionaldataDto, GsdsDb db) =>
           ComplaintAdditionalDataController.CreateComplaintAdditionalData(additionaldataDto, db)).WithTags("ComplaintAdditionalData");

        appRoutes.MapGet("/complaint-additional-data/{complaintCode}",
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00003,00004,00013,00016,00039,00043,00044,00051,00052,00055,00056,00057,00058")]
        (string complaintCode, GsdsDb db) =>
           ComplaintAdditionalDataController.GetComplaintAdditionalData(complaintCode, db)).WithTags("ComplaintAdditionalData");

        // -------------- FOR COMPLAINT ADDITIONAL DATA REPLIES
        appRoutes.MapPost("/complaint-additional-data/reply",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001, 00043,00052,00012")]
        (ComplaintAdditionalInfoReplyDto additionaldataReplyDto, GsdsDb db) =>
           ComplaintAdditionalInfoReplyController.CreateComplaintAdditionalInfoReply(additionaldataReplyDto, db)).WithTags("ComplaintAdditionalDataReply");

        appRoutes.MapGet("/complaint-additional-data/reply/{complaintCode}/complaints-by-cmpltcode",
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00003,00004,00013,00016,00039,00043,00044,00051,00052,00055,00056,00057,00058")]
        (string complaintCode, GsdsDb db) =>
           ComplaintAdditionalInfoReplyController.GetComplaintAdditionalDataReplyByComplaintCode(complaintCode, db)).WithTags("ComplaintAdditionalDataReply");

        appRoutes.MapGet("/complaint-additional-data/reply/{refCode}/complaints-by-refcode",
       [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00003,00004,00013,00016,00039,00043,00044,00051,00052,00055,00056,00057,00058")]
        (Double refCode, GsdsDb db) =>
          ComplaintAdditionalInfoReplyController.GetComplaintAdditionalDataReplyByRefCode(refCode, db)).WithTags("ComplaintAdditionalDataReply");

        // ------------- COMPLAINT CLOSE
        appRoutes.MapPost("/complaint-close",
       [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00003,00004,00013,00016,00039,00043,00044,00051,00052,00055,00056,00057,00058")]
        (ComplaintCloseDto complaintCloseDto, GsdsDb db) =>
          ComplaintCloseController.CreateComplaintClose(complaintCloseDto, db)).WithTags("ComplaintClose");

        appRoutes.MapGet("/complaint-close/{complaintCode}",
       [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00003,00004,00013,00016,00039,00043,00044,00051,00052,00055,00056,00057,00058")]
        (string complaintCode, GsdsDb db) =>
          ComplaintCloseController.GetComplaintClose(complaintCode, db)).WithTags("ComplaintClose");

        // ------------- For files
        appRoutes.MapPost("/file/upload/{complaintCode}",
          [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00003,00004,00013,00016,00039,00043,00044,00051,00052,00055,00056,00057,00058")]
        (IFormFileCollection files, string complaintCode, ClaimsPrincipal user, IWebHostEnvironment hostEnvironment, GsdsDb db) =>
          FileHandlingController.UploadFiles(files, complaintCode, user, hostEnvironment, db)
        ).WithTags("File");

        appRoutes.MapGet("/files",
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00003,00004,00013,00016,00039,00043,00044,00051,00052,00055,00056,00057,00058")]
        (GsdsDb db) => FileHandlingController.getFiles(db)).WithTags("File");

        appRoutes.MapGet("/files/download/{fileName}",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00043,00052,00012")]
        (string fileName, HttpContext context) => new FileHandlingController().GetTheFile(context)).WithTags("File");

        // ---------- For country
        appRoutes.MapGet("/country",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00043")]
            (GsdsDb db) => CountryController.getAllCountries(db)
            ).WithTags("Country");

        appRoutes.MapGet("/country/{countryId}",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00043")]
            (string countryId, GsdsDb db) => CountryController.getSingleCountry(countryId, db)).WithTags("Country");

        appRoutes.MapGet("/country/countryName/{countryName}",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00043")]
            (string countryName, GsdsDb db) => CountryController.getCountryByName(countryName, db)).WithTags("Country");


        // ---------- For Provinces
        appRoutes.MapGet("/province",
       //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00043")]
       (GsdsDb db) => ProvinceController.getProvinces(db)
           ).WithTags("Province");

        appRoutes.MapGet("/province/{provinceId}",
      //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00043")]
      (string provinceId, GsdsDb db) => ProvinceController.getProvinceById(provinceId, db)
          ).WithTags("Province");

        appRoutes.MapGet("/province/{provinceId}/districts",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00043")]
        (string provinceId, GsdsDb db) => ProvinceController.getDistrictsByProvince(provinceId, db)
            ).WithTags("Province");

        appRoutes.MapGet("/province/{provinceId}/complaints",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00043")]
        (string provinceId, GsdsDb db) => ProvinceController.getComplaintsByProvince(provinceId, db)
            ).WithTags("Province");

        appRoutes.MapGet("/province/{provinceId}/complainers",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00043")]
        (string provinceId, GsdsDb db) => ProvinceController.getComplainersByProvince(provinceId, db)
            ).WithTags("Province");

        appRoutes.MapGet("/province/{provinceId}/accuseds",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00043")]
        (string provinceId, GsdsDb db) => ProvinceController.getAccusedByProvince(provinceId, db)
            ).WithTags("Province");


        // ---------- For districts
        appRoutes.MapGet("/district/{districtId}",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00043")]
        (string districtId, GsdsDb db) => DistrictController.getDistrictById(districtId, db)
            ).WithTags("District");

        appRoutes.MapGet("/district/{districtId}/sectors",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00043")]
        (string districtId, GsdsDb db) => DistrictController.getSectorByDistrict(districtId, db)
            ).WithTags("District");

        appRoutes.MapGet("/district/{districtId}/complaints",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00043")]
        (string districtId, GsdsDb db) => DistrictController.getComplaintsByDistrict(districtId, db)
            ).WithTags("District");

        appRoutes.MapGet("/district/{districtId}/complainers",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00043")]
        (string districtId, GsdsDb db) => DistrictController.getComplainersByDistrict(districtId, db)
            ).WithTags("District");

        appRoutes.MapGet("/district/{districtId}/accuseds",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00043")]
        (string districtId, GsdsDb db) => DistrictController.getAccusedByDistrict(districtId, db)
            ).WithTags("District");

        // ---------- For sectors
        appRoutes.MapGet("/sector/{sectorId}",
       //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00043")]
       (string sectorId, GsdsDb db) => SectorController.getSectorById(sectorId, db)
           ).WithTags("Sector");

        appRoutes.MapGet("/sector/{sectorId}/cells",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00043")]
        (string sectorId, GsdsDb db) => SectorController.getCellBySector(sectorId, db)
            ).WithTags("Sector");

        appRoutes.MapGet("/sector/{sectorId}/complaints",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00043")]
        (string sectorId, GsdsDb db) => SectorController.getComplaintsBySector(sectorId, db)
            ).WithTags("Sector");

        appRoutes.MapGet("/sector/{sectorId}/complainers",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00043")]
        (string sectorId, GsdsDb db) => SectorController.getComplainersBySector(sectorId, db)
            ).WithTags("Sector");

        appRoutes.MapGet("/sector/{sectorId}/accuseds",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00043")]
        (string sectorId, GsdsDb db) => SectorController.getAccusedBySector(sectorId, db)
            ).WithTags("Sector");

        // ---------- For Cellls
        appRoutes.MapGet("/cell/{cellId}",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00043")]
        (string cellId, GsdsDb db) => CellController.getCellById(cellId, db)
            ).WithTags("Cell");

        appRoutes.MapGet("/cell/{cellId}/complaints",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00043")]
        (string cellId, GsdsDb db) => CellController.getComplaintsByCell(cellId, db)
            ).WithTags("Cell");

        appRoutes.MapGet("/cell/{cellId}/complainers",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00043")]
        (string cellId, GsdsDb db) => CellController.getComplainersByCell(cellId, db)
            ).WithTags("Cell");

        appRoutes.MapGet("/cell/{cellId}/accuseds",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00043")]
        (string cellId, GsdsDb db) => CellController.getAccusedByCell(cellId, db)
            ).WithTags("Cell");

        appRoutes.MapGet("/person-type",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00043")]
        (GsdsDb db) => PersonTypeController.getPersonTypes(db)
            ).WithTags("PersonType");

        appRoutes.MapGet("/person-type/{personTypeId}",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00043")]
        (string personTypeId, GsdsDb db) => PersonTypeController.getPersonTypeById(personTypeId, db)
            ).WithTags("PersonType");


        // ------------- For identifier type

        appRoutes.MapGet("/identifier-type",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00043")]
        (GsdsDb db) => IdentifierTypeController.getAllIdentifierTypes(db)
            ).WithTags("IdentifierType");

        appRoutes.MapGet("/identifier-type/{id}",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00043")]
        (string id, GsdsDb db) => IdentifierTypeController.getIdentifierTypeById(id, db)
            ).WithTags("IdentifierType");

        // ------------ For roles
        appRoutes.MapGet("/roles",
           [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00003,00004,00013,00016,00039,00043,00044,00051,00052,00055,00056,00057,00058")]
        (GsdsDb db) => ManagerRolesController.GetAllRoles(db)
           ).WithTags("ManagerRoles");

        appRoutes.MapGet("/roles/{roleId}",
           [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00003,00004,00013,00016,00039,00043,00044,00051,00052,00055,00056,00057,00058")]
        (string roleId, GsdsDb db) => ManagerRolesController.GetRoleById(roleId, db)
           ).WithTags("ManagerRoles");

        appRoutes.MapGet("/roles/{roleId}/complaints",
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00003,00004,00013,00016,00039,00043,00044,00051,00052,00055,00056,00057,00058")]
        (string roleId, GsdsDb db) => ManagerRolesController.GetComplaintsByRole(roleId, db)
            ).WithTags("ManagerRoles");

        appRoutes.MapGet("/roles/{roleId}/members",
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00003,00004,00013,00016,00039,00043,00044,00051,00052,00055,00056,00057,00058")]
        (string roleId, GsdsDb db) => ManagerRolesController.GetRoleMembers(roleId, db)
            ).WithTags("ManagerRoles");

        appRoutes.MapGet("/roles/{roleId}/complainers",
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00003,00004,00013,00016,00039,00043,00044,00051,00052,00055,00056,00057,00058")]
        (string roleId, GsdsDb db) => ManagerRolesController.GetComplainersByRole(roleId, db)
            ).WithTags("ManagerRoles");

        // ------------- For complaint category controller

        appRoutes.MapGet("/complaint-category",
       [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        (GsdsDb db) => ComplaintCategoryController.getComplaintCategories(db)
           ).WithTags("ComplaintController");

        // ---------- For Institutions
        appRoutes.MapGet("/institution",
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00003,00004,00013,00016,00039,00043,00044,00051,00052,00055,00056,00057,00058")]
        (GsdsDb db) => InstitutionController.GetInstitutions(db)
            ).WithTags("Institution");

        appRoutes.MapGet("/institution/{institutionId}",
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00001,00003,00004,00013,00016,00039,00043,00044,00051,00052,00055,00056,00057,00058")]
        (Double institutionId, GsdsDb db) => InstitutionController.GetInstitutionById(institutionId, db)
            ).WithTags("Institution");

        appRoutes.MapPost("/email", (EmailDto request, IEmailService emailService) =>
        {
            EmailController.sendEmail(request, emailService);
        }).WithTags("Email");


        // --------- OTHER HELPERS
        appRoutes.MapGet("/marital-status",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00043")]
        (GsdsDb db) => MaritalStatusController.getallMaritalStatus(db)
            ).WithTags("MaritalStatus");

        appRoutes.MapGet("/marital-status/{id}",
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "00043")]
            (string id, GsdsDb db) => MaritalStatusController.getMaritalStatusById(id, db)).WithTags("MaritalStatus");

        // ----------- For Gender
        appRoutes.MapGet("/gender",
            (GsdsDb db) => GenderController.getGenders(db)
        ).WithTags("Gender");

        appRoutes.MapGet("/gender/{genderId}",
           (string genderId, GsdsDb db) => GenderController.getGenderById(genderId, db)
       ).WithTags("Gender");

        // provide swagger ui
        app.UseSwaggerUI();
        app.Run();
    }
}