using backend.Services.FormSubmission;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Microsoft.Extensions.Logging;
using backend.Validators;

namespace backend.Controllers
{
    /// <summary>
    /// Form submission controller
    /// </summary>
    /// <param name="formSubmissionService">Form submission service</param>
    /// <param name="validator">Form submission validator</param>
    /// <param name="logger">Logger</param>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class FormsController : ControllerBase
    {
        private readonly IFormSubmissionService _formSubmissionService;
        private readonly IValidator<FormSubmissionDto> _validator;
        private readonly ILogger<FormsController> _logger;

        public FormsController(
            IFormSubmissionService formSubmissionService,
            IValidator<FormSubmissionDto> validator,
            ILogger<FormsController> logger)
        {
            _formSubmissionService = formSubmissionService ?? throw new ArgumentNullException(nameof(formSubmissionService));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Submits a new form
        /// </summary>
        /// <response code="201">Form successfully submitted</response>
        /// <response code="400">Invalid form data</response>
        /// <response code="499">Request cancelled by client</response>
        /// <response code="500">Internal server error</response>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(FormSubmissionResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status499ClientClosedRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FormSubmissionResponseDto>> SubmitForm(
            [FromBody] FormSubmissionDto submission,
            CancellationToken cancellationToken)
        {
            if (submission == null)
            {
                return BadRequest("Form submission cannot be null");
            }

            try
            {
                var validationResult = await _validator.ValidateAsync(submission, cancellationToken);
                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("Form validation failed: {@Errors}", validationResult.Errors);
                    return BadRequest(new ValidationProblemDetails(validationResult.Errors));
                }

                var result = await _formSubmissionService.SubmitFormAsync(submission, cancellationToken);
                _logger.LogInformation("Form submitted successfully: {FormId}", result.Id);
                
                return CreatedAtAction(
                    nameof(GetSubmission), 
                    new { id = result.Id, version = "1.0" }, 
                    result);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(StatusCodes.Status499ClientClosedRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to submit form");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
            }
        }

        /// <summary>
        /// Retrieves form submissions with optional search criteria
        /// </summary>
        /// <response code="200">Returns list of form submissions</response>
        /// <response code="400">Invalid search criteria</response>
        /// <response code="499">Request cancelled by client</response>
        /// <response code="500">Internal server error</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<FormSubmissionResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status499ClientClosedRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<FormSubmissionResponseDto>>> GetSubmissions(
            [FromQuery] SearchCriteriaDto searchCriteria, 
            CancellationToken cancellationToken)
        {
            try
            {
                if (searchCriteria?.Query?.Length == 1)
                {
                    _logger.LogWarning("Invalid search query length: {Query}", searchCriteria.Query);
                    return BadRequest("Search query must be at least 2 characters long");
                }

                var results = await _formSubmissionService.GetSubmissionsAsync(
                    cancellationToken, 
                    searchCriteria?.Query);

                _logger.LogInformation("Retrieved {Count} submissions", results.Count);
                return Ok(results);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(StatusCodes.Status499ClientClosedRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve submissions");
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred");
            }
        }
    }
}
