namespace Escola.Application.Content.Activities;

public record CreateActivityRequest(CreateActivityDto Activity);
public record CreateActivityResponse(ActivityDto Activity);
public interface ICreateActivityHandler
{
    Task<CreateActivityResponse> Handle(CreateActivityRequest request, CancellationToken cancellationToken);
}

public record GetActivityRequest(Guid ActivityUuid);
public record GetActivityResponse(ActivityDto Activity);
public interface IGetActivityHandler
{
    Task<GetActivityResponse> Handle(GetActivityRequest request, CancellationToken cancellationToken);
}

public record GetActivitiesRequest(Guid? ModuleUuid = null);
public record GetActivitiesResponse(List<ActivityDto> Activities);
public interface IGetActivitiesHandler
{
    Task<GetActivitiesResponse> Handle(GetActivitiesRequest request, CancellationToken cancellationToken);
}

public record UpdateActivityRequest(Guid ActivityUuid, UpdateActivityDto Activity);
public record UpdateActivityResponse(ActivityDto Activity);
public interface IUpdateActivityHandler
{
    Task<UpdateActivityResponse> Handle(UpdateActivityRequest request, CancellationToken cancellationToken);
}
