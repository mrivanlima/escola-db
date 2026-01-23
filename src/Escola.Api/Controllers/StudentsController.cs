using Escola.Api.Common;
using Escola.Application.UseCases.Students.CreateStudent;
using Escola.Application.UseCases.Students.DeleteStudent;
using Escola.Application.UseCases.Students.GetStudent;
using Escola.Application.UseCases.Students.GetStudents;
using Escola.Application.UseCases.Students.UpdateStudent;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Escola.Api.Controllers;

/// <summary>
/// Students management endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly ICreateStudentHandler _createHandler;
    private readonly IDeleteStudentHandler _deleteHandler;
    private readonly IGetStudentsHandler _getStudentsHandler;
    private readonly IGetStudentHandler _getStudentHandler;
    private readonly IUpdateStudentHandler _updateHandler;
    private readonly IValidator<CreateStudentRequest> _createValidator;
    private readonly IValidator<UpdateStudentRequest> _updateValidator;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(
        ICreateStudentHandler createHandler,
        IDeleteStudentHandler deleteHandler,
        IGetStudentsHandler getStudentsHandler,
        IGetStudentHandler getStudentHandler,
        IUpdateStudentHandler updateHandler,
        IValidator<CreateStudentRequest> createValidator,
        IValidator<UpdateStudentRequest> updateValidator,
        ILogger<StudentsController> logger)
    {
        _createHandler = createHandler;
        _deleteHandler = deleteHandler;
        _getStudentsHandler = getStudentsHandler;
        _getStudentHandler = getStudentHandler;
        _updateHandler = updateHandler;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _logger = logger;
    }

    /// <summary>
    /// Get all students.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of students</returns>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Getting all students");

            var students = await _getStudentsHandler.Handle(new GetStudentsRequest(), cancellationToken);

            return Ok(ApiResponse<List<StudentResponse>>.SuccessResult(students, $"Retrieved {students.Count} students"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting students");
            return StatusCode(500, ApiResponse<List<StudentResponse>>.FailureResult(
                "An error occurred while getting students"));
        }
    }

    /// <summary>
    /// Get a student by UUID.
    /// </summary>
    /// <param name="id">Student UUID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Student details</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Getting student with UUID: {StudentUuid}", id);

            var student = await _getStudentHandler.Handle(id, cancellationToken);

            if (student == null)
            {
                _logger.LogWarning("Student with UUID {StudentUuid} not found", id);
                return NotFound(ApiResponse<StudentResponse>.FailureResult(
                    $"Student with UUID {id} not found"));
            }

            return Ok(ApiResponse<StudentResponse>.SuccessResult(student, "Student retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting student with UUID: {StudentUuid}", id);
            return StatusCode(500, ApiResponse<StudentResponse>.FailureResult(
                "An error occurred while getting the student"));
        }
    }

    /// <summary>
    /// Create a new student.
    /// </summary>
    /// <param name="request">Student data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created student with UUID</returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStudentRequest request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate the request
            var validationResult = await _createValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}").ToList();
                return BadRequest(ApiResponse<CreateStudentResponse>.FailureResult(
                    "Validation failed", errors));
            }

            _logger.LogInformation("Creating student: {FirstName} {LastName}", request.FirstName, request.LastName);

            // Create the student
            var response = await _createHandler.Handle(request, cancellationToken);

            _logger.LogInformation("Student created successfully with UUID: {StudentUuid}", response.StudentUuid);

            return Created($"/api/students/{response.StudentUuid}",
                ApiResponse<CreateStudentResponse>.SuccessResult(response, "Student created successfully"));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Invalid operation: {Message}", ex.Message);
            return BadRequest(ApiResponse<CreateStudentResponse>.FailureResult(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating student");
            return StatusCode(500, ApiResponse<CreateStudentResponse>.FailureResult(
                "An error occurred while creating the student"));
        }
    }

    /// <summary>
    /// Soft delete a student by UUID.
    /// </summary>
    /// <param name="id">Student UUID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>NoContent if successful, NotFound if student doesn't exist</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Soft deleting student with UUID: {StudentUuid}", id);

            var deleted = await _deleteHandler.Handle(id, cancellationToken);

            if (!deleted)
            {
                _logger.LogWarning("Student with UUID {StudentUuid} not found", id);
                return NotFound(ApiResponse<object>.FailureResult(
                    $"Student with UUID {id} not found"));
            }

            _logger.LogInformation("Student with UUID {StudentUuid} soft deleted successfully", id);

            return Ok(ApiResponse<object>.SuccessResult(new { }, "Student deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting student with UUID: {StudentUuid}", id);
            return StatusCode(500, ApiResponse<object>.FailureResult(
                "An error occurred while deleting the student"));
        }
    }

    /// <summary>
    /// Update a student by UUID.
    /// </summary>
    /// <param name="id">Student UUID</param>
    /// <param name="request">Updated student data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated student details</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateStudentRequest request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate the request
            var validationResult = await _updateValidator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}").ToList();
                return BadRequest(ApiResponse<UpdateStudentResponse>.FailureResult(
                    "Validation failed", errors));
            }

            _logger.LogInformation("Updating student with UUID: {StudentUuid}", id);

            var response = await _updateHandler.Handle(id, request, cancellationToken);

            if (response == null)
            {
                _logger.LogWarning("Student with UUID {StudentUuid} not found", id);
                return NotFound(ApiResponse<UpdateStudentResponse>.FailureResult(
                    $"Student with UUID {id} not found"));
            }

            _logger.LogInformation("Student with UUID {StudentUuid} updated successfully", id);

            return Ok(ApiResponse<UpdateStudentResponse>.SuccessResult(response, "Student updated successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating student with UUID: {StudentUuid}", id);
            return StatusCode(500, ApiResponse<UpdateStudentResponse>.FailureResult(
                "An error occurred while updating the student"));
        }
    }
}
