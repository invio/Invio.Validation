namespace Invio.Validation {

    public enum ValidationIssueLevel {

        /// <summary>
        ///   This Validaiton Issue should only be considered a warning level and will not
        ///   block a consumer from attempting to perform the action, but will indicate some
        ///   improvement that could be done by the consumer of the validation issue.
        /// </summary>
        Warning = 1,

        /// <summary>
        ///   This Validaiton Issue should only be considered an error level and will
        ///   block a consumer from attempting to perform the action. This is the common level
        ///   of a validation issues.
        /// </summary>
        Error = 2
    }
}
