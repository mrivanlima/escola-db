using Escola.Api.Common;
using Escola.Application.UseCases.Teachers.CreateTeacher;
using Escola.Application.UseCases.Teachers.DeleteTeacher;
using Escola.Application.UseCases.Teachers.GetTeacher;
using Escola.Application.UseCases.Teachers.GetTeachers;
using Escola.Application.UseCases.Teachers.UpdateTeacher;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Escola.Api.Controllers;

/// <summary>
/// Teachers management endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TeachersController : ControllerBase
{
    private readonly IGetTeachersHandler _getTeachersHandler;
    private readonly IGetTeacherHandler _getTeacherHandler;
    private readonly ICreateTeacherHandler _createTeacherHandler;
    private readonly IUpdateTeacherHandler _updateTeacherHandler;
    private readonly IDeleteTeacherHandler _deleteTeacherHandler;
    private readonly IValidator<CreateTeacherRequest> _createValidator;
    private readonly IValidator<UpdateTeacherRequest> _updateValidator;
    private readonly ILogger<TeachersController> _logger;

    public TeachersController(
        IGetTeachersHandler getTeachersHandler,
        IGetTeacherHandler getTeacherHandler,
        ICreateTeacherHandler createTeacherHandler,
        IUpdateTeacherHandler updateTeacherHandler,
        IDeleteTeacherHandler deleteTeacherHandler,
        IValidator<CreateTeacherRequest> createValidator,
        IValidator<UpdateTeacherRequest> updateValidator,
        ILogger<TeachersController> logger)
    {
        _getTeachersHandler = getTeachersHandler;
        _getTeacherHandler = getTeacherHandler;
        _createTeacherHandler = createTeacherHandler;
        _updateTeacherHandler = updateTeacherHandler;
        _deleteTeacherHandler = deleteTeacherHandler;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _logger = logger;
    }

    /// <summary>
    /// Get all teachers.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetTeachersRequest();
            var teachers = await _getTeachersHandler.Handle(request, cancellationToken);

            return Ok(ApiResponse<List<TeacherResponse>>.SuccessResult(
                teachers,
                $"Retrieved {teachers.Count} teachers"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching teachers");
            return StatusCode(500, ApiResponse<object>.FailureResult(
                "An error occurred while fetching teachers",
                new List<string> { ex.Message }));
        }
    }

    /// <summary>
    /// Get a teacher by UUID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var teacher = await _getTeacherHandler.Handle(id, cancellationToken);

            if (teacher == null)
            {
                return NotFound(ApiResponse<object>.FailureResult(
                    $"Teacher with UUID {id} not found"));
            }

            return Ok(ApiResponse<TeacherResponse>.SuccessResult(
                teacher,
                "Teacher retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching teacher {TeacherUuid}", id);
            return StatusCode(500, ApiResponse<object>.FailureResult(
                "An error occurred while fetching the teacher",
                new List<string> { ex.Message }));
        }
    }

    /// <summary>
    /// Create a new teacher.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTeacherRequest request, CancellationToken cancellationToken)
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

            var response = await _createTeacherHandler.Handle(request, cancellationToken);

            return Created($"/api/teachers/{response.TeacherUuid}",
                ApiResponse<CreateTeacherResponse>.SuccessResult(
                    response,
                    "Teacher created successfully"));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Validation error creating teacher");
            return BadRequest(ApiResponse<object>.FailureResult(
                ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating teacher");
            return StatusCode(500, ApiResponse<object>.FailureResult(
                "An error occurred while creating the teacher",
                new List<string> { ex.Message }));
        }
    }

    /// <summary>
    /// Update an existing teacher.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTeacherRequest request, CancellationToken cancellationToken)
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

            var response = await _updateTeacherHandler.Handle(id, request, cancellationToken);

            if (response == null)
            {
                return NotFound(ApiResponse<object>.FailureResult(
                    $"Teacher with UUID {id} not found"));
            }

            return Ok(ApiResponse<UpdateTeacherResponse>.SuccessResult(
                response,
                "Teacher updated successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating teacher {TeacherUuid}", id);
            return StatusCode(500, ApiResponse<object>.FailureResult(
                "An error occurred while updating the teacher",
                new List<string> { ex.Message }));
        }
    }

    /// <summary>
    /// Soft delete a teacher.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var success = await _deleteTeacherHandler.Handle(id, cancellationToken);

            if (!success)
            {
                return NotFound(ApiResponse<object>.FailureResult(
                    $"Teacher with UUID {id} not found"));
            }

            return Ok(ApiResponse<object>.SuccessResult(
                new { },
                "Teacher deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting teacher {TeacherUuid}", id);
            return StatusCode(500, ApiResponse<object>.FailureResult(
                "An error occurred while deleting the teacher",
                new List<string> { ex.Message }));
        }
    }
}
