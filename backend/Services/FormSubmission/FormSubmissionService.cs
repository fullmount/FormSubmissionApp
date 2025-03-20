namespace backend.Services.FormSubmission
{
    /// <summary>
    /// Form submission service
    /// </summary>
    /// <param name="formRepository"></param>
    public class FormSubmissionService(IFormSubmissionRepository formRepository) : IFormSubmissionService
    {
        private readonly IFormSubmissionRepository formRepository = formRepository;

        /// <summary>
        /// Submits form asynchronously
        /// </summary>
        /// <param name="formData">Form to submit</param>
        /// <param name="cancellationToken">Operation cancel token</param>
        /// <returns>Newly created submission id</returns>
        public async Task<int> SubmitFormAsync(Dictionary<string, object> formData, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await formRepository.SaveAsync(formData, cancellationToken);
        }

        /// <summary>
        /// Gets sumbissions filtered by criteria, or all submissions if criteria not provided
        /// </summary>
        /// <param name="searchCriteria">Search criteria</param>
        /// <param name="cancellationToken">Operation cancel token</param>
        /// <returns>List of submitted forms</returns>
        public async Task<List<Dictionary<string, object>>> GetSubmissionsAsync(CancellationToken cancellationToken, string? searchCriteria = null)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await formRepository.GetSubmissionsAsync(cancellationToken, searchCriteria);
        }
    }
}
