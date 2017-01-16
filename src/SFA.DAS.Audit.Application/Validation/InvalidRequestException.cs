using System;
using System.Linq;

namespace SFA.DAS.Audit.Application.Validation
{
    public class InvalidRequestException : Exception
    {
        public InvalidRequestException(ValidationError[] errors)
            : base(GetErrorMessage(errors))
        {
            Errors = errors;
        }

        public ValidationError[] Errors { get; set; }


        private static string GetErrorMessage(ValidationError[] errors)
        {
            var aggregatedErrors = errors.Select(e => e.Description).Aggregate((x, y) => $"{x}\n{y}");
            return $"Invalid request.\n\n{aggregatedErrors}";
        }
    }
}
