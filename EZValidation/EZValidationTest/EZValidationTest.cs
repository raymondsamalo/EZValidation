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
        public String name { get; set; }
    }
    public class ValidatorTest
    {

        [Fact]
        public void CheckEnumTest()
        {
            var validator = new Validator<TestTarget>();
            validator.BeginRuleFor(x => x.name)
                     .NotNullOrEmpty
                     .End();
            validator.BeginRuleFor(x => x.Et,"Enum is defined","Et")
                     .IsDefined
                     .End();
            
           var results= validator.Validate(new TestTarget { Et = (EnumTest)(-1) });
            foreach(var result in results.ErrorMessages ){
                Console.WriteLine($" {result}");
            }
        }
    }
}
