using backend.Services.FormSubmission;

namespace backend.Tests
{
    public class FormSubmissionRepositoryTests
    {
        private FormSubmissionRepository CreateStorage() => new();

        [Fact]
        public async Task AddSubmission_ShouldStoreData()
        {
            var storage = CreateStorage();
            var form = new Dictionary<string, object> { { "FullName", "Jack Black" } };

            await storage.SaveAsync(form, CancellationToken.None);
            var submissions = await storage.GetSubmissionsAsync(CancellationToken.None);

            Assert.Single(submissions);
            Assert.True(submissions[0].ContainsKey("FullName"));
            Assert.Equal("Jack Black", submissions[0]["FullName"]);
        }

        [Fact]
        public async Task AddSubmission_ShouldAssignId()
        {
            var storage = CreateStorage();
            var form = new Dictionary<string, object> { { "FullName", "Jack Black" } };

            await storage.SaveAsync(form,CancellationToken.None);
            var submissions = await storage.GetSubmissionsAsync(CancellationToken.None);

            Assert.Single(submissions);
            Assert.True(submissions[0].ContainsKey("Id"));
            Assert.IsType<int>(submissions[0]["Id"]);
        }

        [Theory]
        [InlineData("Jack Black", "Jack", 1)]
        [InlineData("Steve Smith", "Jack", 0)]
        [InlineData("Anna Johnson", "Anna", 1)]
        [InlineData("Michael Brown", "Brown", 1)]
        [InlineData("Chris Evans", "Chris", 1)]
        [InlineData("Natalie Portman", "Leo", 0)]
        public async Task SearchSubmissions_ShouldWorkCorrectly(string fullName, string searchQuery, int expectedCount)
        {
            var storage = CreateStorage();
            var form = new Dictionary<string, object> { { "FullName", fullName } };

            await storage.SaveAsync(form, CancellationToken.None);
            var result = await storage.GetSubmissionsAsync(CancellationToken.None, searchQuery);

            Assert.Equal(expectedCount, result.Count);
        }

        [Fact]
        public async Task SearchSubmissions_ShouldReturnEmptyList_WhenNoMatch()
        {
            var storage = CreateStorage();
            var form = new Dictionary<string, object> { { "FullName", "Jack Black" } };

            await storage.SaveAsync(form, CancellationToken.None);
            var result = await storage.GetSubmissionsAsync(CancellationToken.None, "Unknown Name");

            Assert.Empty(result);
        }
    }
}
