﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Human_Resources_Management.ENTITIES.Entities.CustomValidation
{
    public class OlderForThis : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime date = (DateTime)value; if (date > DateTime.Now.AddYears(-100)) { return new ValidationResult("Tarihi kontrol ediniz"); }
            return ValidationResult.Success;
        }
    }
}

