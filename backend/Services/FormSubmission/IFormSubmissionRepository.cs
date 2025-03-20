namespace backend.Services.FormSubmission
{
    /// <summary>
    /// Interface for handling form storage
    /// </summary>
    public interface IFormSubmissionRepository
    {
        /// <summary>
        /// Saves form data
        /// </summary>
        /// <param name="formData">Form data to save</param>
        /// <param name="cancellationToken">Operation cancel token</param>
        /// <returns>Newly created submission id</returns>
        Task<int> SaveAsync(Dictionary<string, object> formData, CancellationToken cancellationToken);
        /// <summary>
        /// Gets sumbissions filtered by criteria, or all submissions if criteria not provided
        /// </summary>
        /// <param name="searchCriteria">Search criteria</param>
        /// <param name="cancellationToken">Operation cancel token</param>
        /// <returns>List of submitted forms</returns>
        Task<List<Dictionary<string, object>>> GetSubmissionsAsync(CancellationToken cancellationToken, string? searchCriteria = null);
    }
}
