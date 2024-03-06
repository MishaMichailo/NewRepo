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

            
        }
    }
}
