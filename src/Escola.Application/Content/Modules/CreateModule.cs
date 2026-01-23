namespace Escola.Application.Content.Modules;

public record CreateModuleRequest(CreateModuleDto Module);

public record CreateModuleResponse(ModuleDto Module);

public interface ICreateModuleHandler
{
    Task<CreateModuleResponse> Handle(CreateModuleRequest request, CancellationToken cancellationToken);
}
