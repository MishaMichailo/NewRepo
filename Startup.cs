using Microsoft.AspNetCore.Builder;

namespace TestTask
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Configure error handling for production
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );

                // Additional route configurations for your controller
                endpoints.MapControllerRoute(
                    name: "fileUpload",
                    pattern: "file-upload/{action=UploadFile}",
                    defaults: new { controller = "FileUpload" }
                );
            });
        }
    }
}
