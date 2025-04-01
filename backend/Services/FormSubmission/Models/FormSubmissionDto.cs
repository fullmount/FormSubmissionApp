using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;

public class FormSubmissionDto
{
    public Dictionary<string, object> Fields { get; set; } = new();

    public class FormFieldValidator : AbstractValidator<FormSubmissionDto>
    {
        public FormFieldValidator()
        {
            RuleFor(x => x.Fields)
                .NotEmpty()
                .Must(HaveRequiredFields)
                .WithMessage("Form must contain all required fields");
        }

        private bool HaveRequiredFields(Dictionary<string, object> fields)
        {
            var requiredFields = new[] { "fullName", "email", "date", "choice", "agreement" };
            return requiredFields.All(field => fields.ContainsKey(field));
        }
    }
} 