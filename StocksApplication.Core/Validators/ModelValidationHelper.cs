using System.ComponentModel.DataAnnotations;

namespace StocksApplication.Core.Validators
{
    public class ModelValidationHelper
    {
        public ModelValidationHelper() { }

        internal static void Validate(object model)
        {
            var validationContext = new ValidationContext(model);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

            if (!isValid) throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
        }
    }
}
