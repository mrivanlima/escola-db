using Escola.Api.Common;
using Escola.Application.Content.Modules;
using Microsoft.AspNetCore.Mvc;

namespace Escola.API.Controllers.Content;

[ApiController]
[Route("api/modules")]
public class ModulesController : ControllerBase
{
    private readonly ICreateModuleHandler _createHandler;
    private readonly IGetModuleHandler _getHandler;
    private readonly IGetModulesHandler _getAllHandler;
    private readonly IUpdateModuleHandler _updateHandler;
    private readonly CreateModuleValidator _createValidator;
    private readonly UpdateModuleValidator _updateValidator;

    public ModulesController(
        ICreateModuleHandler createHandler,
        IGetModuleHandler getHandler,
        IGetModulesHandler getAllHandler,
        IUpdateModuleHandler updateHandler,
        CreateModuleValidator createValidator,
        UpdateModuleValidator updateValidator)
    {
        _createHandler = createHandler;
        _getHandler = getHandler;
        _getAllHandler = getAllHandler;
        _updateHandler = updateHandler;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ModuleDto>>> CreateModule(
        [FromBody] CreateModuleDto dto,
        CancellationToken cancellationToken)
    {
        var validationResult = await _createValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(ApiResponse<ModuleDto>.FailureResult(
                string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage))));
        }

        var request = new CreateModuleRequest(dto);
        var response = await _createHandler.Handle(request, cancellationToken);

        return CreatedAtAction(
            nameof(GetModule),
            new { uuid = response.Module.ModuleUuid },
            ApiResponse<ModuleDto>.SuccessResult(response.Module));
    }

    [HttpGet("{uuid:guid}")]
    public async Task<ActionResult<ApiResponse<ModuleDto>>> GetModule(
        Guid uuid,
        CancellationToken cancellationToken)
    {
        var request = new GetModuleRequest(uuid);
        var response = await _getHandler.Handle(request, cancellationToken);

        return Ok(ApiResponse<ModuleDto>.SuccessResult(response.Module));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<ModuleDto>>>> GetModules(
        CancellationToken cancellationToken)
    {
        var request = new GetModulesRequest();
        var response = await _getAllHandler.Handle(request, cancellationToken);

        return Ok(ApiResponse<List<ModuleDto>>.SuccessResult(response.Modules));
    }

    [HttpPut("{uuid:guid}")]
    public async Task<ActionResult<ApiResponse<ModuleDto>>> UpdateModule(
        Guid uuid,
        [FromBody] UpdateModuleDto dto,
        CancellationToken cancellationToken)
    {
        var validationResult = await _updateValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return BadRequest(ApiResponse<ModuleDto>.FailureResult(
                string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage))));
        }

        var request = new UpdateModuleRequest(uuid, dto);
        var response = await _updateHandler.Handle(request, cancellationToken);

        return Ok(ApiResponse<ModuleDto>.SuccessResult(response.Module));
    }
}
