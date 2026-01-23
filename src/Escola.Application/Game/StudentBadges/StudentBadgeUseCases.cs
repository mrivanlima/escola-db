namespace Escola.Application.Game.StudentBadges;

public record AwardStudentBadgeRequest(AwardStudentBadgeDto Badge);
public record AwardStudentBadgeResponse(StudentBadgeDto Badge);
public interface IAwardStudentBadgeHandler
{
    Task<AwardStudentBadgeResponse> Handle(AwardStudentBadgeRequest request, CancellationToken cancellationToken);
}

public record GetStudentBadgesRequest(Guid? StudentUuid = null);
public record GetStudentBadgesResponse(List<StudentBadgeDto> Badges);
public interface IGetStudentBadgesHandler
{
    Task<GetStudentBadgesResponse> Handle(GetStudentBadgesRequest request, CancellationToken cancellationToken);
}
