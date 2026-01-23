namespace Escola.Application.Content.Modules;

public record UpdateModuleRequest(Guid ModuleUuid, UpdateModuleDto Module);

public record UpdateModuleResponse(ModuleDto Module);

public interface IUpdateModuleHandler
{
    Task<UpdateModuleResponse> Handle(UpdateModuleRequest request, CancellationToken cancellationToken);
}
