namespace Escola.Application.Game.Badges;

public record CreateBadgeRequest(CreateBadgeDto Badge);
public record CreateBadgeResponse(BadgeDto Badge);
public interface ICreateBadgeHandler
{
    Task<CreateBadgeResponse> Handle(CreateBadgeRequest request, CancellationToken cancellationToken);
}

public record GetBadgeRequest(Guid BadgeUuid);
public record GetBadgeResponse(BadgeDto Badge);
public interface IGetBadgeHandler
{
    Task<GetBadgeResponse> Handle(GetBadgeRequest request, CancellationToken cancellationToken);
}

public record GetBadgesRequest;
public record GetBadgesResponse(List<BadgeDto> Badges);
public interface IGetBadgesHandler
{
    Task<GetBadgesResponse> Handle(GetBadgesRequest request, CancellationToken cancellationToken);
}

public record UpdateBadgeRequest(Guid BadgeUuid, UpdateBadgeDto Badge);
public record UpdateBadgeResponse(BadgeDto Badge);
public interface IUpdateBadgeHandler
{
    Task<UpdateBadgeResponse> Handle(UpdateBadgeRequest request, CancellationToken cancellationToken);
}
