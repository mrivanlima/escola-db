namespace Escola.Application.Content.Modules;

public record GetModulesRequest;

public record GetModulesResponse(List<ModuleDto> Modules);

public interface IGetModulesHandler
{
    Task<GetModulesResponse> Handle(GetModulesRequest request, CancellationToken cancellationToken);
}
