namespace backend.Services.FormSubmission
{
    /// <summary>
    /// Implementation of an in-memory form repository
    /// </summary>
    public class FormSubmissionRepository : IFormSubmissionRepository
    {
        private readonly List<Dictionary<string, object>> _submissions = [];
        private int _idCounter = 0;

        /// <summary>
        /// Saves form data to in-memory storage
        /// </summary>
        /// <param name="formData">Form data to save</param>
        /// <param name="cancellationToken">Operation cancel token</param>
        /// <returns>Newly created submission id</returns>
        public async Task<int> SaveAsync(Dictionary<string, object> formData, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return await Task.FromCanceled<int>(cancellationToken);
            }
            return await Task.Run(() =>
            {
                formData["Id"] = ++_idCounter;
                _submissions.Add(formData);
                return Task.FromResult((int)formData["Id"]);
            });
        }

        /// <summary>
        /// Gets sumbissions filtered by criteria, or all submissions if criteria not provided
        /// </summary>
        /// <param name="searchCriteria">Search criteria</param>
        /// <param name="cancellationToken">Operation cancel token</param>
        /// <returns>List of submitted forms</returns>
        public async Task<List<Dictionary<string, object>>> GetSubmissionsAsync(CancellationToken cancellationToken, string? searchCriteria = null)
        {
            return await Task.Run(() =>
            {
                if (searchCriteria is null) return Task.FromResult(_submissions);
                return Task.FromResult<List<Dictionary<string, object>>>(
                    [.. _submissions.Where(
                        sub => sub.Values.Any(
                            value => value.ToString()?.Contains(
                                searchCriteria, StringComparison.OrdinalIgnoreCase) == true
                        )
                    )]
                );
            });
        }
    }
}
