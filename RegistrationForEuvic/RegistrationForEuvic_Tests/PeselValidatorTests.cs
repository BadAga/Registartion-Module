using System.ComponentModel.DataAnnotations;
using RegistrationForEuvic.Models.Validators;

namespace RegistrationForEuvic_Test
{
    public class PeselValidatorTests
    {
        [Fact]
        public void PeselValid_WrongLength_LengthShouldBe11()
        {
            PeselValid peselValid = new PeselValid();

            string testedPesel1= "1234567891655656565";
            string testedPesel2 = "123456";
            string testedPesel3 = "68082095213";

            bool result = peselValid.IsValid(testedPesel1);
            bool result2 = peselValid.IsValid(testedPesel2);
            bool result3 = peselValid.IsValid(testedPesel3);

            Assert.False(result);
            Assert.False(result2);
            Assert.True(result3);
        }

        [Fact]
        public void PeselValid_WrongCharacters_CharacterstMustBeOnlyDigits()
        {
            PeselValid peselValid = new PeselValid();

            string testedPesel1 = "123456k7896";
            string testedPesel2 = "126  6a7891";
            string testedPesel3 = "123/?6a7891";
            string testedPesel4 = "85120449896";

            bool result = peselValid.IsValid(testedPesel1);
            bool result2 = peselValid.IsValid(testedPesel2);
            bool result3= peselValid.IsValid(testedPesel3);
            bool result4 = peselValid.IsValid(testedPesel4);

            Assert.False(result);
            Assert.False(result2);
            Assert.False(result3);
            Assert.True(result4);
        }

        [Fact]
        public void PeselValid_InocrrectControlNumber_ShouldReturnFalse()
        {
            PeselValid peselValid = new PeselValid();
            string testedPesel1 = "85091029933";
            string testedPesel2 = "99031769287";
            string testedPesel3 = "49080193781";
            string testedPesel4 = "72040747634";

            bool result = peselValid.IsValid(testedPesel1);
            bool result2 = peselValid.IsValid(testedPesel2);
            bool result3 = peselValid.IsValid(testedPesel3);
            bool result4 = peselValid.IsValid(testedPesel4);

            Assert.False(result);
            Assert.False(result2);
            Assert.False(result3);
            Assert.True(result4);
        }

        [Fact]
        public void PeselValid_ErrorMessages_ShouldBeAdequate()
        {
            PeselValid peselValid = new PeselValid();
            string testedPesel1 = "85091029933";
            string testedPesel2 = "123/?6a7891";
            string testedPesel3 = "123456";

            ValidationContext validationContext = new ValidationContext(testedPesel1);
            ValidationResult validationResult = peselValid.GetValidationResult(testedPesel1,validationContext);

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
