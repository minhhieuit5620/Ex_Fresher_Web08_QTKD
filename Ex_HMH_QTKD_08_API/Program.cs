
using Misa.AMIS.BL;
using Misa.AMIS.BL.BaseBL;
using Misa.AMIS.BL.DepartmentBL;
using Misa.AMIS.DL;
using Misa.AMIS.DL.BaseDL;
using Misa.AMIS.DL.DeparmentDL;
using NLog;
using NLog.Web;
using System.Transactions;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");
try
{
    var builder = WebApplication.CreateBuilder(args);

    var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

    // Add services to the container.

    builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

    //add CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(MyAllowSpecificOrigins,
                              policy =>
                              {
                                  policy.AllowAnyOrigin()
                                  .AllowAnyHeader()
                                  .AllowAnyMethod();
                              });
    });

    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Dependency injection 
    //base
    builder.Services.AddScoped(typeof(IBaseDL<>), typeof(BaseDL<>));
    builder.Services.AddScoped(typeof(IBaseBL<>), typeof(BaseBL<>));
    //default
    builder.Services.AddScoped<IEmployeeBL, EmployeeBL>();
    builder.Services.AddScoped<IEmployeeDL, EmployeeDL>();

    builder.Services.AddScoped<IDepartmentBL, DepartmentBL>();
    builder.Services.AddScoped<IDeparmentDL, DepartmentDL>();

    //Connect DB
    DataContext.MySqlConnectionString = builder.Configuration.GetConnectionString("MySqlConnectionString");

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCors(MyAllowSpecificOrigins);

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}
