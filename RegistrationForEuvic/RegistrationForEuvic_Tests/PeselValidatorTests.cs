using System.ComponentModel.DataAnnotations;
using RegistrationForEuvic.Validators;

namespace RegistrationForEuvic_Test
{
    public class PeselValidatorTests
    {
        [Fact]
        public void PeselValid_WrongLength_LengthShouldBe11()
        {
            PeselValid peselValid = new PeselValid();
            bool[] testCases = {peselValid.IsValid("1234567891655656565"),
                                peselValid.IsValid("123456"),

                                peselValid.IsValid("68082095213")};

            bool[] expectedResults = { false, false, true };
            Assert.Equal(expectedResults, testCases);
        }

        [Fact]
        public void PeselValid_WrongCharacters_CharacterstMustBeOnlyDigits()
        {
            PeselValid peselValid = new PeselValid();
            bool[] testCases = {peselValid.IsValid("123456k7896"),
                                peselValid.IsValid("126  6a7891"),
                                peselValid.IsValid("123/?6a7891"),

                                peselValid.IsValid("85120449896")};

            bool[] expectedResults = { false, false, false, true };
            Assert.Equal(expectedResults, testCases);
        }

        [Fact]
        public void PeselValid_InocrrectControlNumber_ShouldReturnFalse()
        {
            PeselValid peselValid = new PeselValid();

            bool[] testCases = {peselValid.IsValid("85091029933"),
                                peselValid.IsValid("99031769287"),
                                peselValid.IsValid("49080193781"),

                                peselValid.IsValid("72040747634")};

            bool[] expectedResults = { false, false, false, true };
            Assert.Equal(expectedResults, testCases);
        }

        [Fact]
        public void PeselValid_ErrorMessages_ShouldBeAdequate()
        {
            PeselValid peselValid = new PeselValid();
            string testedPesel1 = "85091029933";
            string testedPesel2 = "123/?6a7891";
            string testedPesel3 = "123456";

            ValidationContext validationContext = new ValidationContext(testedPesel1);
            ValidationResult validationResult = peselValid.GetValidationResult(testedPesel1, validationContext);

            Assert.Equal("Incorrect PESEL", validationResult.ErrorMessage);


            validationContext = new ValidationContext(testedPesel2);
            validationResult = peselValid.GetValidationResult(testedPesel2, validationContext);
            Assert.Equal("PESEL contains only numbers", validationResult.ErrorMessage);


            validationContext = new ValidationContext(testedPesel3);
            validationResult = peselValid.GetValidationResult(testedPesel3, validationContext);
            Assert.Equal("Incorrect PESEL length", validationResult.ErrorMessage);
        }
    }
}
