using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Invio.Immutable;

namespace Invio.Validation {
    public class ValidationIssue : ImmutableBase<ValidationIssue> {

        public String Message { get; }
        public ValidationIssueLevel Level { get; }
        public ISet<String> MemberNames { get; }

        public static ValidationIssue Error(
            String message,
            ISet<String> memberNames = default(ImmutableHashSet<String>)) {
            return new ValidationIssue(
                message,
                ValidationIssueLevel.Error,
                memberNames
            );
        }

        public static ValidationIssue Warning(
            String message,
            ISet<String> memberNames = default(ImmutableHashSet<String>)) {
            return new ValidationIssue(
                message,
                ValidationIssueLevel.Warning,
                memberNames
            );
        }

        public ValidationIssue(
            String message,
            ValidationIssueLevel level,
            ISet<String> memberNames = default(ImmutableHashSet<String>)) {
            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            } else if (String.IsNullOrWhiteSpace(message)) {
                throw new ArgumentException(
                    $"The {nameof(message)} argument should be populated and should not just be whitespace.",
                    nameof(message)
                );
            }

            this.Message = message;
            this.Level = level;
            this.MemberNames = memberNames;
        }

        public ValidationIssue SetMessage(String message) {
            if (message == null) {
                throw new ArgumentNullException(nameof(message));
            } else if (String.IsNullOrWhiteSpace(message)) {
                throw new ArgumentException(
                    $"The {nameof(message)} argument should be populated and should not just be whitespace.",
                    nameof(message)
                );
            }

            return this.SetPropertyValueImpl(nameof(Message), message);
        }

        public ValidationIssue SetLevel(ValidationIssueLevel level) {
            return this.SetPropertyValueImpl(nameof(Level), level);
        }

        public ValidationIssue SetMemberNames(ISet<String> memberNames) {
            return this.SetPropertyValueImpl(nameof(MemberNames), memberNames);
        }
    }
}
