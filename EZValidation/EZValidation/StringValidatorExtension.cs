using System;
namespace EZValidation
{
    // EXTEND With Some Built In Validator
    /// <summary>
    /// String validator rule.
    /// </summary>
    public class StringValidatorCheck : ValidatorCheck<string>
    {
        /// <summary>
        /// Gets the not null or empty.
        /// </summary>
        /// <value>The not null or empty.</value>
        public StringValidatorCheck NotNullOrEmpty
        {
            get
            {
                this.Must(x => !string.IsNullOrEmpty(x));
                return this;
            }
        }
        /// <summary>
        /// Ises the equals.
        /// </summary>
        /// <returns>The equals.</returns>
        /// <param name="other">Other.</param>
        public StringValidatorCheck isEquals(String other)
        {
            this.Must(x => String.Equals(x, other));
            return this;
        }
        /// <summary>
        /// Longers the than.
        /// </summary>
        /// <returns>The than.</returns>
        /// <param name="length">Length.</param>
        public StringValidatorCheck LongerThan(int length)
        {
            this.Must(x => x.Length > length);
            return this;
        }
    }
    /// <summary>
    /// String validator extension.
    /// </summary>
    public static class StringValidatorExtension
    {
        /// <summary>
        /// Adds the rule for.
        /// </summary>
        /// <returns>The rule for.</returns>
        /// <param name="validator">Validator.</param>
        /// <param name="ruleSelector">Rule selector.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
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
