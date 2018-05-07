using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace EZ.Validation
{
    /// <summary>
    /// - rsamalo May 4th 2018
    /// A feeble attempt to build fluent validation:
    /// - extensible
    /// - handle all cases
    /// </summary>
    ///
    public interface IValidatorNamedRule
    {
        String Name { get; }
        String Message { get; }
    }
    public interface IValidatorRule<T> : IValidatorNamedRule
    {
        bool Validate(T target);
    }

    public class ValidatorRule<T, TProp> : IValidatorRule<T>
    {
        public Func<T, TProp> Selector { get; set; }
        public ValidatorCheck<TProp> Check { get; set; } = new ValidatorCheck<TProp>();
        public bool Validate(T target)
        {
            var prop = this.Selector(target);
            return Check.Validate(prop);
        }
        public String Name => Check.Name;
        public String Message => Check.Message;

    }
    public class ValidatorCheck<TProp>
    {
        public String Name { get; private set; } = typeof(TProp).FullName;
        public void AsName(String name)
        {
            Name = name;
        }
        public String Message { get; private set; } = "Invalidate rule";
        public void WithMessage(String message)
        {
            Message = message;
        }

        private List<Func<TProp, bool>> Checks { get; set; } = new List<Func<TProp, bool>>();
        public ValidatorCheck<TProp> Must(Func<TProp, bool> func)
        {
            this.Checks.Add(func);
            return this;
        }
        public ValidatorCheck<TProp> MustNot(Func<TProp, bool> func)
        {
            this.Checks.Add( x => !func(x));
            return this;
        }
        public bool Validate(TProp prop)
        {
            bool valid = true;
            foreach (Func<TProp, bool> check in Checks)
            {
                valid = valid && check(prop);
                if (!valid)
                {
                    break;
                }
            }
            return valid;
        }
    }

    public class ValidatorResult
    {
        public Dictionary<string, List<string>> ErrorMessages { get; protected set; } = new Dictionary<string, List<string>>();
        public bool Valid { get; protected set; } = true;
        public void AddFailedRule(IValidatorNamedRule rule)
        {
            var messages = ErrorMessages.GetValueOrDefault(rule.Name);
            if (messages == null)
            {
                messages = new List<string>();
            }
            messages.Add(rule.Message);
            ErrorMessages.Add(rule.Name, messages);
            Valid = false;
        }
    }

    public class Validator<T>
    {
        public List<IValidatorRule<T>> Rules { get; set; } = new List<IValidatorRule<T>>();

        public ValidatorResult Validate(T target, bool continueOnFail = false)
        {
            var result = new ValidatorResult();
            foreach (var rule in Rules)
            {
                if (!rule.Validate(target))
                {
                    result.AddFailedRule(rule);
                    if (!continueOnFail)
                    {
                        break;
                    }
                }
            }
            return result;
        }
    }

}
