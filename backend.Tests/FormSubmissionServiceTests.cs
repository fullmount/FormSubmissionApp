using backend.Services.FormSubmission;

namespace backend.Tests
{
    public class FormSubmissionServiceTests
    {
        private readonly IFormSubmissionService _formService;
        private readonly IFormSubmissionRepository _repository;

        public FormSubmissionServiceTests()
        {
            _repository = new FormSubmissionRepository();
            _formService = new FormSubmissionService(_repository);
        }

        [Fact]
        public async Task SubmitFormAsync_ShouldStoreForm()
        {
            var formData = new Dictionary<string, object> { { "FullName", "Alice" } };
            await _formService.SubmitFormAsync(formData,CancellationToken.None);

            var allForms = await _formService.GetSubmissionsAsync(CancellationToken.None);
            Assert.Contains(allForms, form => form["FullName"].Equals("Alice"));
        }

        [Fact]
        public async Task SearchFormsAsync_ShouldReturnMatchingForms()
        {
            await _formService.SubmitFormAsync(new Dictionary<string, object> { { "FullName", "Alice" } }, CancellationToken.None);
            await _formService.SubmitFormAsync(new Dictionary<string, object> { { "FullName", "Bob" } }, CancellationToken.None);

            var results = await _formService.GetSubmissionsAsync(CancellationToken.None, "Bob");

            Assert.Single(results);
            Assert.Equal("Bob", results[0]["FullName"]);
        }

        [Fact]
        public async Task SubmitFormAsync_ShouldAbortByCancellationToken()
        {
            var formData = new Dictionary<string, object> { { "FullName", "Alice" } };
            var tokenSource = new CancellationTokenSource();
            tokenSource.Cancel();
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await _formService.SubmitFormAsync(formData, tokenSource.Token));
        }

        [Fact]
        public async Task SearchFormsAsync_ShouldAbortByCancellationToken()
        {
            var tokenSource = new CancellationTokenSource();
            await Task.Delay(100).ContinueWith(_ => tokenSource.Cancel());
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await _formService.GetSubmissionsAsync(tokenSource.Token));
        }
    }
}
