
namespace ProductApp.API.Extensions
{
    public static class WebAppExtensions
    {
        public static void ConfigureApp(this WebApplication app)
        {

            app.UseCors("AllowAll");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API v1"));
            }
            
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains; preload");
                context.Response.Headers.Add("Cache-Control", "no-store, no-cache, must-revalidate");
                await next();
            });

            app.UseHttpsRedirection();
            
            app.UseRouting(); 
            app.UseXContentTypeOptions();//Setting X-Content-Type-Options header
            app.UseReferrerPolicy(opts => opts.NoReferrer()); //Referrer-Policy header
            app.UseXXssProtection(options => options.EnabledWithBlockMode());//X-XSS-Proetection header
            app.UseXfo(options => options.Deny()); //X-Frame-Options head
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }    
}