using System;
using System.Collections.Immutable;

namespace Invio.Validation {
    public class ValidationIssueException : OperationResultException {

        public ValidationIssueException(ValidationIssue validationIssue)
            : base(ImmutableHashSet.Create(validationIssue)) { }

        public ValidationIssueException(ValidationIssue validationIssue, Exception innerException)
            : base(ImmutableHashSet.Create(validationIssue), innerException) { }
    }
}
