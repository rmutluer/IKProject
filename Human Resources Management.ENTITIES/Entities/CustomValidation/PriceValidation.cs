using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Human_Resources_Management.ENTITIES.Entities.CustomValidation
{
    public class PriceValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            decimal deger = (decimal)value;
            if (deger < 0)
                return new ValidationResult("Değer 0'dan Küçük olamaz!");
            return ValidationResult.Success;
        }
    }
}
