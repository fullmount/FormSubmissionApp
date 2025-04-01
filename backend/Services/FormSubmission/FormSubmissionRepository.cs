namespace backend.Services.FormSubmission
{
    /// <summary>
    /// Implementation of an in-memory form repository
    /// </summary>
    public class FormSubmissionRepository : IFormSubmissionRepository
    {
        private static readonly ConcurrentDictionary<int, Dictionary<string, object>> _storage = 
            new ConcurrentDictionary<int, Dictionary<string, object>>();
        private static int _currentId = 0;
        private const int MAX_STORAGE_SIZE = 10000;

        /// <summary>
        /// Saves form data to in-memory storage
        /// </summary>
        /// <param name="form">Form data to save</param>
        /// <param name="token">Operation cancel token</param>
        /// <returns>Newly created submission id</returns>
        public async Task<int> SaveAsync(Dictionary<string, object> form, CancellationToken token)
        {
            if (_storage.Count >= MAX_STORAGE_SIZE)
            {
                throw new StorageLimitExceededException($"Storage limit of {MAX_STORAGE_SIZE} items reached");
            }

            token.ThrowIfCancellationRequested();

            var id = Interlocked.Increment(ref _currentId);
            form["Id"] = id;

            if (!_storage.TryAdd(id, form))
            {
                throw new ConcurrentOperationException("Failed to add form due to concurrent operation");
            }

            return id;
        }

        /// <summary>
        /// Gets sumbissions filtered by criteria, or all submissions if criteria not provided
        /// </summary>
        /// <param name="token">Operation cancel token</param>
        /// <param name="searchTerm">Search term</param>
        /// <returns>List of submitted forms</returns>
        public async Task<List<Dictionary<string, object>>> GetSubmissionsAsync(
            CancellationToken token,
            string? searchTerm = null)
        {
            token.ThrowIfCancellationRequested();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return _storage.Values.ToList();
            }

            var searchTerms = searchTerm.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            
            return _storage.Values
                .Where(form => ContainsSearchTerms(form, searchTerms))
                .ToList();
        }

        private static bool ContainsSearchTerms(Dictionary<string, object> form, string[] searchTerms)
        {
            return searchTerms.All(term =>
                form.Values.Any(value => 
                    value?.ToString()?.Contains(term, StringComparison.OrdinalIgnoreCase) == true));
        }
    }
}
