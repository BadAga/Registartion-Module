using Microsoft.Identity.Client;

namespace RegistrationForEuvic.Managers
{
    public class UserInputManager
    {
        /// <summary>
        /// Formats passed value to a number with 3 decimal precision
        /// </summary>
        /// <param name="userInput">user input with avg power usage </param>
        /// <returns> formatted user input</returns>
        public static double? FormatPowerUsage(double? userInput)
        {
            double formated = 0.0;
            try
            {
                decimal temp = new decimal((double)userInput);
                temp = decimal.Round(temp, 3);
                formated = (double)temp;
            }
            catch (Exception e)
            {
                return null;
            }
            return formated;
        }

        /// <summary>
        /// Returns the age of person based on their PESEL number
        /// based on: https://www.gov.pl/web/cyfryzacja/co-to-jest-numer-pesel-i-jak-sie-go-nadaje
        /// </summary>
        /// <param name="peselNumber">valid PESEL number value</param>
        /// <returns>age</returns>
        public static int GetAgeBasedOnPesel(string peselNumber)
        {
            int birthYear = (peselNumber[0] - '0') * 10 + peselNumber[1] - '0';
            int birthMonth = (peselNumber[2] - '0') * 10 + peselNumber[3] - '0';
            int birthDay = (peselNumber[4] - '0') * 10 + peselNumber[5] - '0';
            if (birthMonth > 12)//this means that the personwasporn after 2000
            {
                birthYear += 2000;
                birthMonth -= 20;
            }
            else
            {
                birthYear += 1900;
            }
            int currentYear = DateTime.Now.Year;
            int currentMonth = DateTime.Now.Month;
            int currentDay = DateTime.Now.Day;
            //are thay before their birthday?
            if (currentMonth < birthMonth ||
                currentMonth == birthMonth && currentDay < birthDay)
            {
                return currentYear - birthYear - 1;
            }
            return currentYear - birthYear;
        }

        /// <summary>
        /// Deletes all separators from phone number.
        /// Assumption: each number have ONLYONE type of separator.
        /// </summary>
        /// <param name="phoneNumber">phone number with no separators</param>
        /// <param name="separtors">array of separtors</param>
        /// <returns>phone number with no separators</returns>
        public static string FormatToNoSepartorNumber(string phoneNumber, params char[] separtors)
        {
            string noSparators = string.Empty;
            char? existingSeparator = null;

            foreach (char separator in separtors)
            {
                if (phoneNumber.Any(x => x == separator))
                {
                    existingSeparator = separator;
                    break;
                }
            }
            if (existingSeparator == null)
            {
                return phoneNumber;
            }

            string[] subNumbers = phoneNumber.Split((char)existingSeparator);
            noSparators = string.Join("", subNumbers);

            return noSparators;
        }

    }
}
