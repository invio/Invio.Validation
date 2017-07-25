using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Invio.Xunit;
using Xunit;

namespace Invio.Validation {

    [UnitTest]
    public class ValidationIssueExceptionTests {

        [Fact]
        public void Constructor_NullValidationIssue() {

            // Act

            var exception = Record.Exception(
                () => new ValidationIssueException((ValidationIssue) null)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void Constructor_NullInnerException() {

            // Arrange

            var issue = ValidationIssue.Error("Foo");

            // Act

            var exception = Record.Exception(
                () => new ValidationIssueException(issue, null)
            );

            // Assert

            Assert.Null(exception);
        }

        [Fact]
        public void Constructor_Valid() {

            // Arrange

            var issue = ValidationIssue.Error("Foo");

            // Act

            var exception = new ValidationIssueException(issue);

            // Assert

            Assert.Null(exception.InnerException);
            Assert.NotNull(exception.OperationResult);
            Assert.NotNull(exception.OperationResult.ValidationIssues);
            var actualIssue = Assert.Single(exception.OperationResult.ValidationIssues);
            Assert.Equal(issue, actualIssue);
        }

        [Fact]
        public void Constructor_WithInnerException() {

            // Arrange

            var issue = ValidationIssue.Error("Foo");
            var innerException = new InvalidOperationException();

            // Act

            var exception = new ValidationIssueException(issue, innerException);

            // Assert

            Assert.NotNull(exception.InnerException);
            Assert.Equal(innerException, exception.InnerException);
        }
    }
}
