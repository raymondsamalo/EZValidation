using System;
namespace EZValidation
{
    // EXTEND With Some Built In Validator
    /// <summary>
    /// String validator rule.
    /// </summary>
    public class StringValidatorCheck : ValidatorCheck<string>
    {
        public StringValidatorCheck NotNullOrEmpty
        {
            get
            {
                this.Must(x => !string.IsNullOrEmpty(x));
                return this;
            }
        }
        public StringValidatorCheck isEquals(String other)
        {
            this.Must(x => String.Equals(x, other));
            return this;
        }
        public StringValidatorCheck LongerThan(int length)
        {
            this.Must(x => x.Length > length);
            return this;
        }
    }

    public static class StringValidatorExtension
    {
        public static StringValidatorCheck AddRuleFor<T>(this Validator<T> validator, Func<T, String> ruleSelector)
        {
            var rule = new ValidatorRule<T,string>();
            var check = new StringValidatorCheck();
            rule.Selector = ruleSelector;
            rule.Check = check;
            validator.Rules.Add(rule);
            return check;
        }

    }
}
