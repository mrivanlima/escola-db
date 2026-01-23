namespace Escola.Application.Content.ActivityResources;

public record CreateActivityResourceRequest(CreateActivityResourceDto Resource);
public record CreateActivityResourceResponse(ActivityResourceDto Resource);
public interface ICreateActivityResourceHandler
{
    Task<CreateActivityResourceResponse> Handle(CreateActivityResourceRequest request, CancellationToken cancellationToken);
}

public record GetActivityResourceRequest(Guid ResourceUuid);
public record GetActivityResourceResponse(ActivityResourceDto Resource);
public interface IGetActivityResourceHandler
{
    Task<GetActivityResourceResponse> Handle(GetActivityResourceRequest request, CancellationToken cancellationToken);
}

public record GetActivityResourcesRequest(Guid? ActivityUuid = null);
public record GetActivityResourcesResponse(List<ActivityResourceDto> Resources);
public interface IGetActivityResourcesHandler
{
    Task<GetActivityResourcesResponse> Handle(GetActivityResourcesRequest request, CancellationToken cancellationToken);
}

public record UpdateActivityResourceRequest(Guid ResourceUuid, UpdateActivityResourceDto Resource);
public record UpdateActivityResourceResponse(ActivityResourceDto Resource);
public interface IUpdateActivityResourceHandler
{
    Task<UpdateActivityResourceResponse> Handle(UpdateActivityResourceRequest request, CancellationToken cancellationToken);
}
