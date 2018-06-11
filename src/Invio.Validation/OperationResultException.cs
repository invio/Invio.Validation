using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Invio.Validation {
    public class OperationResultException : Exception {
        public OperationResultBase OperationResult { get; }

        public OperationResultException(ISet<ValidationIssue> validationIssues)
            : base(BuildValidationIssuesMessage(validationIssues)) {
            if (validationIssues == null) {
                throw new ArgumentNullException(nameof(validationIssues));
            }

            this.OperationResult = new OperationResult(validationIssues);
        }


        public OperationResultException(ISet<ValidationIssue> validationIssues, Exception innerException)
            : base(BuildValidationIssuesMessage(validationIssues), innerException) {
            if (validationIssues == null) {
                throw new ArgumentNullException(nameof(validationIssues));
            }

            this.OperationResult = new OperationResult(validationIssues);
        }

        public OperationResultException(OperationResultBase operationResult)
            : base(BuildValidationIssuesMessage(operationResult)) {
            if (operationResult == null) {
                throw new ArgumentNullException(nameof(operationResult));
            }

            this.OperationResult = operationResult;
        }

        public OperationResultException(OperationResultBase operationResult, Exception innerException)
            : base(BuildValidationIssuesMessage(operationResult), innerException) {
            if (operationResult == null) {
                throw new ArgumentNullException(nameof(operationResult));
            }

            this.OperationResult = operationResult;
        }

        private static String BuildValidationIssuesMessage(
            OperationResultBase operationResult) {
            if (operationResult == null) {
                throw new ArgumentNullException(nameof(operationResult));
            }

            return BuildValidationIssuesMessage(operationResult.ValidationIssues);
        }

        private static String BuildValidationIssuesMessage(
            ISet<ValidationIssue> validationIssues) {
            if (validationIssues == null) {
                throw new ArgumentNullException(nameof(validationIssues));
            } else if (!validationIssues.Any()) {
                throw new ArgumentException(
                    $"The {nameof(validationIssues)} must contain at least one issue.",
                    nameof(validationIssues)
                );
            }

            if (!validationIssues.Skip(1).Any()) {
                return FormatIssueMessage(validationIssues.Single());
            }

            var issueMessages =
                validationIssues.Select(FormatIssueMessage);

            return String.Join("\n\n", issueMessages);
        }

        private static String FormatIssueMessage(ValidationIssue issue) {
            var members = issue.MemberNames ?? ImmutableHashSet<String>.Empty;
            var memberNames = String.Join(", ", members);
            var message = String.Empty;

            if (!String.IsNullOrWhiteSpace(memberNames)) {
                message += $"Member(s): {memberNames}\n";
            }

            return $"{message}{issue.Level} Message: {issue.Message}";
        }
    }
}
