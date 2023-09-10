using System.Text;
using System.Text.Json.Serialization;
using Api.DAL;
using Api.DAL.DBinitialization;
using Api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Bearer token",
    };

    c.AddSecurityDefinition("oauth2", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                }
            },
            new List<string>()
        },
    };

    c.AddSecurityRequirement(securityRequirement);
});

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseLazyLoadingProxies()
        .UseNpgsql(builder.Configuration.GetConnectionString("DatabaseContext") ?? throw new InvalidOperationException("Connection string 'DatabaseContext' not found.")));

builder.Services.AddDbContextFactory<DatabaseContext>(options =>
    options.UseLazyLoadingProxies()
        .UseNpgsql(builder.Configuration.GetConnectionString("DatabaseContext") ??
                   throw new InvalidOperationException("Connection string 'DatabaseContext' not found.")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration.GetSection("Token").Value!)),
        ValidateLifetime = true
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "defaultPolicy",
        policy  =>
        {
            policy.AllowAnyOrigin();
            policy.AllowAnyMethod();
            policy.AllowAnyHeader();
        });
});

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();
    q.SchedulerId = "Scheduler-Core";
    q.UseDefaultThreadPool(tp =>
    {
        tp.MaxConcurrency = 2;
    });
    
    var jobKey = new JobKey("PollReadyEventJob");
    q.AddJob<PollReadyEventJob>(opts => opts.WithIdentity(jobKey));
    
    q.AddTrigger(opts => opts
            .ForJob(jobKey)
            .WithIdentity("PollReadyEventJob-trigger")
            .WithCronSchedule("0/10 * * * * ?")

    );
});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

/*builder.Services.AddScoped<ItemSpawnHandler>();
builder.Services.AddScoped<MissionEndedHandler>();*/
builder.Services.AddSingleton<EventHandlerProvider>();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<DatabaseContext>();
    context.Database.EnsureDeleted();
    context.Database.EnsureCreated(); // TODO add migrations
    DatabaseInitializer.Initialize(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("defaultPolicy");
app.MapControllers();

app.Run();
