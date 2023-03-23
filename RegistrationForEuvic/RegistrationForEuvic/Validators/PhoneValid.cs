using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace RegistrationForEuvic.Validators
{
    public class PhoneValid:ValidationAttribute
    {
        //based on https://en.wikipedia.org/wiki/Telephone_numbers_in_Poland
        private readonly Regex regex;

        public PhoneValid()
        {
            this.regex = new Regex(@"\b(([0-9]{9})|(([0-9]{3}[\s-]){2}([0-9]{3}))|(([0-9]{2}[\s-])([0-9]{3}[\s-])([0-9]{2}[\s-])([0-9]{2})))\b");
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            ValidationResult result = ValidationResult.Success;

            if (value == null)
            {
                result= new ValidationResult("Empty phone number value");
            }

            string phoneNumber=(string)value;   

            if(!PhoneIsValid(ref phoneNumber))
            {
                result= new ValidationResult("Invalid phone number");
            }
            return result;
        }

        private bool PhoneIsValid(ref string phone_number)
        {
            bool result = false;
            char separator = ' ';

            if(phone_number.Any(x=>x=='-'))
            {
                separator = '-';
            }

            string[] chars = phone_number.Split(separator);
            string justNumbers=String.Join("", chars);

            if(justNumbers.Length!=9)
            {
                return false;
            }
            try
            {
                result=regex.IsMatch(phone_number);
            }
            catch(RegexMatchTimeoutException e)
            {
                return false;
            }
            return result;
        }
        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture,
            ErrorMessageString, name);
        }
    }
}
