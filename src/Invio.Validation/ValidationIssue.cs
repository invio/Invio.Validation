using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Invio.Immutable;

namespace Invio.Validation {

    public class ValidationIssue : ImmutableBase<ValidationIssue> {

        public String Message { get; }
        public ValidationIssueLevel Level { get; }
        public String Code { get; }
        public ISet<String> MemberNames { get; }

        public static ValidationIssue Error(String message) {
            return ValidationIssue.Error(message, null, null);
        }

        public static ValidationIssue Error(String message, String code) {
            return ValidationIssue.Error(message, code, null);
        }

        public static ValidationIssue Error(String message, ISet<String> memberNames) {
            return ValidationIssue.Error(message, null, memberNames);
        }

        public static ValidationIssue Error(
            String message,
            String code = default(String),
            ISet<String> memberNames = default(ImmutableHashSet<String>)) {

            return new ValidationIssue(
                message,
                ValidationIssueLevel.Error,
                code,
                memberNames
            );
        }

        public static ValidationIssue Warning(String message) {
            return ValidationIssue.Warning(message, null, null);
        }

        public static ValidationIssue Warning(String message, String code) {
            return ValidationIssue.Warning(message, code, null);
        }

        public static ValidationIssue Warning(String message, ISet<String> memberNames) {
            return ValidationIssue.Warning(message, null, memberNames);
        }

        public static ValidationIssue Warning(
            String message,
            String code = default(String),
            ISet<String> memberNames = default(ImmutableHashSet<String>)) {

            return new ValidationIssue(
                message,
                ValidationIssueLevel.Warning,
                code,
                memberNames
            );
        }

        public ValidationIssue(
            String message,
            ValidationIssueLevel level,
            String code = default(String),
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
            this.Code = code;
            this.MemberNames = memberNames;
        }

        public ValidationIssue SetMessage(String message) {
            return this.SetPropertyValueImpl(nameof(Message), message);
        }

        public ValidationIssue SetLevel(ValidationIssueLevel level) {
            return this.SetPropertyValueImpl(nameof(Level), level);
        }

        public ValidationIssue SetCode(String code) {
            return this.SetPropertyValueImpl(nameof(Code), code);
        }

        public ValidationIssue SetMemberNames(ISet<String> memberNames) {
            return this.SetPropertyValueImpl(nameof(MemberNames), memberNames);
        }

    }

}
