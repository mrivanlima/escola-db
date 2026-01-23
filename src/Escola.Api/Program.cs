using Escola.Application.Services;
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
using Escola.Application.UseCases.Teachers.GetTeacher;
using Escola.Application.UseCases.Teachers.GetTeachers;
using Escola.Application.UseCases.Teachers.UpdateTeacher;
using Escola.Application.UseCases.Tenants.CreateTenant;
using Escola.Application.UseCases.Tenants.GetTenant;
using Escola.Application.UseCases.Tenants.GetTenants;
using Escola.Application.UseCases.Tenants.UpdateTenant;
using Escola.Application.UseCases.TenantTypes.GetTenantType;
using Escola.Application.UseCases.TenantTypes.GetTenantTypes;
using Escola.Application.UseCases.UserRoles.GetUserRole;
using Escola.Application.UseCases.UserRoles.GetUserRoles;
using Escola.Application.UseCases.AppUsers.CreateAppUser;
using Escola.Application.UseCases.AppUsers.GetAppUser;
using Escola.Application.UseCases.AppUsers.GetAppUsers;
using Escola.Application.UseCases.AppUsers.UpdateAppUser;
using Escola.Application.UseCases.GradeLevels.GetGradeLevel;
using Escola.Application.UseCases.GradeLevels.GetGradeLevels;
using Escola.Application.UseCases.SchoolYears.CreateSchoolYear;
using Escola.Application.UseCases.SchoolYears.GetSchoolYear;
using Escola.Application.UseCases.SchoolYears.GetSchoolYears;
using Escola.Application.UseCases.SchoolYears.UpdateSchoolYear;
using Escola.Application.UseCases.EnrollmentStatuses.GetEnrollmentStatus;
using Escola.Application.UseCases.EnrollmentStatuses.GetEnrollmentStatuses;
using Escola.Application.UseCases.RelationshipTypes.GetRelationshipType;
using Escola.Application.UseCases.RelationshipTypes.GetRelationshipTypes;
using Escola.Application.UseCases.Guardians.CreateGuardian;
using Escola.Application.UseCases.Guardians.GetGuardian;
using Escola.Application.UseCases.Guardians.GetGuardians;
using Escola.Application.UseCases.Guardians.UpdateGuardian;
using Escola.Application.UseCases.StudentGuardians.CreateStudentGuardian;
using Escola.Application.UseCases.StudentGuardians.GetStudentGuardian;
using Escola.Application.UseCases.StudentGuardians.GetStudentGuardians;
using Escola.Application.UseCases.StudentGuardians.UpdateStudentGuardian;
using Escola.Application.Validators.School;
using Escola.Infrastructure.Persistence;
using Escola.Infrastructure.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure DbContext with PostgreSQL
builder.Services.AddDbContext<EscolaDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register current user service for tenant isolation
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

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
builder.Services.AddScoped<IGetTeachersHandler, Escola.Infrastructure.UseCases.Teachers.GetTeachersHandler>();
builder.Services.AddScoped<IGetTeacherHandler, Escola.Infrastructure.UseCases.Teachers.GetTeacherHandler>();
builder.Services.AddScoped<IUpdateTeacherHandler, Escola.Infrastructure.UseCases.Teachers.UpdateTeacherHandler>();

// Register Specialization handlers
builder.Services.AddScoped<Escola.Application.UseCases.Specializations.GetSpecialization.IGetSpecializationHandler, Escola.Infrastructure.UseCases.Specializations.GetSpecializationHandler>();
builder.Services.AddScoped<Escola.Application.UseCases.Specializations.GetSpecializations.IGetSpecializationsHandler, Escola.Infrastructure.UseCases.Specializations.GetSpecializationsHandler>();

