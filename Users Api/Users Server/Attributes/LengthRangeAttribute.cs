using System.ComponentModel.DataAnnotations;

namespace Users_Server.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class LengthRangeAttribute : ValidationAttribute
    {
        private readonly int _minLength;
        private readonly int _maxLength;

        public LengthRangeAttribute(int minLength, int maxLength)
        {
            _minLength = minLength;
            _maxLength = maxLength;
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string item)
            {
                int length = item.Length;
                if (length < _minLength || length > _maxLength)
                {
                    ErrorMessage = string.Format(ErrorMessage!, validationContext.DisplayName, _minLength, _maxLength);
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success!;
        }
    }
}
