using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RegistrationForEuvic.Validators;

namespace RegistrationForEuvic_Test
{
    public class NameLikeValidatorTests
    {
        [Fact]
        public void NameLikeValues_IncorrectValues_OnlyCorrectShouldPass()
        {
            NameLikeValue nameLikeValue = new NameLikeValue();

            bool[] testCases =
            {
                nameLikeValue.IsValid("Amanda"),
                nameLikeValue.IsValid("Róża"),
                nameLikeValue.IsValid("amanda"),
                nameLikeValue.IsValid("Aman-da"),
                nameLikeValue.IsValid("Robert Jr."),
                nameLikeValue.IsValid("Justine Mary"),

                nameLikeValue.IsValid("Mandy%"),
                nameLikeValue.IsValid("Mand123"),

            };

            bool[] expectedResults = {true, true, true, true, true, true,
                                      false,false};
            Assert.Equal(expectedResults,testCases);
        }
    }
}
