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
        String RuleName { get;  }
        String Name { get; set; }
        String Message { get; set; }
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
        public String Name { get; set; }  
        public String Message { get;  set; } = "validation rule";
        public String RuleName => Name ?? $"Rule({typeof(T).Name} => {typeof(TProp).Name})";
    }
    public class ValidatorCheck<TProp>
    {

        private List<Func<TProp, bool>> Checks { get; set; } = new List<Func<TProp, bool>>();
        public ValidatorCheck<TProp> Must(Func<TProp, bool> func)
        {
            this.Checks.Add(func);
            return this;
        }
        public void End()
        {
            //this does nothing    
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
        protected Dictionary<string, List<string>> Failures { get;  set; } = new Dictionary<string, List<string>>();
        public bool Valid { get; protected set; } = true;
        public void AddFailedRule(IValidatorNamedRule rule)
        {
            var messages = Failures.GetValueOrDefault(rule.RuleName);
            if (messages == null)
            {
                messages = new List<string>();
            }
            messages.Add(rule.Message);
            Failures.Add(rule.RuleName, messages);
            Valid = false;
        }
        public IEnumerable<String> ErrorMessages =>Failures.Select((KeyValuePair<string, List<string>> arg) =>$"{arg.Key} failed {String.Join(",", arg.Value)}");
    }

    public class Validator<T>
    {
        private List<IValidatorRule<T>> Rules { get; set; } = new List<IValidatorRule<T>>();
        public TCheck AddRule<TProp,TCheck>(Func<T, TProp> ruleSelector,String message=null,
                                            String name=null )
            where TCheck:ValidatorCheck<TProp>, new()
        {
            var check = new TCheck();
            var rule = new ValidatorRule<T, TProp>
            {
                Selector = ruleSelector,
                Check = check,
            };
            if (name != null) { rule.Name = name; }
            if (message != null) { rule.Message = message; }
            Rules.Add(rule);
            return check;
        }
       
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
