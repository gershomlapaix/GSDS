using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Gsds{
    public class Startup{

        // constructor
        public Startup(IConfiguration configuration){
            Configuration = configuration;
        }

        // properties
        public IConfiguration Configuration{get;}

        // A method load the data from appsettings.Development.json to the MailSettings at runtime
        // dependency injection and IOptions is used to do so
        public void ConfigureServices(IServiceCollection services){
           try
            {
               services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options=> {
                options.TokenValidationParameters = new TokenValidationParameters{
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"]))
                };
               });

               services.AddMvc();
               services.AddControllers();
            }
           catch (Exception ex)
           {
            Console.WriteLine(ex);
           }
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env){
            if(env.IsDevelopment()){
                app.UseDeveloperExceptionPage();
            }
            else{
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


    }
}