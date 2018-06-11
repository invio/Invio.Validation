using System;
using System.Collections.Immutable;

namespace Invio.Validation {

    public class ValidationIssueException : OperationResultException {

        public ValidationIssueException(ValidationIssue validationIssue)
            : base(ToSet(validationIssue)) { }

        public ValidationIssueException(ValidationIssue validationIssue, Exception innerException)
            : base(ToSet(validationIssue), innerException) { }

        private static ImmutableHashSet<ValidationIssue> ToSet(ValidationIssue issue) {
            if (issue == null) {
                throw new ArgumentNullException(nameof(issue));
            }

            return ImmutableHashSet.Create<ValidationIssue>(issue);
        }

    }

}
