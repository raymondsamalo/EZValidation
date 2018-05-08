using System;
namespace EZ.Validation
{
    public class EnumValidatorCheck<TEnum> : ValidatorCheck<TEnum> 
    {
        /// <summary>
        /// Gets the not null or empty.
        /// </summary>
        /// <value>The not null or empty.</value>
        public EnumValidatorCheck<TEnum> IsDefined
        {
            get
            {
                this.Must(x => Enum.IsDefined(typeof(TEnum),x));
                return this;
            }
        }
    }
    /// <summary>
    /// Enum validator extension.
    /// </summary>
    public static class EnumValidatorExtension
    {
        /// <summary>
        /// Adds the rule for.
        /// </summary>
        /// <returns>The rule for.</returns>
        /// <param name="validator">Validator.</param>
        /// <param name="ruleSelector">Rule selector.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static EnumValidatorCheck<TEnum> BeginRuleFor<T,TEnum>(this Validator<T> validator,
                                                                    Func<T, TEnum> ruleSelector,
                                                        string message = null,
                                                        string name = null
                                                       )
        {
            return validator.AddRule<TEnum, EnumValidatorCheck<TEnum>>(ruleSelector, message, name);
        }
    }
}
