using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Globalization;

namespace RegistrationForEuvic.Models.Validators
{
    public class PeselValid:ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value,ValidationContext validationContext)
        {
            ValidationResult result = ValidationResult.Success;
            
            if(value==null)
            {
                FormatErrorMessage("Empty PESEL value");
                result = new ValidationResult("Empty PESEL value");
            }

            String pesel = (String)value;

            if (pesel.Length!=11)
            {
                FormatErrorMessage("Incorrect PESEL length");
                result = new ValidationResult("Incorrect PESEL length");
            }
            else if(!pesel.All(Char.IsDigit))
            {
                result = new ValidationResult("PESEL contains only numbers");
            }
            else if(!CheckControlNumber(ref pesel))
            {
                result = new ValidationResult("Incorrect PESEL");
            }
            return result;
        }

        /// <summary>
        /// Checks if control number (last digit) is valid
        /// more here: https://pl.wikipedia.org/wiki/PESEL
        /// </summary>
        /// <param name="pesel">PESEL number</param>
        /// <returns> true if control number is valid; false otherwise </returns>
        private bool CheckControlNumber(ref String pesel)
        {
            //product of sum of digits that have weight==1
            int productWithWeight1 = (pesel.ElementAt(0)- '0') +
                                    (pesel.ElementAt(4) - '0') +
                                    (pesel.ElementAt(8) - '0') +
                                    (pesel.ElementAt(10) - '0');
            //product of sum of digits that have weight==3
            int productWithWeight3 = ((pesel.ElementAt(1) - '0') +
                                      (pesel.ElementAt(5) - '0') +
                                      (pesel.ElementAt(9) - '0'))*3;
            //product of sum of digits that have weight==7
            int productWithWeight7 = ((pesel.ElementAt(2) - '0') +
                                      (pesel.ElementAt(6) - '0')) * 7;
            //product of sum of digits that have weight==9
            int productWithWeight9 = ((pesel.ElementAt(3) - '0') +
                                      (pesel.ElementAt(7)- '0')) * 9;

            int sValue=productWithWeight1 + productWithWeight3+productWithWeight7+productWithWeight9;

            return (sValue%10)==0;

        }
        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture,
            ErrorMessageString, name);
        }
    }
}
