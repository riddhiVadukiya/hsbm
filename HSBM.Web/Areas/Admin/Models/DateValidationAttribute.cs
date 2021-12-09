using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace HSBM.Web.Areas.Admin.Models
{

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class DateValidationAttribute : ValidationAttribute
    {
        private string _defaultErrorMessage;
        private string _propertyNameToCompare;

        public DateValidationAttribute(string message, string compareWith = "")
        {
            _propertyNameToCompare = compareWith;
            _defaultErrorMessage = message;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var baseProperyInfo = validationContext.ObjectType.GetProperty(_propertyNameToCompare);
            var startDate = (DateTime)baseProperyInfo.GetValue(validationContext.ObjectInstance, null);
            if (value != null)
            {
                DateTime enteredDate = (DateTime)value;
                if (enteredDate <= startDate)
                {
                    return new ValidationResult(_defaultErrorMessage);
                }
            }
            return null;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            //string errorMessage = this.FormatErrorMessage(metadata.DisplayName);
            string errorMessage = ErrorMessageString;

            // The value we set here are needed by the jQuery adapter
            ModelClientValidationRule dateGreaterThanRule = new ModelClientValidationRule();
            dateGreaterThanRule.ErrorMessage = errorMessage;
            dateGreaterThanRule.ValidationType = "dategreaterthan"; // This is the name the jQuery adapter will use
            //"otherpropertyname" is the name of the jQuery parameter for the adapter, must be LOWERCASE!
            dateGreaterThanRule.ValidationParameters.Add("otherpropertyname", _propertyNameToCompare);

            yield return dateGreaterThanRule;
        }

    }

}