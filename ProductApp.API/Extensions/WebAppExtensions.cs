
namespace ProductApp.API.Extensions
{
    public static class WebAppExtensions
    {
        public static void ConfigureApp(this WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API v1"));
            }
            
            app.UseHttpsRedirection();
            
            app.UseRouting();
            
            app.UseCors("AllowAll");
            
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }    
}