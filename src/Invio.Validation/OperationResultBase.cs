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

        public TExecutionResult AddError(String message) {
            return this.AddError(message, null, null);
        }

        public TExecutionResult AddError(String message, String code) {
            return this.AddError(message, code, null);
        }

        public TExecutionResult AddError(String message, ISet<String> memberNames) {
            return this.AddError(message, null, memberNames);
        }

        public TExecutionResult AddError(
            String message,
            String code = default(String),
            ISet<String> memberNames = default(ImmutableHashSet<String>)) {

            return this.AddValidationIssue(
                ValidationIssue.Error(
                    message: message,
                    code: code,
                    memberNames: memberNames
                )
            );
        }

        public TExecutionResult AddWarning(String message) {
            return this.AddWarning(message, null, null);
        }

        public TExecutionResult AddWarning(String message, String code) {
            return this.AddWarning(message, code, null);
        }

        public TExecutionResult AddWarning(String message, ISet<String> memberNames) {
            return this.AddWarning(message, null, memberNames);
        }

        public TExecutionResult AddWarning(
            String message,
            String code = default(String),
            ISet<String> memberNames = default(ImmutableHashSet<String>)) {

            return this.AddValidationIssue(
                ValidationIssue.Warning(
                    message: message,
                    code: code,
                    memberNames: memberNames
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
