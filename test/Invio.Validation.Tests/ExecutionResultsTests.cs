using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Invio.Xunit;
using Xunit;

namespace Invio.Validation {

    [UnitTest]
    public class ExecutionResultTests {

        [Fact]
        public void Constructor_NullValidationResults_NonGeneric() {

            // Act

            var exception = Record.Exception(
                () => new ExecutionResult(null)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void Constructor_NullValidationResults_Generic() {

            // Act

            var exception = Record.Exception(
                () => new ExecutionResult<Int32>((IEnumerable<ValidationResult>) null)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void Success_IsSuccessful_NonGeneric() {
            Assert.NotNull(ExecutionResult.Success);
            Assert.True(ExecutionResult.Success.IsSuccessful);

            Assert.NotNull(ExecutionResult.Success.ValidationResults);
            Assert.Empty(ExecutionResult.Success.ValidationResults);
        }

        [Fact]
        public void Success_IsSuccessful_Generic() {
            var result = new ExecutionResult<Object>();
            Assert.True(result.IsSuccessful);
            Assert.Equal(default(Object), result.Result);

            Assert.NotNull(ExecutionResult.Success.ValidationResults);
            Assert.Empty(ExecutionResult.Success.ValidationResults);
        }

        [Fact]
        public void IsSuccessful_TrueWithoutResults_NonGeneric() {

            // Arrange

            var validationResults = ImmutableList<ValidationResult>.Empty;

            // Act

            var result = new ExecutionResult(validationResults);

            // Assert

            Assert.True(result.IsSuccessful);
            Assert.Empty(result.ValidationResults);
        }

        [Fact]
        public void IsSuccessful_TrueWithoutResults_Generic() {

            // Arrange

            var validationResults = ImmutableList<ValidationResult>.Empty;

            // Act

            var result = new ExecutionResult<Object>(validationResults);

            // Assert

            Assert.True(result.IsSuccessful);
            Assert.Empty(result.ValidationResults);
            Assert.Equal(default(Object), result.Result);
        }

        [Fact]
        public void IsSuccessful_TrueWithResult_Generic() {

            // Arrange

            var realResult = new Object();

            // Act

            var result = new ExecutionResult<Object>(realResult);

            // Assert

            Assert.True(result.IsSuccessful);
            Assert.Empty(result.ValidationResults);
            Assert.Equal(realResult, result.Result);
        }

        [Fact]
        public void IsSuccessful_FalseWithResults_NonGeneric() {

            // Arrange

            const string message = "Foo";
            var validationResults = ImmutableList.Create(new ValidationResult(message));

            // Act

            var result = new ExecutionResult(validationResults);

            // Assert

            Assert.False(result.IsSuccessful);
            var validationResult = Assert.Single(result.ValidationResults);
            Assert.Equal(message, validationResult.ErrorMessage);
        }

        [Fact]
        public void IsSuccessful_FalseWithResults_Generic() {

            // Arrange

            const string message = "Foo";
            var validationResults = ImmutableList.Create(new ValidationResult(message));

            // Act

            var result = new ExecutionResult<Object>(validationResults);

            // Assert

            Assert.False(result.IsSuccessful);
            var validationResult = Assert.Single(result.ValidationResults);
            Assert.Equal(message, validationResult.ErrorMessage);
            Assert.Equal(default(Object), result.Result);
        }

        [Fact]
        public void AddValidationResult_NullValidationResult_NonGeneric() {

            // Arrange

            var result = ExecutionResult.Success;

            // Act

            var exception = Record.Exception(
                () => result.AddValidationResult(null)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void AddValidationResult_NullValidationResult_Generic() {

            // Arrange

            var result = new ExecutionResult<Object>();

            // Act

            var exception = Record.Exception(
                () => result.AddValidationResult(null)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void AddValidationResult_ImmutableAndCombinesResults_NonGeneric() {

            // Arrange

            var validationResultOne = new ValidationResult("Foo");
            var validationResultTwo = new ValidationResult("Bar");
            var original = new ExecutionResult(ImmutableList.Create(validationResultOne));

            // Act

            var updated = original.AddValidationResult(validationResultTwo);

            // Assert

            Assert.NotEqual(original, updated);
            Assert.Contains(validationResultOne, updated.ValidationResults);
            Assert.Contains(validationResultTwo, updated.ValidationResults);
        }

        [Fact]
        public void AddValidationResult_ImmutableAndCombinesResults_Generic() {

            // Arrange

            var validationResultOne = new ValidationResult("Foo");
            var validationResultTwo = new ValidationResult("Bar");
            var original = new ExecutionResult<Object>(ImmutableList.Create(validationResultOne));

            // Act

            var updated = original.AddValidationResult(validationResultTwo);

            // Assert

            Assert.NotEqual(original, updated);
            Assert.Contains(validationResultOne, updated.ValidationResults);
            Assert.Contains(validationResultTwo, updated.ValidationResults);
        }

        [Fact]
        public void AddValidationResults_NullValidationResults_NonGeneric() {

            // Arrange

            var result = ExecutionResult.Success;

            // Act

            var exception = Record.Exception(
                () => result.AddValidationResults(null)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void AddValidationResults_NullValidationResults_Generic() {

            // Arrange

            var result = new ExecutionResult<Object>();

            // Act

            var exception = Record.Exception(
                () => result.AddValidationResults(null)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 0)]
        [InlineData(0, 1)]
        [InlineData(2, 2)]
        public void AddValidationResults_ImmutableAndCombineResults_NonGeneric(
            int initialNumberOfValidationResults,
            int additionalNumberOfValidationResults) {

            // Arrange

            var initialValidationResults =
                this.GenerateValidationResults(initialNumberOfValidationResults)
                    .ToImmutableList();

            var additionalValidationResults =
                this.GenerateValidationResults(additionalNumberOfValidationResults)
                    .ToImmutableList();

            var initial = new ExecutionResult(initialValidationResults);

            // Act

            var updated = initial.AddValidationResults(additionalValidationResults);

            // Assert

            Assert.Equal(
                initialNumberOfValidationResults + additionalNumberOfValidationResults,
                updated.ValidationResults.Count
            );

            foreach (var validationResult in initialValidationResults) {
                Assert.Contains(validationResult, updated.ValidationResults);
            }

            foreach (var validationResult in additionalValidationResults) {
                Assert.Contains(validationResult, updated.ValidationResults);
            }
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 0)]
        [InlineData(0, 1)]
        [InlineData(2, 2)]
        public void AddValidationResults_ImmutableAndCombineResults_Generic(
            int initialNumberOfValidationResults,
            int additionalNumberOfValidationResults) {

            // Arrange

            var initialValidationResults =
                this.GenerateValidationResults(initialNumberOfValidationResults)
                    .ToImmutableList();

            var additionalValidationResults =
                this.GenerateValidationResults(additionalNumberOfValidationResults)
                    .ToImmutableList();

            var initial = new ExecutionResult<Object>(initialValidationResults);

            // Act

            var updated = initial.AddValidationResults(additionalValidationResults);

            // Assert

            Assert.Equal(
                initialNumberOfValidationResults + additionalNumberOfValidationResults,
                updated.ValidationResults.Count
            );

            foreach (var validationResult in initialValidationResults) {
                Assert.Contains(validationResult, updated.ValidationResults);
            }

            foreach (var validationResult in additionalValidationResults) {
                Assert.Contains(validationResult, updated.ValidationResults);
            }
        }

        private IEnumerable<ValidationResult> GenerateValidationResults(int numberOfResults) {
            foreach (var _ in Enumerable.Range(0, numberOfResults)) {
                yield return new ValidationResult(Guid.NewGuid().ToString());
            }
        }

    }

}
