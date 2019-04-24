using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Invio.Xunit;
using Xunit;

namespace Invio.Validation {

    [UnitTest]
    public class ValidationIssueTests {

        [Fact]
        public void Warning_NullMessage() {

            // Act

            var exception = Record.Exception(
                () => ValidationIssue.Warning(null)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void Warning_WhiteSpaceMessage() {

            // Act

            var exception = Record.Exception(
                () => ValidationIssue.Warning(" \t")
            );

            // Assert

            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void Warning_WithJustCode() {

            // Arrange

            var message = "FooBar";
            var code = "some-code";

            // Act

            var warning = ValidationIssue.Warning(message, code);

            // Assert

            Assert.Equal(message, warning.Message);
            Assert.Equal(ValidationIssueLevel.Warning, warning.Level);
            Assert.Equal(code, warning.Code);
            Assert.Null(warning.MemberNames);
        }

        [Fact]
        public void Warning_WithJustMemberNames() {

            // Arrange

            var message = "FooBar";
            var memberNames = ImmutableHashSet.Create("foo", "bar");

            // Act

            var warning = ValidationIssue.Warning(message, memberNames);

            // Assert

            Assert.Equal(message, warning.Message);
            Assert.Equal(ValidationIssueLevel.Warning, warning.Level);
            Assert.Null(warning.Code);
            Assert.Equal(memberNames, warning.MemberNames);
        }

        [Fact]
        public void Warning_Valid() {

            // Arrange

            var message = "FooBar";
            var code = "some-code";
            var names = ImmutableHashSet.Create("Foo", "Bar");

            // Act

            var warning = ValidationIssue.Warning(
                message,
                code,
                names
            );

            // Assert

            Assert.Equal(message, warning.Message);
            Assert.Equal(ValidationIssueLevel.Warning, warning.Level);
            Assert.Equal(code, warning.Code);
            Assert.Equal(names, warning.MemberNames);
        }

        [Fact]
        public void Error_NullMessage() {

            // Act

            var exception = Record.Exception(
                () => ValidationIssue.Error(null)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void Error_WhiteSpaceMessage() {

            // Act

            var exception = Record.Exception(
                () => ValidationIssue.Error(" \t")
            );

            // Assert

            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void Error_WithJustCode() {

            // Arrange

            var message = "FooBar";
            var code = "some-code";

            // Act

            var warning = ValidationIssue.Error(message, code);

            // Assert

            Assert.Equal(message, warning.Message);
            Assert.Equal(ValidationIssueLevel.Error, warning.Level);
            Assert.Equal(code, warning.Code);
            Assert.Null(warning.MemberNames);
        }

        [Fact]
        public void Error_WithJustMemberNames() {

            // Arrange

            var message = "FooBar";
            var memberNames = ImmutableHashSet.Create("foo", "bar");

            // Act

            var warning = ValidationIssue.Error(message, memberNames);

            // Assert

            Assert.Equal(message, warning.Message);
            Assert.Equal(ValidationIssueLevel.Error, warning.Level);
            Assert.Null(warning.Code);
            Assert.Equal(memberNames, warning.MemberNames);
        }

        [Fact]
        public void Error_Valid() {

            // Arrange

            var message = "FooBar";
            var code = "bizz buzz barr";
            var names = ImmutableHashSet.Create("Foo", "Bar");

            // Act

            var warning = ValidationIssue.Error(
                message,
                code,
                names
            );

            // Assert

            Assert.Equal(message, warning.Message);
            Assert.Equal(ValidationIssueLevel.Error, warning.Level);
            Assert.Equal(code, warning.Code);
            Assert.Equal(names, warning.MemberNames);
        }

        [Fact]
        public void Constructor_NullMessage() {

            // Act

            var exception = Record.Exception(
                () => new ValidationIssue(null, ValidationIssueLevel.Warning)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void Constructor_WhiteSpaceMessage() {

            // Act

            var exception = Record.Exception(
                () => new ValidationIssue(" \n", ValidationIssueLevel.Warning)
            );

            // Assert

            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void Constructor_Valid() {

            // Arrange

            var message = "FooBar";
            var code = "some-error-code";
            var level = ValidationIssueLevel.Error;
            var names = ImmutableHashSet.Create("Foo", "Bar");

            // Act

            var issue = new ValidationIssue(
                message: message,
                code: code,
                level: level,
                memberNames: names
            );

            // Assert

            Assert.Equal(message, issue.Message);
            Assert.Equal(code, issue.Code);
            Assert.Equal(level, issue.Level);
            Assert.Equal(names, issue.MemberNames);
        }

        [Fact]
        public void SetMessage_NullMessage() {

            // Arrange

            var issue = ValidationIssue.Error("FooBar");

            // Act

            var exception = Record.Exception(
                () => issue.SetMessage(null)
            );

            // Assert

            Assert.IsType<ArgumentNullException>(exception);
        }

        [Fact]
        public void SetMessage_WhiteSpaceMessage() {

            // Arrange

            var issue = ValidationIssue.Error("FooBar");

            // Act

            var exception = Record.Exception(
                () => issue.SetMessage(" \n")
            );

            // Assert

            Assert.IsType<ArgumentException>(exception);
        }

        [Fact]
        public void SetMessage_Valid() {

            // Arrange

            var initial = ValidationIssue.Error("Foo");

            // Act

            var updated = initial.SetMessage("FooBar");

            // Assert

            Assert.Equal("Foo", initial.Message);
            Assert.Equal("FooBar", updated.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("SomeCode")]
        public void SetCode_AllStringsAreValid(string newCode) {

            // Arrange

            var initial = ValidationIssue.Warning("Bar", "Code?");

            // Act

            var updated = initial.SetCode(newCode);

            // Assert

            Assert.Equal(newCode, updated.Code);
        }

        [Fact]
        public void SetLevel_Valid() {

            // Arrange

            var initial = ValidationIssue.Error("Foo");

            // Act

            var updated = initial.SetLevel(ValidationIssueLevel.Warning);

            // Assert

            Assert.Equal(ValidationIssueLevel.Error, initial.Level);
            Assert.Equal(ValidationIssueLevel.Warning, updated.Level);
        }

        [Fact]
        public void SetMemberNames_Valid() {

            // Arrange

            var names = ImmutableHashSet.Create("Foo", "Bar");
            var initial = ValidationIssue.Error("Foo", memberNames: names);

            // Act

            var nextNames = names.Add("Name");
            var updated = initial.SetMemberNames(nextNames);

            // Assert

            Assert.DoesNotContain("Name", initial.MemberNames);
            Assert.Contains("Name", updated.MemberNames);
        }

    }

}
