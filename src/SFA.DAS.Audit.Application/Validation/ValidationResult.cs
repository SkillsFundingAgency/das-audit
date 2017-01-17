using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Audit.Application.Validation
{
    public class ValidationResult
    {
        protected IList<ValidationError> ErrorList;
        public ValidationResult()
        {
            ErrorList = new List<ValidationError>();
        }

        public void AddError(string propertyName)
        {
            ErrorList.Add(new ValidationError {Description = $"No value supplied for {propertyName}",Property = propertyName});
        }

        public void AddError(string propertyName, string description)
        {
            ErrorList.Add(new ValidationError {Description = description, Property = propertyName});
        }
        
        public ValidationError[] Errors => ErrorList.ToArray();

        public bool IsValid => Errors == null || Errors.Length == 0;
    }
}