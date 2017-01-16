﻿using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Audit.Application.Validation
{
    public class ValidationResult
    {
        public ValidationResult(IEnumerable<ValidationError> errors)
        {
            Errors = (errors ?? new ValidationError[0]).ToArray();
        }

        public ValidationError[] Errors { get; }

        public bool IsValid => Errors == null || Errors.Length == 0;
    }
}