// Register Class handlers
builder.Services.AddScoped<ICreateClassHandler, Escola.Infrastructure.UseCases.Classes.CreateClassHandler>();
builder.Services.AddScoped<IGetClassesHandler, Escola.Infrastructure.UseCases.Classes.GetClassesHandler>();
builder.Services.AddScoped<IGetClassHandler, Escola.Infrastructure.UseCases.Classes.GetClassHandler>();
builder.Services.AddScoped<IUpdateClassHandler, Escola.Infrastructure.UseCases.Classes.UpdateClassHandler>();

// Register ClassStudent handlers
builder.Services.AddScoped<Escola.Application.UseCases.ClassStudents.CreateClassStudent.ICreateClassStudentHandler, Escola.Infrastructure.UseCases.ClassStudents.CreateClassStudentHandler>();
builder.Services.AddScoped<Escola.Application.UseCases.ClassStudents.GetClassStudent.IGetClassStudentHandler, Escola.Infrastructure.UseCases.ClassStudents.GetClassStudentHandler>();
builder.Services.AddScoped<Escola.Application.UseCases.ClassStudents.GetClassStudents.IGetClassStudentsHandler, Escola.Infrastructure.UseCases.ClassStudents.GetClassStudentsHandler>();
builder.Services.AddScoped<Escola.Application.UseCases.ClassStudents.UpdateClassStudent.IUpdateClassStudentHandler, Escola.Infrastructure.UseCases.ClassStudents.UpdateClassStudentHandler>();

// Register Subject handlers
builder.Services.AddScoped<Escola.Application.UseCases.Subjects.CreateSubject.ICreateSubjectHandler, Escola.Infrastructure.UseCases.Subjects.CreateSubjectHandler>();
builder.Services.AddScoped<Escola.Application.UseCases.Subjects.GetSubject.IGetSubjectHandler, Escola.Infrastructure.UseCases.Subjects.GetSubjectHandler>();
builder.Services.AddScoped<Escola.Application.UseCases.Subjects.GetSubjects.IGetSubjectsHandler, Escola.Infrastructure.UseCases.Subjects.GetSubjectsHandler>();
builder.Services.AddScoped<Escola.Application.UseCases.Subjects.UpdateSubject.IUpdateSubjectHandler, Escola.Infrastructure.UseCases.Subjects.UpdateSubjectHandler>();

// Register ProficiencyLevel handlers
builder.Services.AddScoped<Escola.Application.UseCases.ProficiencyLevels.GetProficiencyLevel.IGetProficiencyLevelHandler, Escola.Infrastructure.UseCases.ProficiencyLevels.GetProficiencyLevelHandler>();
builder.Services.AddScoped<Escola.Application.UseCases.ProficiencyLevels.GetProficiencyLevels.IGetProficiencyLevelsHandler, Escola.Infrastructure.UseCases.ProficiencyLevels.GetProficiencyLevelsHandler>();

// Register Certification handlers
builder.Services.AddScoped<Escola.Application.UseCases.Certifications.GetCertification.IGetCertificationHandler, Escola.Infrastructure.UseCases.Certifications.GetCertificationHandler>();
builder.Services.AddScoped<Escola.Application.UseCases.Certifications.GetCertifications.IGetCertificationsHandler, Escola.Infrastructure.UseCases.Certifications.GetCertificationsHandler>();

// Register TeacherSubject handlers
builder.Services.AddScoped<Escola.Application.UseCases.TeacherSubjects.CreateTeacherSubject.ICreateTeacherSubjectHandler, Escola.Infrastructure.UseCases.TeacherSubjects.CreateTeacherSubjectHandler>();
builder.Services.AddScoped<Escola.Application.UseCases.TeacherSubjects.GetTeacherSubject.IGetTeacherSubjectHandler, Escola.Infrastructure.UseCases.TeacherSubjects.GetTeacherSubjectHandler>();
builder.Services.AddScoped<Escola.Application.UseCases.TeacherSubjects.GetTeacherSubjects.IGetTeacherSubjectsHandler, Escola.Infrastructure.UseCases.TeacherSubjects.GetTeacherSubjectsHandler>();
builder.Services.AddScoped<Escola.Application.UseCases.TeacherSubjects.UpdateTeacherSubject.IUpdateTeacherSubjectHandler, Escola.Infrastructure.UseCases.TeacherSubjects.UpdateTeacherSubjectHandler>();

