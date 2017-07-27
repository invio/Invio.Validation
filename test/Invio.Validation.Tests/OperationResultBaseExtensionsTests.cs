using System;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using Invio.Xunit;
using Xunit;

namespace Invio.Validation {

    [UnitTest]
    public class OperationResultBaseExtensionsTests {

        [Fact]
        public void AddValidationResult_Valid_NonGeneric() {

            // Arrange

            var validationResult = new ValidationResult(
                "Foo is empty",
                ImmutableHashSet.Create("Foo", "Bar")
            );
            var result = OperationResult.Success;

            // Act

            result = result.AddValidationResult(validationResult);

            // Assert

            Assert.False(result.IsSuccessful);
            var issue = Assert.Single(result.ValidationIssues);
            Assert.Equal(validationResult.ErrorMessage, issue.Message);
            Assert.Equal(validationResult.MemberNames, issue.MemberNames);
        }

        [Fact]
        public void AddValidationResult_Valid_Generic() {

            // Arrange

            var validationResult = new ValidationResult(
                "Foo is empty",
                ImmutableHashSet.Create("Foo", "Bar")
            );
            var result = new OperationResult<Object>();

            // Act

            result = result.AddValidationResult(validationResult);

            // Assert

            Assert.False(result.IsSuccessful);
            var issue = Assert.Single(result.ValidationIssues);
            Assert.Equal(validationResult.ErrorMessage, issue.Message);
            Assert.Equal(validationResult.MemberNames, issue.MemberNames);
        }

        [Fact]
        public void AddValidationResult_NullMemberNames() {

            // Arrange

            var validationResult = new ValidationResult("Foo is empty");
            var result = new OperationResult<Object>();

            // Act

            result = result.AddValidationResult(validationResult);

            // Assert

            Assert.False(result.IsSuccessful);
            var issue = Assert.Single(result.ValidationIssues);
            Assert.Equal(validationResult.ErrorMessage, issue.Message);
            Assert.NotNull(issue.MemberNames);
            Assert.Empty(issue.MemberNames);
        }

    }
}
