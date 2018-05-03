using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace EZValidation
{
    /// <summary>
    /// - rsamalo May 4th 2018
    /// A feeble attempt to build fluent validation:
    /// - extensible 
    /// - handle all cases 
    /// </summary>
    public interface IValidatorRule<T>
    {
         bool Validate(T target);
        String Name { get; }
    }

    public class ValidatorRule<T,TProp>:IValidatorRule<T>
    {
        public Func<T, TProp> Selector { get; set; } 
        public ValidatorCheck<TProp> Check { get; set; } = new ValidatorCheck<TProp>();
        public bool Validate(T target){
            var prop= this.Selector(target);
            return Check.Validate(prop);
        }     
        public String Name => Check.Name;

    }
    public class ValidatorCheck<TProp>
    {
        public String Name { get; private set; }
        public void WithName(String name)
        {
            Name = name;
        }

        public List<Func<TProp, bool>> Checks { get; set; } = new List<Func<TProp, bool>>();
        public ValidatorCheck< TProp> Must(Func<TProp, bool> func)
        {
            this.Checks.Add(func);
            return this;
        }
        public bool Validate(TProp prop){
            bool valid = true;
            foreach(Func<TProp,bool> check in Checks)
            {
                valid = valid && check(prop);
                if(!valid){
                    break;
                }
            }
            return valid;
        }      
    }
   
    public class ValidatorResult
    {
        public List<string> ErrorMessages { get; protected set; } = new List<string>();
        public bool Valid { get; protected set;  } = true;
        public void AddFailure(string errorMessage)
        {
            ErrorMessages.Add(errorMessage);
            Valid = false;
        }
    }
  
   
    public class Validator<T>
    {
       public List<IValidatorRule<T>> Rules { get; set; }= new List<IValidatorRule<T>>();

        public ValidatorResult Validate(T target,bool continueOnFail=false)
        {
            ValidatorResult result = new ValidatorResult();
            foreach(var rule in Rules)
            {
                bool valid = rule.Validate(target);
                if (!valid)
                {
                    result.AddFailure($"Fail rule {rule.Name}");
                    if(!continueOnFail){
                        break;
                    }
                }
            }
            return result;
        }
    }
   
   
   
}
