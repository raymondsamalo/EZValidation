using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace EZ.Validation
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
        public StringValidatorCheck IsMatch(string pattern)
        => (StringValidatorCheck)Must((arg) => Regex.IsMatch(arg, pattern));

        /// <summary>
        /// Gets the not null or empty.
        /// </summary>
        /// <value>The not null or empty.</value>
        public StringValidatorCheck NotNullOrEmpty
        => (StringValidatorCheck)Must(x => !string.IsNullOrEmpty(x));


        /// <summary>
        /// Ises the equals.
        /// </summary>
        /// <returns>The equals.</returns>
        /// <param name="other">Other.</param>
        public StringValidatorCheck isEquals(String other)
        => (StringValidatorCheck)Must(x => String.Equals(x, other));


        /// <summary>
        /// Longers the than.
        /// </summary>
        /// <returns>The than.</returns>
        /// <param name="length">Length.</param>
        public StringValidatorCheck LongerThan(int length)
        => (StringValidatorCheck)Must(x => x.Length > length);


        public StringValidatorCheck IsMailAddress(string mailAddress)
        => (StringValidatorCheck)Must(x =>
            {
                var result = false;
                try
                {
                result = string.Equals((new MailAddress(x)).Address, x);
                }
                catch (Exception)
                {
                    result = false;
                }
                return result;
            }
         );


        public StringValidatorCheck IsAbsoluteURI
        => (StringValidatorCheck)Must(x => Uri.IsWellFormedUriString(x, UriKind.Absolute));


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
        public static StringValidatorCheck BeginRuleFor<T>(this Validator<T> validator,
                                                         Func<T, String> ruleSelector,
                                                         string message = null,
                                                         string name = null
                                                        )
        => validator.AddRule<String, StringValidatorCheck>(ruleSelector, message, name);
    }
}
