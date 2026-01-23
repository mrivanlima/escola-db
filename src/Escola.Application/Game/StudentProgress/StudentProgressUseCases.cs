namespace Escola.Application.Game.StudentProgress;

public record CreateStudentProgressRequest(CreateStudentProgressDto Progress);
public record CreateStudentProgressResponse(StudentProgressDto Progress);
public interface ICreateStudentProgressHandler
{
    Task<CreateStudentProgressResponse> Handle(CreateStudentProgressRequest request, CancellationToken cancellationToken);
}

public record GetStudentProgressRequest(Guid ProgressUuid);
public record GetStudentProgressResponse(StudentProgressDto Progress);
public interface IGetStudentProgressHandler
{
    Task<GetStudentProgressResponse> Handle(GetStudentProgressRequest request, CancellationToken cancellationToken);
}

public record GetStudentProgressListRequest(Guid? StudentUuid = null, Guid? ActivityUuid = null);
public record GetStudentProgressListResponse(List<StudentProgressDto> ProgressList);
public interface IGetStudentProgressListHandler
{
    Task<GetStudentProgressListResponse> Handle(GetStudentProgressListRequest request, CancellationToken cancellationToken);
}

public record UpdateStudentProgressRequest(Guid ProgressUuid, UpdateStudentProgressDto Progress);
public record UpdateStudentProgressResponse(StudentProgressDto Progress);
public interface IUpdateStudentProgressHandler
{
    Task<UpdateStudentProgressResponse> Handle(UpdateStudentProgressRequest request, CancellationToken cancellationToken);
}
