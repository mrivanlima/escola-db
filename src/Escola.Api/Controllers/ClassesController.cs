using Escola.Api.Common;
using Escola.Application.UseCases.Classes.CreateClass;
using Escola.Application.UseCases.Classes.DeleteClass;
using Escola.Application.UseCases.Classes.GetClass;
using Escola.Application.UseCases.Classes.GetClasses;
using Escola.Application.UseCases.Classes.UpdateClass;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Escola.Api.Controllers;

/// <summary>
/// Classes management endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ClassesController : ControllerBase
{
    private readonly IGetClassesHandler _getClassesHandler;
    private readonly IGetClassHandler _getClassHandler;
    private readonly ICreateClassHandler _createClassHandler;
    private readonly IUpdateClassHandler _updateClassHandler;
    private readonly IDeleteClassHandler _deleteClassHandler;
    private readonly IValidator<CreateClassRequest> _createValidator;
    private readonly IValidator<UpdateClassRequest> _updateValidator;
    private readonly ILogger<ClassesController> _logger;

    public ClassesController(
        IGetClassesHandler getClassesHandler,
        IGetClassHandler getClassHandler,
        ICreateClassHandler createClassHandler,
        IUpdateClassHandler updateClassHandler,
        IDeleteClassHandler deleteClassHandler,
        IValidator<CreateClassRequest> createValidator,
        IValidator<UpdateClassRequest> updateValidator,
        ILogger<ClassesController> logger)
    {
        _getClassesHandler = getClassesHandler;
        _getClassHandler = getClassHandler;
        _createClassHandler = createClassHandler;
        _updateClassHandler = updateClassHandler;
        _deleteClassHandler = deleteClassHandler;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _logger = logger;
    }

    /// <summary>
    /// Get all classes.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetClassesRequest();
            var classes = await _getClassesHandler.Handle(request, cancellationToken);

            return Ok(ApiResponse<List<ClassResponse>>.SuccessResult(
                classes,
                $"Retrieved {classes.Count} classes"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching classes");
            return StatusCode(500, ApiResponse<object>.FailureResult(
                "An error occurred while fetching classes",
                new List<string> { ex.Message }));
        }
    }

    /// <summary>
    /// Get a class by UUID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var classEntity = await _getClassHandler.Handle(id, cancellationToken);

            if (classEntity == null)
            {
                return NotFound(ApiResponse<object>.FailureResult(
                    $"Class with UUID {id} not found"));
            }

            return Ok(ApiResponse<ClassResponse>.SuccessResult(
                classEntity,
                "Class retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching class {ClassUuid}", id);
            return StatusCode(500, ApiResponse<object>.FailureResult(
                "An error occurred while fetching the class",
                new List<string> { ex.Message }));
        }
    }

    /// <summary>
    /// Create a new class.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClassRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _createValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return BadRequest(ApiResponse<object>.FailureResult(
                    "Validation failed",
                    validationResult.Errors.Select(e => e.ErrorMessage).ToList()));
            }

            var response = await _createClassHandler.Handle(request, cancellationToken);

            return Created($"/api/classes/{response.ClassUuid}",
                ApiResponse<CreateClassResponse>.SuccessResult(
                    response,
                    "Class created successfully"));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Validation error creating class");
            return BadRequest(ApiResponse<object>.FailureResult(
                ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating class");
            return StatusCode(500, ApiResponse<object>.FailureResult(
                "An error occurred while creating the class",
                new List<string> { ex.Message }));
        }
    }

    /// <summary>
    /// Update an existing class.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateClassRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _updateValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return BadRequest(ApiResponse<object>.FailureResult(
                    "Validation failed",
                    validationResult.Errors.Select(e => e.ErrorMessage).ToList()));
            }

            var response = await _updateClassHandler.Handle(id, request, cancellationToken);

            if (response == null)
            {
                return NotFound(ApiResponse<object>.FailureResult(
                    $"Class with UUID {id} not found"));
            }

            return Ok(ApiResponse<UpdateClassResponse>.SuccessResult(
                response,
                "Class updated successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating class {ClassUuid}", id);
            return StatusCode(500, ApiResponse<object>.FailureResult(
                "An error occurred while updating the class",
                new List<string> { ex.Message }));
        }
    }

    /// <summary>
    /// Soft delete a class.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var success = await _deleteClassHandler.Handle(id, cancellationToken);

            if (!success)
            {
                return NotFound(ApiResponse<object>.FailureResult(
                    $"Class with UUID {id} not found"));
            }

            return Ok(ApiResponse<object>.SuccessResult(
                new { },
                "Class deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting class {ClassUuid}", id);
            return StatusCode(500, ApiResponse<object>.FailureResult(
                "An error occurred while deleting the class",
                new List<string> { ex.Message }));
        }
    }
}
