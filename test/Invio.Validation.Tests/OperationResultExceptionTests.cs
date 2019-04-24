using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Invio.Xunit;
using Xunit;

namespace Invio.Validation {

    [UnitTest]
    public class OperationResultExceptionTests {

        [Fact]
        public void Constructor_NullValidationIssues() {

            // Act

            var exception = Record.Exception(
                () => new OperationResultException((ISet<ValidationIssue>) null)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void Constructor_NullOperationResult() {

            // Act

            var exception = Record.Exception(
                () => new OperationResultException((OperationResultBase) null)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void Constructor_NullInnerException() {

            // Arrange

            var issues = ImmutableHashSet.Create(
                ValidationIssue.Error("Foo")
            );

            // Act

            var exception = Record.Exception(
                () => new OperationResultException(issues, null)
            );

            // Assert

            Assert.Null(exception);
        }

        [Fact]
        public void Constructor_NoValidationIssue() {

            // Act

            var exception = Record.Exception(
                () => new OperationResultException(ImmutableHashSet<ValidationIssue>.Empty)
            );

            // Assert

            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void Constructor_SuccessfulOperationResult() {

            // Act

            var exception = Record.Exception(
                () => new OperationResultException(OperationResult.Success)
            );

            // Assert

            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void Constructor_InnerException() {

            // Arrange

            var issues = ImmutableHashSet.Create(
                ValidationIssue.Error("Foo")
            );
            var innerException = new InvalidOperationException();

            // Act

            var exception = new OperationResultException(
                issues,
                innerException
            );

            // Assert

            Assert.Equal(issues, exception.OperationResult.ValidationIssues);
            Assert.Equal(innerException, exception.InnerException);
        }

        [Fact]
        public void ExceptionMessage_SingleIssue_Message() {

            // Arrange

            var message = "Foo is beyond all reason.";
            var issues = ImmutableHashSet.Create(
                ValidationIssue.Error(message)
            );

            // Act

            var exception = new OperationResultException(issues);

            // Assert

            Assert.Equal($"Error Message: {message}", exception.Message);
        }

        [Fact]
        public void ExceptionMessage_SingleIssue_Message_WithMembers() {

            // Arrange

            var message = "Foo is beyond all reason.";
            var names = ImmutableHashSet.Create("Foo", "Bar");
            var issues = ImmutableHashSet.Create(
                ValidationIssue.Error(message, memberNames: names)
            );

            // Act

            var exception = new OperationResultException(issues);

            // Assert

            var memberNames = String.Join(", ", names);
            Assert.Equal($"Member(s): {memberNames}\nError Message: {message}", exception.Message);
        }

        [Fact]
        public void ExceptionMessage_MultipleIssues_Message() {

            // Arrange

            var message = "Foo is beyond all reason.";
            var warningMessage = "What is Kung Fu Fighting?";
            var names = ImmutableHashSet.Create("Foo", "Bar");
            var issues = ImmutableHashSet.Create(
                ValidationIssue.Error(message, memberNames: names),
                ValidationIssue.Warning(warningMessage)
            );

            // Act

            var exception = new OperationResultException(issues);

            // Assert

            var memberNames = String.Join(", ", names);
            Assert.Contains($"Member(s): {memberNames}\nError Message: {message}", exception.Message);
            Assert.Contains($"Warning Message: {warningMessage}", exception.Message);
        }
    }
}
