using Escola.Application.UseCases.Classes.CreateClass;
using Escola.Application.UseCases.Classes.DeleteClass;
using Escola.Application.UseCases.Classes.GetClass;
using Escola.Application.UseCases.Classes.GetClasses;
using Escola.Application.UseCases.Classes.UpdateClass;
using Escola.Application.UseCases.Health.CheckDatabase;
using Escola.Application.UseCases.Health.GetHealth;
using Escola.Application.UseCases.Students.CreateStudent;
using Escola.Application.UseCases.Students.DeleteStudent;
using Escola.Application.UseCases.Students.GetStudent;
using Escola.Application.UseCases.Students.GetStudents;
using Escola.Application.UseCases.Students.UpdateStudent;
using Escola.Application.UseCases.Teachers.CreateTeacher;
using Escola.Application.UseCases.Teachers.DeleteTeacher;
using Escola.Application.UseCases.Teachers.GetTeacher;
using Escola.Application.UseCases.Teachers.GetTeachers;
using Escola.Application.UseCases.Teachers.UpdateTeacher;
using Escola.Application.UseCases.Tenants.GetTenants;
using Escola.Infrastructure.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure DbContext with PostgreSQL
builder.Services.AddDbContext<EscolaDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register FluentValidation validators
builder.Services.AddValidatorsFromAssemblyContaining<CreateStudentValidator>();

// Register Student handlers
builder.Services.AddScoped<ICreateStudentHandler, Escola.Infrastructure.UseCases.Students.CreateStudentHandler>();
builder.Services.AddScoped<IDeleteStudentHandler, Escola.Infrastructure.UseCases.Students.DeleteStudentHandler>();
builder.Services.AddScoped<IGetStudentsHandler, Escola.Infrastructure.UseCases.Students.GetStudentsHandler>();
builder.Services.AddScoped<IGetStudentHandler, Escola.Infrastructure.UseCases.Students.GetStudentHandler>();
builder.Services.AddScoped<IUpdateStudentHandler, Escola.Infrastructure.UseCases.Students.UpdateStudentHandler>();

// Register Teacher handlers
builder.Services.AddScoped<ICreateTeacherHandler, Escola.Infrastructure.UseCases.Teachers.CreateTeacherHandler>();
builder.Services.AddScoped<IDeleteTeacherHandler, Escola.Infrastructure.UseCases.Teachers.DeleteTeacherHandler>();
builder.Services.AddScoped<IGetTeachersHandler, Escola.Infrastructure.UseCases.Teachers.GetTeachersHandler>();
builder.Services.AddScoped<IGetTeacherHandler, Escola.Infrastructure.UseCases.Teachers.GetTeacherHandler>();
builder.Services.AddScoped<IUpdateTeacherHandler, Escola.Infrastructure.UseCases.Teachers.UpdateTeacherHandler>();

// Register Class handlers
builder.Services.AddScoped<ICreateClassHandler, Escola.Infrastructure.UseCases.Classes.CreateClassHandler>();
builder.Services.AddScoped<IDeleteClassHandler, Escola.Infrastructure.UseCases.Classes.DeleteClassHandler>();
builder.Services.AddScoped<IGetClassesHandler, Escola.Infrastructure.UseCases.Classes.GetClassesHandler>();
builder.Services.AddScoped<IGetClassHandler, Escola.Infrastructure.UseCases.Classes.GetClassHandler>();
builder.Services.AddScoped<IUpdateClassHandler, Escola.Infrastructure.UseCases.Classes.UpdateClassHandler>();

// Register Tenant handlers
builder.Services.AddScoped<IGetTenantsHandler, Escola.Infrastructure.UseCases.Tenants.GetTenantsHandler>();

// Register Health handlers
builder.Services.AddScoped<IGetHealthHandler, Escola.Infrastructure.UseCases.Health.GetHealthHandler>();
builder.Services.AddScoped<ICheckDatabaseHandler, Escola.Infrastructure.UseCases.Health.CheckDatabaseHandler>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
