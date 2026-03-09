using FC.Codeflix.Catalog.Domain.Exceptions;

namespace FC.Codeflix.Catalog.Domain.Validation;

public class DomainValidation {
    public static void NotNull(object? value, string fieldName) {
        if (value is null) {
            throw new EntityValidationException($"{fieldName} should not be null");
        }
    }

    public static void NotNullOrEmpty(string? value, string fieldName) {
        if (String.IsNullOrWhiteSpace(value)) {
            throw new EntityValidationException($"{fieldName} should not be empty or null");
        }
    }

    public static void MinLength(string value, int minLength, string fieldName) {
        if (value.Length < minLength) {
            throw new EntityValidationException($"{fieldName} should be at least {minLength} characters long");
        }
    }

    public static void MaxLength(string value, int maxLength, string fieldName) {
        if (value.Length > maxLength) {
            throw new EntityValidationException($"{fieldName} should be less or equal {maxLength} characters long");
        }
    }
}
