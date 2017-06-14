using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApplication
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<SecondContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<SchemaContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseDatabaseErrorPage();

            app.MapWhen(ctx => ctx.Request.Path.StartsWithSegments("/delete"),
                builder => builder.Run(async (context) =>
                {
                    var ctx = context.RequestServices.GetRequiredService<AppContext>();
                    await ctx.Database.EnsureDeletedAsync();
                    context.Response.Redirect($"http://{context.Request.Host}");
                }));

            app.Run(async (context) =>
            {
                var ctx = context.RequestServices.GetRequiredService<AppContext>();
                var customers = await ctx.Customers.ToListAsync();

                var secondContext = context.RequestServices.GetRequiredService<SecondContext>();
                var bloggers = await secondContext.Bloggers.ToListAsync();

                var schemaContext = context.RequestServices.GetRequiredService<SchemaContext>();
                var bloggersFromSchema = await schemaContext.Bloggers.ToListAsync();

                var id = customers.FirstOrDefault() != null ? customers.FirstOrDefault().Id.ToString() : "stranger";

                await context.Response.WriteAsync($"Hello World {id}.");
            });
        }
    }
}
