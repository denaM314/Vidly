using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vidly.Models
{
    public class Min18YearsToSignUpForMembership: ValidationAttribute 
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var customer = (Customer)validationContext.ObjectInstance;

            if (customer.MembershipTypeId == MembershipType.Unknown || customer.MembershipTypeId == MembershipType.PayAsYouGo)
                return ValidationResult.Success;

            if (customer.Birthday == null)
                return new ValidationResult("Birthdate is required.");

            if (DateTime.Now.Year - customer.Birthday.Value.Year >= 18)
                return ValidationResult.Success;
            else
                return new ValidationResult("Customer must be at least 18 years old to sign up for membership.");
        }
    }
}