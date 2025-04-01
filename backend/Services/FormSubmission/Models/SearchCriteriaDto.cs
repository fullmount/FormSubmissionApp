using FluentValidation;

public class SearchCriteriaDto
{
    public string? Query { get; set; }

    public class SearchCriteriaValidator : AbstractValidator<SearchCriteriaDto>
    {
        public SearchCriteriaValidator()
        {
            RuleFor(x => x.Query)
                .MinimumLength(2)
                .When(x => !string.IsNullOrEmpty(x.Query))
                .WithMessage("Search query must be at least 2 characters long");
        }
    }
} 