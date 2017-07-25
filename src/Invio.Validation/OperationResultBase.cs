using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Invio.Validation {
    public abstract class OperationResultBase<TExecutionResult> : OperationResultBase
        where TExecutionResult : OperationResultBase<TExecutionResult> {

        protected abstract TExecutionResult self { get; }

        public OperationResultBase(ISet<ValidationIssue> validationIssues)
            : base(validationIssues) { }

        public TExecutionResult AddError(
            String message,
            ISet<String> memberNames = default(ImmutableHashSet<String>)) {

            return this.AddValidationIssue(
                ValidationIssue.Error(
                    message,
                    memberNames
                )
            );
        }

        public TExecutionResult AddWarning(
            String message,
            ISet<String> memberNames = default(ImmutableHashSet<String>)) {

            return this.AddValidationIssue(
                ValidationIssue.Warning(
                    message,
                    memberNames
                )
            );
        }

        public TExecutionResult AddValidationIssue(ValidationIssue validationIssue) {
            if (validationIssue == null) {
                throw new ArgumentNullException(nameof(validationIssue));
            }

            return this.AddValidationIssues(ImmutableHashSet.Create(validationIssue));
        }

        public TExecutionResult AddValidationIssues(
            ISet<ValidationIssue> validationIssues) {

            if (validationIssues == null) {
                throw new ArgumentNullException(nameof(validationIssues));
            }

            if (!validationIssues.Any()) {
                return this.self;
            }

            return this.SetValidationIssues(
                validationIssues.Aggregate(
                    this.ValidationIssues,
                    (hashSet, issue) => hashSet.Add(issue)
                )
            );
        }

        protected abstract TExecutionResult SetValidationIssues(
            ISet<ValidationIssue> validationResults);
    }

    public abstract class OperationResultBase {
        public bool IsSuccessful => !this.ValidationIssues.Any(issue => issue.Level > ValidationIssueLevel.Warning);
        public ImmutableHashSet<ValidationIssue> ValidationIssues { get; }

        public OperationResultBase(ISet<ValidationIssue> validationIssues) {
            if (validationIssues == null) {
                throw new ArgumentNullException(nameof(validationIssues));
            }

            this.ValidationIssues = validationIssues.ToImmutableHashSet();
        }
    }
}
