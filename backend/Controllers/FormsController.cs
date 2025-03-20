using backend.Services.FormSubmission;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace backend.Controllers
{
    /// <summary>
    /// Form submission controller
    /// </summary>
    /// <param name="formStorageService">Form submission service</param>
    [Route("api/[controller]")]
    [ApiController]
    public class FormsController(IFormSubmissionService formStorageService) : ControllerBase
    {
        private readonly IFormSubmissionService _formStorageService = formStorageService;

        /// <summary>
        /// Submits a form
        /// </summary>
        /// <param name="submission">Form to submit</param>
        /// <param name="cancellationToken">Operation cancel token</param>
        /// <returns>Newly created submission id</returns>
        [HttpPost("submit")]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status499ClientClosedRequest)]
        [ProducesResponseType(400)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<int>> SubmitForm([FromBody] Dictionary<string, object> submission, CancellationToken cancellationToken)
        {
            try
            {
                return (submission == null || submission.Count == 0) ?
                    BadRequest("Empty form") :
                    CreatedAtAction(nameof(SubmitForm),await _formStorageService.SubmitFormAsync(submission, cancellationToken));
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "The request was canceled by the client.");
            }
        }

        /// <summary>
        /// Gets sumbissions filtered by criteria, or all submissions if criteria not provided
        /// </summary>
        /// <param name="searchCriteria">Search criteria</param>
        /// <param name="cancellationToken">Operation cancel token</param>
        /// <returns>List of submissions</returns>
        [HttpGet("getSubmissions")]
        [ProducesResponseType(typeof(List<Dictionary<string, object>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status499ClientClosedRequest)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<List<Dictionary<string, object>>>> GetSubmissions([FromQuery] string? searchCriteria, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _formStorageService.GetSubmissionsAsync(cancellationToken, searchCriteria));
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "The request was canceled by the client.");
            }
        }
    }
}
