namespace Escola.Application.Content.Modules;

public record GetModuleRequest(Guid ModuleUuid);

public record GetModuleResponse(ModuleDto Module);

public interface IGetModuleHandler
{
    Task<GetModuleResponse> Handle(GetModuleRequest request, CancellationToken cancellationToken);
}
