using System;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;

namespace Invio.Validation {
    public static class OperationResultBaseExtensions {

        public static TExecutionResult AddValidationResult<TExecutionResult>(
            this OperationResultBase<TExecutionResult> operationResult,
            ValidationResult validationResult)
            where TExecutionResult : OperationResultBase<TExecutionResult> {

            var memberNames = validationResult.MemberNames ?? ImmutableHashSet<String>.Empty;

            var issue = new ValidationIssue(
                validationResult.ErrorMessage,
                ValidationIssueLevel.Error,
                memberNames.ToImmutableHashSet()
            );

            return operationResult.AddValidationIssue(issue);
        }

    }
}
