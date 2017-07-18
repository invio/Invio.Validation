using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Invio.Validation {

    public class ExecutionResult<T> : BaseExecutionResult<ExecutionResult<T>> {

        public T Result { get; }

        protected override ExecutionResult<T> self { get { return this; } }

        public ExecutionResult() : this(ImmutableList<ValidationResult>.Empty) { }

        public ExecutionResult(T result)
            : this(ImmutableList<ValidationResult>.Empty, result) {}

        public ExecutionResult(IEnumerable<ValidationResult> validationResults)
            : base(validationResults) { }

        public ExecutionResult(
            IEnumerable<ValidationResult> validationResults,
            T result
        ) : base(validationResults) {
            this.Result = result;
        }

        protected override ExecutionResult<T> SetValidationResults(
            IEnumerable<ValidationResult> validationResults) {

            return new ExecutionResult<T>(
                validationResults,
                this.Result
            );
        }

        public ExecutionResult<T> SetResult(T result) {
            return new ExecutionResult<T>(
                validationResults: this.ValidationResults,
                result: result
            );
        }
    }

    public class ExecutionResult : BaseExecutionResult<ExecutionResult> {
        public static ExecutionResult Success { get; }

        static ExecutionResult() {
            Success = new ExecutionResult(ImmutableList<ValidationResult>.Empty);
        }

        protected override ExecutionResult self { get { return this; } }

        public ExecutionResult(IEnumerable<ValidationResult> validationResults)
            : base(validationResults) { }

        protected override ExecutionResult SetValidationResults(
            IEnumerable<ValidationResult> validationResults) {

            return new ExecutionResult(validationResults);
        }
    }

    public abstract class BaseExecutionResult<TExecutionResult>
        where TExecutionResult : BaseExecutionResult<TExecutionResult> {

        public bool IsSuccessful => !this.ValidationResults.Any();
        protected abstract TExecutionResult self { get; }
        public IList<ValidationResult> ValidationResults { get; }

        public BaseExecutionResult(IEnumerable<ValidationResult> validationResults) {
            if (validationResults == null) {
                throw new ArgumentNullException(nameof(validationResults));
            }

            this.ValidationResults = validationResults.ToImmutableList();
        }

        public TExecutionResult AddValidationResult(ValidationResult validationResult) {
            if (validationResult == null) {
                throw new ArgumentNullException(nameof(validationResult));
            }

            return this.AddValidationResults(new [] { validationResult });
        }

        public TExecutionResult AddValidationResults(
            IEnumerable<ValidationResult> validationResults) {

            if (validationResults == null) {
                throw new ArgumentNullException(nameof(validationResults));
            }

            if (!validationResults.Any()) {
                return this.self;
            }

            return this.SetValidationResults(
                ImmutableList
                    .CreateRange(this.ValidationResults)
                    .AddRange(validationResults)
            );
        }

        protected abstract TExecutionResult SetValidationResults(
            IEnumerable<ValidationResult> validationResults);
    }

}