// Register TeacherCertification handlers
builder.Services.AddScoped<Escola.Application.UseCases.TeacherCertifications.CreateTeacherCertification.ICreateTeacherCertificationHandler, Escola.Infrastructure.UseCases.TeacherCertifications.CreateTeacherCertificationHandler>();
builder.Services.AddScoped<Escola.Application.UseCases.TeacherCertifications.GetTeacherCertification.IGetTeacherCertificationHandler, Escola.Infrastructure.UseCases.TeacherCertifications.GetTeacherCertificationHandler>();
builder.Services.AddScoped<Escola.Application.UseCases.TeacherCertifications.GetTeacherCertifications.IGetTeacherCertificationsHandler, Escola.Infrastructure.UseCases.TeacherCertifications.GetTeacherCertificationsHandler>();
builder.Services.AddScoped<Escola.Application.UseCases.TeacherCertifications.UpdateTeacherCertification.IUpdateTeacherCertificationHandler, Escola.Infrastructure.UseCases.TeacherCertifications.UpdateTeacherCertificationHandler>();

// Register Tenant handlers
builder.Services.AddScoped<IGetTenantsHandler, Escola.Infrastructure.UseCases.Tenants.GetTenantsHandler>();
builder.Services.AddScoped<IGetTenantHandler, Escola.Infrastructure.UseCases.Tenants.GetTenantHandler>();
builder.Services.AddScoped<ICreateTenantHandler, Escola.Infrastructure.UseCases.Tenants.CreateTenantHandler>();
builder.Services.AddScoped<IUpdateTenantHandler, Escola.Infrastructure.UseCases.Tenants.UpdateTenantHandler>();

// Register TenantType handlers
builder.Services.AddScoped<IGetTenantTypesHandler, Escola.Infrastructure.UseCases.TenantTypes.GetTenantTypesHandler>();
builder.Services.AddScoped<IGetTenantTypeHandler, Escola.Infrastructure.UseCases.TenantTypes.GetTenantTypeHandler>();

// Register UserRole handlers
builder.Services.AddScoped<IGetUserRolesHandler, Escola.Infrastructure.UseCases.UserRoles.GetUserRolesHandler>();
builder.Services.AddScoped<IGetUserRoleHandler, Escola.Infrastructure.UseCases.UserRoles.GetUserRoleHandler>();

// Register AppUser handlers
builder.Services.AddScoped<IGetAppUsersHandler, Escola.Infrastructure.UseCases.AppUsers.GetAppUsersHandler>();
builder.Services.AddScoped<IGetAppUserHandler, Escola.Infrastructure.UseCases.AppUsers.GetAppUserHandler>();
builder.Services.AddScoped<ICreateAppUserHandler, Escola.Infrastructure.UseCases.AppUsers.CreateAppUserHandler>();
builder.Services.AddScoped<IUpdateAppUserHandler, Escola.Infrastructure.UseCases.AppUsers.UpdateAppUserHandler>();

// Register GradeLevel handlers
builder.Services.AddScoped<IGetGradeLevelsHandler, Escola.Infrastructure.UseCases.GradeLevels.GetGradeLevelsHandler>();
builder.Services.AddScoped<IGetGradeLevelHandler, Escola.Infrastructure.UseCases.GradeLevels.GetGradeLevelHandler>();

// Register SchoolYear handlers
builder.Services.AddScoped<ICreateSchoolYearHandler, Escola.Infrastructure.UseCases.SchoolYears.CreateSchoolYearHandler>();
builder.Services.AddScoped<IGetSchoolYearHandler, Escola.Infrastructure.UseCases.SchoolYears.GetSchoolYearHandler>();
builder.Services.AddScoped<IGetSchoolYearsHandler, Escola.Infrastructure.UseCases.SchoolYears.GetSchoolYearsHandler>();
builder.Services.AddScoped<IUpdateSchoolYearHandler, Escola.Infrastructure.UseCases.SchoolYears.UpdateSchoolYearHandler>();

