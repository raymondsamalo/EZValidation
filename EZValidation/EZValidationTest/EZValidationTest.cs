using System;
using Xunit;
using EZ.Validation;
namespace EZValidationTest
{

    public enum EnumTest
    {
        valid=0,
        invalid=1
    };
    public class TestTarget
    {
        public EnumTest Et { get; set; }
    }
    public class ValidatorTest
    {

        [Fact]
        public void EnumTest()
        {
            var validator = new Validator<TestTarget>();
            validator.AddRuleFor(x => x.Et).IsDefined.WithMessage("Not Defined");
           var result= validator.Validate(new TestTarget { Et = (EnumTest)(-1) });
            Console.WriteLine($" {result.ErrorMessages.Keys}");
            Console.WriteLine($" {result.ErrorMessages.Values}");
        }
    }
}
