using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using RegistrationForEuvic.Models.Validators;

namespace RegistrationForEuvic_Test
{
    public class PhoneValidatorTest
    {
        [Fact]
        public void PhoneValid_IncorrectNumberOfDigits_MustBeOnly9()
        {
            PhoneValid phoneValidator = new PhoneValid();
            bool[] testCasesResults = {phoneValidator.IsValid("12345667899"),
                                phoneValidator.IsValid("123 456 7899"),
                                phoneValidator.IsValid("123 456 55"),

                                phoneValidator.IsValid("123 456 556"),
                                phoneValidator.IsValid("12 456 55 62"),
                                phoneValidator.IsValid("123-456-556"),
                                phoneValidator.IsValid("12-456-55-62"),
                                phoneValidator.IsValid("666456963")};

            bool[] expectedResults = {false,false,false,
                                      true,true,true,true,true};

            Assert.Equal(expectedResults, testCasesResults);
        }

        [Fact]
        public void PhoneValid_CharacterValue_ShouldOnlyBeDigitsWithOptionalSeperator()
        {
            PhoneValid phoneValidator =new PhoneValid();

            bool[] testCasesResults = {phoneValidator.IsValid("12345698k"),
                                phoneValidator.IsValid("12 345 69 8k"),
                                phoneValidator.IsValid("123 456 98k"),
                                phoneValidator.IsValid("12-345-69-8k"),
                                phoneValidator.IsValid("123-456-98k"),

                                phoneValidator.IsValid("123456789"),
                                phoneValidator.IsValid("123 456 789")};

            bool[] expectedResults = {false,false,false,false,false,
                                      true,true};

            Assert.Equal(expectedResults, testCasesResults);
        }

        [Fact]
        public void PhoneValid_SeparatrValue_OnlySpaceOrDash()
        {
            PhoneValid phoneValidator = new PhoneValid();

            bool[] testCasesResults = {phoneValidator.IsValid("123.123.122"),
                                    phoneValidator.IsValid("123/123/122"),
                                    phoneValidator.IsValid("123  123  122"),
                                    phoneValidator.IsValid("123/123/122"),
                                    phoneValidator.IsValid("12.123.56.96"),
                                    phoneValidator.IsValid("12/123/56/96"),
                                    phoneValidator.IsValid("12  123  56  96"),
                                    phoneValidator.IsValid("12,123,56,96"),

                                    phoneValidator.IsValid("123 123 122"),
                                    phoneValidator.IsValid("123-123-122"),
                                    phoneValidator.IsValid("12 123 56 96"),
                                    phoneValidator.IsValid("12-123-56-96") };

            bool[] expectedResults = {false,false,false,false,false,false,false,false,
                                      true,true,true,true};

            Assert.Equal(expectedResults, testCasesResults);
        }

        [Fact]
        public void PhoneValid_CorrecttFormat()
        {
            PhoneValid phoneValidator = new PhoneValid();

            bool[] testCasesResults = {phoneValidator.IsValid("1 2 3 4 5 6 7 8 9"),
                                    phoneValidator.IsValid("12 34 56 78 9"),
                                    phoneValidator.IsValid("1 23 45 67 89"),
                                    phoneValidator.IsValid("1234 56789"),
                                    phoneValidator.IsValid("12345 6789"),
                                    phoneValidator.IsValid("123456 789"),
                                    phoneValidator.IsValid("1234567 89"),
                                    phoneValidator.IsValid("12345678 9"),
                                    phoneValidator.IsValid("12 34 567 89"),
                                    phoneValidator.IsValid("123 45 67 89"),
                                    phoneValidator.IsValid("12 34 56 899"),

                                    phoneValidator.IsValid("123 123 122"),
                                    phoneValidator.IsValid("123-123-122"),
                                    phoneValidator.IsValid("12 123 56 96"),
                                    phoneValidator.IsValid("12-123-56-96") };

            bool[] expectedResults = {false,false,false,false,false,false,false,false,false,false,false,
                                      true,true,true,true};

            Assert.Equal(expectedResults, testCasesResults);
        }
    }
}