// Register EnrollmentStatus handlers
builder.Services.AddScoped<IGetEnrollmentStatusHandler, Escola.Infrastructure.UseCases.EnrollmentStatuses.GetEnrollmentStatusHandler>();
builder.Services.AddScoped<IGetEnrollmentStatusesHandler, Escola.Infrastructure.UseCases.EnrollmentStatuses.GetEnrollmentStatusesHandler>();

// Register RelationshipType handlers
builder.Services.AddScoped<IGetRelationshipTypeHandler, Escola.Infrastructure.UseCases.RelationshipTypes.GetRelationshipTypeHandler>();
builder.Services.AddScoped<IGetRelationshipTypesHandler, Escola.Infrastructure.UseCases.RelationshipTypes.GetRelationshipTypesHandler>();

// Register Guardian handlers
builder.Services.AddScoped<ICreateGuardianHandler, Escola.Infrastructure.UseCases.Guardians.CreateGuardianHandler>();
builder.Services.AddScoped<IGetGuardianHandler, Escola.Infrastructure.UseCases.Guardians.GetGuardianHandler>();
builder.Services.AddScoped<IGetGuardiansHandler, Escola.Infrastructure.UseCases.Guardians.GetGuardiansHandler>();
builder.Services.AddScoped<IUpdateGuardianHandler, Escola.Infrastructure.UseCases.Guardians.UpdateGuardianHandler>();

// Register StudentGuardian handlers
builder.Services.AddScoped<ICreateStudentGuardianHandler, Escola.Infrastructure.UseCases.StudentGuardians.CreateStudentGuardianHandler>();
builder.Services.AddScoped<IGetStudentGuardianHandler, Escola.Infrastructure.UseCases.StudentGuardians.GetStudentGuardianHandler>();
builder.Services.AddScoped<IGetStudentGuardiansHandler, Escola.Infrastructure.UseCases.StudentGuardians.GetStudentGuardiansHandler>();
builder.Services.AddScoped<IUpdateStudentGuardianHandler, Escola.Infrastructure.UseCases.StudentGuardians.UpdateStudentGuardianHandler>();

// Register Health handlers
builder.Services.AddScoped<IGetHealthHandler, Escola.Infrastructure.UseCases.Health.GetHealthHandler>();
builder.Services.AddScoped<ICheckDatabaseHandler, Escola.Infrastructure.UseCases.Health.CheckDatabaseHandler>();

// Register validators
builder.Services.AddScoped<CreateClassValidator>();
builder.Services.AddScoped<UpdateClassValidator>();
builder.Services.AddScoped<CreateClassStudentValidator>();
builder.Services.AddScoped<UpdateClassStudentValidator>();
builder.Services.AddScoped<CreateSubjectValidator>();
builder.Services.AddScoped<UpdateSubjectValidator>();
builder.Services.AddScoped<CreateTeacherSubjectValidator>();
builder.Services.AddScoped<UpdateTeacherSubjectValidator>();
builder.Services.AddScoped<CreateTeacherCertificationValidator>();
builder.Services.AddScoped<UpdateTeacherCertificationValidator>();
builder.Services.AddScoped<CreateTeacherValidator>();
builder.Services.AddScoped<UpdateTeacherValidator>();
builder.Services.AddScoped<CreateGuardianValidator>();
builder.Services.AddScoped<UpdateGuardianValidator>();
builder.Services.AddScoped<CreateStudentGuardianValidator>();
builder.Services.AddScoped<UpdateStudentGuardianValidator>();
builder.Services.AddScoped<CreateSchoolYearValidator>();
builder.Services.AddScoped<UpdateSchoolYearValidator>();

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
