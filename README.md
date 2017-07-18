# Invio.Validation

[![Appveyor](https://ci.appveyor.com/api/projects/status/2ec64xak3uivp2ln/branch/master?svg=true)](https://ci.appveyor.com/project/invio/invio-validation/branch/master)
[![Travis CI](https://img.shields.io/travis/invio/Invio.Validation.svg?maxAge=3600&label=travis)](https://travis-ci.org/invio/Invio.Validation)
[![NuGet](https://img.shields.io/nuget/v/Invio.Validation.svg)](https://www.nuget.org/packages/Invio.Validation/)
[![Coverage](https://coveralls.io/repos/github/invio/Invio.Validation/badge.svg?branch=master)](https://coveralls.io/github/invio/Invio.Validation?branch=master)

Invio's implementations to help with running validations within dotnet.

# Installation
The latest version of this package is available on NuGet. To install, run the following command:

```
PM> Install-Package Invio.Validation
```

# Noteworthy Features
Features included here are what makes this library useful

## ExecutionResult

These classes are intended to contain the result of an execution and/or any validation failures that have occurred instead of the execution occurring. `ExecutionResult` is immutable to allow for ease during asynchronous programming.

The non-generic form of `ExecutionResult` is used to capture validation failures if any. It will only hold the validation results if any.

```csharp
// Simple implementationType example
using System;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using Invio.Validation;

private IList<ValidationResult> Validate(String metadata) {
    var result = ImmutableList<ValidationResult>.Empty;
    if (!String.IsNullOrWhiteSpace(metadata)) {
        result = result.Add(
            new ValidationResult(
                $"The {nameof(metadata)} should be null or whitespace since no metadata "+
                $"is expected.",
                ImmutableList.Create(
                    nameof(metadata)
                )
            )
        );
    }

    return result;
}

public ExecutionResult Execute(String metadata) {
    var results = this.Validate(metadata);

    if (results.Any()) {
        return new ExecutionResult(results);
    }

    return ExecutionResult.Success;
}
```

This generic form of `ExecutionResult<T>` is used when a return value can be expected when successful.

```csharp
// Simple implementationType example
using System;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using Invio.Validation;

private IList<ValidationResult> Validate(String metadata) {
    var result = ImmutableList<ValidationResult>.Empty;
    if (!String.IsNullOrWhiteSpace(metadata)) {
        result = result.Add(
            new ValidationResult(
                $"The {nameof(metadata)} should be null or whitespace since no metadata "+
                $"is expected.",
                ImmutableList.Create(
                    nameof(metadata)
                )
            )
        );
    }

    return result;
}

public ExecutionResult<String> Execute(String metadata) {
    var results = this.Validate(metadata);

    if (results.Any()) {
        return new ExecutionResult<String>(results);
    }

    // Returns the empty string as a result for this function.
    return new ExecutionResult<String>(String.Empty);
}
```
