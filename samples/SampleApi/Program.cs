using MCIO.OutputEnvelop.Samples.SampleApi.Application.Queries.GetAllCustomers;
using MCIO.OutputEnvelop.Samples.SampleApi.Application.Queries.GetCustomerByEmail;
using MCIO.OutputEnvelop.Samples.SampleApi.Application.UseCases.RegisterNewCustomer;
using MCIO.OutputEnvelop.Samples.SampleApi.Domain.Repositoes;
using MCIO.OutputEnvelop.Samples.SampleApi.Domain.Services.Customers;
using MCIO.OutputEnvelop.Samples.SampleApi.Infra.CrossCutting;
using MCIO.OutputEnvelop.Samples.SampleApi.Infra.Data.Repositories;
using MCIO.OutputEnvelop.Samples.SampleApi.Infra.Data.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerDomainService, CustomerDomainService>();

builder.Services.AddScoped<IRegisterNewCustomerUseCase, RegisterNewCustomerUseCase>();

builder.Services.AddScoped<IGetAllCustomersQuery, GetAllCustomersQuery>();
builder.Services.AddScoped<IGetCustomerByEmailQuery, GetCustomerByEmailQuery>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEventPublisher, EventPublisher>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
