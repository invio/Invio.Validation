using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Invio.Xunit;
using Xunit;

namespace Invio.Validation {

    [UnitTest]
    public class OperationResultTests {

        [Fact]
        public void Constructor_NullValidationResults_NonGeneric() {

            // Act

            var exception = Record.Exception(
                () => new OperationResult(null)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void Constructor_NullValidationResults_Generic() {

            // Act

            var exception = Record.Exception(
                () => new OperationResult<Int32>((ISet<ValidationIssue>) null)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void Success_IsSuccessful_NonGeneric() {
            Assert.NotNull(OperationResult.Success);
            Assert.True(OperationResult.Success.IsSuccessful);

            Assert.NotNull(OperationResult.Success.ValidationIssues);
            Assert.Empty(OperationResult.Success.ValidationIssues);
        }

        [Fact]
        public void Success_IsSuccessful_Generic() {
            var result = new OperationResult<Object>();
            Assert.True(result.IsSuccessful);
            Assert.Equal(default(Object), result.Value);

            Assert.NotNull(OperationResult.Success.ValidationIssues);
            Assert.Empty(OperationResult.Success.ValidationIssues);
        }

        [Fact]
        public void IsSuccessful_TrueWithoutResults_NonGeneric() {

            // Arrange

            var validationIssues = ImmutableHashSet<ValidationIssue>.Empty;

            // Act

            var result = new OperationResult(validationIssues);

            // Assert

            Assert.True(result.IsSuccessful);
            Assert.Empty(result.ValidationIssues);
        }

        [Fact]
        public void IsSuccessful_TrueWithoutResults_Generic() {

            // Arrange

            var validationIssues = ImmutableHashSet<ValidationIssue>.Empty;

            // Act

            var result = new OperationResult<Object>(validationIssues);

            // Assert

            Assert.True(result.IsSuccessful);
            Assert.Empty(result.ValidationIssues);
            Assert.Equal(default(Object), result.Value);
        }

        [Fact]
        public void IsSuccessful_TrueWithWarnings_NonGeneric() {

            // Arrange

            const string message = "WarnFoo";
            var validationIssues = ImmutableHashSet.Create(ValidationIssue.Warning(message));

            // Act

            var result = new OperationResult(validationIssues);

            // Assert

            Assert.True(result.IsSuccessful);
            Assert.NotEmpty(result.ValidationIssues);
        }

        [Fact]
        public void IsSuccessful_TrueWithWarnings_Generic() {

            // Arrange

            const string message = "WarnFoo";
            var validationIssues = ImmutableHashSet.Create(ValidationIssue.Warning(message));

            // Act

            var result = new OperationResult<Object>(validationIssues);

            // Assert

            Assert.True(result.IsSuccessful);
            Assert.NotEmpty(result.ValidationIssues);
            Assert.Equal(default(Object), result.Value);
        }

        [Fact]
        public void IsSuccessful_TrueWithResult_Generic() {

            // Arrange

            var realResult = new Object();

            // Act

            var result = new OperationResult<Object>(realResult);

            // Assert

            Assert.True(result.IsSuccessful);
            Assert.Empty(result.ValidationIssues);
            Assert.Equal(realResult, result.Value);
        }

        [Fact]
        public void IsSuccessful_FalseWithResults_NonGeneric() {

            // Arrange

            const string message = "Foo";
            var validationIssues = ImmutableHashSet.Create(ValidationIssue.Error(message));

            // Act

            var result = new OperationResult(validationIssues);

            // Assert

            Assert.False(result.IsSuccessful);
            var validationResult = Assert.Single(result.ValidationIssues);
            Assert.Equal(message, validationResult.Message);
        }

        [Fact]
        public void IsSuccessful_FalseWithResults_Generic() {

            // Arrange

            const string message = "Foo";
            var validationIssues = ImmutableHashSet.Create(ValidationIssue.Error(message));

            // Act

            var result = new OperationResult<Object>(validationIssues);

            // Assert

            Assert.False(result.IsSuccessful);
            var validationIssue = Assert.Single(result.ValidationIssues);
            Assert.Equal(message, validationIssue.Message);
            Assert.Equal(default(Object), result.Value);
        }

        [Fact]
        public void AddValidationIssue_NullValidationResult_NonGeneric() {

            // Arrange

            var result = OperationResult.Success;

            // Act

            var exception = Record.Exception(
                () => result.AddValidationIssue(null)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void AddValidationIssue_NullValidationResult_Generic() {

            // Arrange

            var result = new OperationResult<Object>();

            // Act

            var exception = Record.Exception(
                () => result.AddValidationIssue(null)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void AddValidationIssue_ImmutableAndCombinesResults_NonGeneric() {

            // Arrange

            var validationIssueOne = ValidationIssue.Error("Foo");
            var validationIssueTwo = ValidationIssue.Error("Bar");
            var original = new OperationResult(ImmutableHashSet.Create(validationIssueOne));

            // Act

            var updated = original.AddValidationIssue(validationIssueTwo);

            // Assert

            Assert.NotEqual(original, updated);
            Assert.Contains(validationIssueOne, updated.ValidationIssues);
            Assert.Contains(validationIssueTwo, updated.ValidationIssues);
        }

        [Fact]
        public void AddValidationIssue_ImmutableAndCombinesResults_Generic() {

            // Arrange

            var validationIssueOne = ValidationIssue.Error("Foo");
            var validationIssueTwo = ValidationIssue.Error("Bar");
            var original = new OperationResult<Object>(ImmutableHashSet.Create(validationIssueOne));

            // Act

            var updated = original.AddValidationIssue(validationIssueTwo);

            // Assert

            Assert.NotEqual(original, updated);
            Assert.Contains(validationIssueOne, updated.ValidationIssues);
            Assert.Contains(validationIssueTwo, updated.ValidationIssues);
        }

        [Fact]
        public void AddValidationIssues_NullValidationResults_NonGeneric() {

            // Arrange

            var result = OperationResult.Success;

            // Act

            var exception = Record.Exception(
                () => result.AddValidationIssues(null)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void AddValidationIssues_NullValidationResults_Generic() {

            // Arrange

            var result = new OperationResult<Object>();

            // Act

            var exception = Record.Exception(
                () => result.AddValidationIssues(null)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 0)]
        [InlineData(0, 1)]
        [InlineData(2, 2)]
        public void AddValidationIssues_ImmutableAndCombineResults_NonGeneric(
            int initialNumberOfValidationResults,
            int additionalNumberOfValidationResults) {

            // Arrange

            var initialValidationIssues =
                this.GenerateValidationIssues(initialNumberOfValidationResults)
                    .ToImmutableHashSet();

            var additionalValidationIssues =
                this.GenerateValidationIssues(additionalNumberOfValidationResults)
                    .ToImmutableHashSet();

            var initial = new OperationResult(initialValidationIssues);

            // Act

            var updated = initial.AddValidationIssues(additionalValidationIssues);

            // Assert

            Assert.Equal(
                initialNumberOfValidationResults + additionalNumberOfValidationResults,
                updated.ValidationIssues.Count
            );

            foreach (var validationIssue in initialValidationIssues) {
                Assert.Contains(validationIssue, updated.ValidationIssues);
            }

            foreach (var validationIssue in additionalValidationIssues) {
                Assert.Contains(validationIssue, updated.ValidationIssues);
                Assert.DoesNotContain(validationIssue, initial.ValidationIssues);
            }
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 0)]
        [InlineData(0, 1)]
        [InlineData(2, 2)]
        public void AddValidationResults_ImmutableAndCombineResults_Generic(
            int initialNumberOfValidationIssues,
            int additionalNumberOfValidationIssues) {

            // Arrange

            var initialValidationIssues =
                this.GenerateValidationIssues(initialNumberOfValidationIssues)
                    .ToImmutableHashSet();

            var additionalValidationIssues =
                this.GenerateValidationIssues(additionalNumberOfValidationIssues)
                    .ToImmutableHashSet();

            var initial = new OperationResult<Object>(initialValidationIssues);

            // Act

            var updated = initial.AddValidationIssues(additionalValidationIssues);

            // Assert

            Assert.Equal(
                initialNumberOfValidationIssues + additionalNumberOfValidationIssues,
                updated.ValidationIssues.Count
            );

            foreach (var validationIssue in initialValidationIssues) {
                Assert.Contains(validationIssue, updated.ValidationIssues);
            }

            foreach (var validationIssue in additionalValidationIssues) {
                Assert.Contains(validationIssue, updated.ValidationIssues);
                Assert.DoesNotContain(validationIssue, initial.ValidationIssues);
            }
        }

        [Fact]
        public void AddWarning_NullMessage_NonGeneric() {

            // Arrange

            var initial = OperationResult.Success;

            // Act

            var exception = Record.Exception(
                () => initial.AddWarning(null)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);

        }

        [Fact]
        public void AddWarning_NullMessage_Generic() {

            // Arrange

            var initial = new OperationResult<Object>();

            // Act

            var exception = Record.Exception(
                () => initial.AddWarning(null)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);

        }

        [Fact]
        public void AddWarning_WhiteSpaceMessage_NonGeneric() {

            // Arrange

            var initial = OperationResult.Success;

            // Act

            var exception = Record.Exception(
                () => initial.AddWarning(" ")
            );

            // Assert

            Assert.IsType<ArgumentException>(exception);

        }

        [Fact]
        public void AddWarning_WhiteSpaceMessage_Generic() {

            // Arrange

            var initial = new OperationResult<Object>();

            // Act

            var exception = Record.Exception(
                () => initial.AddWarning(" ")
            );

            // Assert

            Assert.IsType<ArgumentException>(exception);

        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void AddWarning_VaryingSets_NonGeneric(
            int initialNumberOfValidationIssues) {

            // Arrange

            var initialValidationIssues =
                this.GenerateValidationIssues(initialNumberOfValidationIssues)
                    .ToImmutableHashSet();

            var message = "FooBar";
            var initial = new OperationResult(initialValidationIssues);

            // Act

            var updated = initial.AddWarning(message);

            // Assert

            var issue = ValidationIssue.Warning(message);
            Assert.Contains(issue, updated.ValidationIssues);
            Assert.DoesNotContain(issue, initial.ValidationIssues);

        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void AddWarning_VaryingSets_Generic(
            int initialNumberOfValidationIssues) {

            // Arrange

            var initialValidationIssues =
                this.GenerateValidationIssues(initialNumberOfValidationIssues)
                    .ToImmutableHashSet();

            var message = "FooBar";
            var initial = new OperationResult<Object>(initialValidationIssues);

            // Act

            var updated = initial.AddWarning(message);

            // Assert

            var issue = ValidationIssue.Warning(message);
            Assert.Contains(issue, updated.ValidationIssues);
            Assert.DoesNotContain(issue, initial.ValidationIssues);

        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void AddWarning_VaryingSetsWithMembers_NonGeneric(
            int initialNumberOfValidationIssues) {

            // Arrange

            var initialValidationIssues =
                this.GenerateValidationIssues(initialNumberOfValidationIssues)
                    .ToImmutableHashSet();

            var message = "FooBar";
            var memberNames = ImmutableHashSet.Create("Foo", "Bar");
            var initial = new OperationResult(initialValidationIssues);

            // Act

            var updated = initial.AddWarning(message, memberNames);

            // Assert

            var issue = ValidationIssue.Warning(message, memberNames);
            Assert.Contains(issue, updated.ValidationIssues);
            Assert.DoesNotContain(issue, initial.ValidationIssues);

        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void AddWarning_VaryingSetsWithMembers_Generic(
            int initialNumberOfValidationIssues) {

            // Arrange

            var initialValidationIssues =
                this.GenerateValidationIssues(initialNumberOfValidationIssues)
                    .ToImmutableHashSet();

            var message = "FooBar";
            var memberNames = ImmutableHashSet.Create("Foo", "Bar");
            var initial = new OperationResult<Object>(initialValidationIssues);

            // Act

            var updated = initial.AddWarning(message, memberNames);

            // Assert

            var issue = ValidationIssue.Warning(message, memberNames);
            Assert.Contains(issue, updated.ValidationIssues);
            Assert.DoesNotContain(issue, initial.ValidationIssues);

        }

        [Fact]
        public void AddError_NullMessage_NonGeneric() {

            // Arrange

            var initial = OperationResult.Success;

            // Act

            var exception = Record.Exception(
                () => initial.AddError(null)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);

        }

        [Fact]
        public void AddError_NullMessage_Generic() {

            // Arrange

            var initial = new OperationResult<Object>();

            // Act

            var exception = Record.Exception(
                () => initial.AddError(null)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);

        }

        [Fact]
        public void AddError_WhiteSpaceMessage_NonGeneric() {

            // Arrange

            var initial = OperationResult.Success;

            // Act

            var exception = Record.Exception(
                () => initial.AddError(" ")
            );

            // Assert

            Assert.IsType<ArgumentException>(exception);

        }

        [Fact]
        public void AddError_WhiteSpaceMessage_Generic() {

            // Arrange

            var initial = new OperationResult<Object>();

            // Act

            var exception = Record.Exception(
                () => initial.AddError(" ")
            );

            // Assert

            Assert.IsType<ArgumentException>(exception);

        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void AddError_VaryingSets_NonGeneric(
            int initialNumberOfValidationIssues) {

            // Arrange

            var initialValidationIssues =
                this.GenerateValidationIssues(initialNumberOfValidationIssues)
                    .ToImmutableHashSet();

            var message = "FooBar";
            var initial = new OperationResult(initialValidationIssues);

            // Act

            var updated = initial.AddError(message);

            // Assert

            var issue = ValidationIssue.Error(message);
            Assert.Contains(issue, updated.ValidationIssues);
            Assert.DoesNotContain(issue, initial.ValidationIssues);

        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void AddError_VaryingSets_Generic(
            int initialNumberOfValidationIssues) {

            // Arrange

            var initialValidationIssues =
                this.GenerateValidationIssues(initialNumberOfValidationIssues)
                    .ToImmutableHashSet();

            var message = "FooBar";
            var initial = new OperationResult<Object>(initialValidationIssues);

            // Act

            var updated = initial.AddError(message);

            // Assert

            var issue = ValidationIssue.Error(message);
            Assert.Contains(issue, updated.ValidationIssues);
            Assert.DoesNotContain(issue, initial.ValidationIssues);

        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void AddError_VaryingSetsWithMembers_NonGeneric(
            int initialNumberOfValidationIssues) {

            // Arrange

            var initialValidationIssues =
                this.GenerateValidationIssues(initialNumberOfValidationIssues)
                    .ToImmutableHashSet();

            var message = "FooBar";
            var memberNames = ImmutableHashSet.Create("Foo", "Bar");
            var initial = new OperationResult(initialValidationIssues);

            // Act

            var updated = initial.AddError(message, memberNames);

            // Assert

            var issue = ValidationIssue.Error(message, memberNames);
            Assert.Contains(issue, updated.ValidationIssues);
            Assert.DoesNotContain(issue, initial.ValidationIssues);

        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void AddError_VaryingSetsWithMembers_Generic(
            int initialNumberOfValidationIssues) {

            // Arrange

            var initialValidationIssues =
                this.GenerateValidationIssues(initialNumberOfValidationIssues)
                    .ToImmutableHashSet();

            var message = "FooBar";
            var memberNames = ImmutableHashSet.Create("Foo", "Bar");
            var initial = new OperationResult<Object>(initialValidationIssues);

            // Act

            var updated = initial.AddError(message, memberNames);

            // Assert

            var issue = ValidationIssue.Error(message, memberNames);
            Assert.Contains(issue, updated.ValidationIssues);
            Assert.DoesNotContain(issue, initial.ValidationIssues);

        }

        private IEnumerable<ValidationIssue> GenerateValidationIssues(int numberOfIssues) {
            foreach (var _ in Enumerable.Range(0, numberOfIssues)) {
                yield return ValidationIssue.Error(Guid.NewGuid().ToString());
            }
        }

    }

}
