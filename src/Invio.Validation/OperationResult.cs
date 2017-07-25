using System.Collections.Generic;
using System.Collections.Immutable;

namespace Invio.Validation {

    public class OperationResult<T> : OperationResultBase<OperationResult<T>> {

        public T Value { get; }

        protected override OperationResult<T> self { get { return this; } }

        public OperationResult() : this(ImmutableHashSet<ValidationIssue>.Empty) { }

        public OperationResult(T value)
            : this(ImmutableHashSet<ValidationIssue>.Empty, value) {}

        public OperationResult(ISet<ValidationIssue> validationIssues)
            : base(validationIssues) { }

        public OperationResult(
            ISet<ValidationIssue> validationIssues,
            T value
        ) : base(validationIssues) {
            this.Value = value;
        }

        protected override OperationResult<T> SetValidationIssues(
            ISet<ValidationIssue> validationIssues) {

            return new OperationResult<T>(
                validationIssues,
                this.Value
            );
        }

        public OperationResult<T> SetValue(T value) {
            return new OperationResult<T>(
                validationIssues: this.ValidationIssues,
                value: value
            );
        }
    }

    public class OperationResult : OperationResultBase<OperationResult> {
        public static OperationResult Success { get; }

        static OperationResult() {
            Success = new OperationResult(ImmutableHashSet<ValidationIssue>.Empty);
        }

        protected override OperationResult self { get { return this; } }

        public OperationResult(ISet<ValidationIssue> validationIssues)
            : base(validationIssues) { }

        protected override OperationResult SetValidationIssues(
            ISet<ValidationIssue> validationIssues) {

            return new OperationResult(validationIssues);
        }
    }
}
