using CICD_Core;
using CICD_Core.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.Configure<Settings>(builder.Configuration.GetSection("CustomerDatabase"));

builder.Services.AddSingleton<ICustomerContext, CustomerContext>();
builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
