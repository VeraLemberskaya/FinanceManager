using System;

namespace FinanceManager.Core
{
    public class ModelRule
    {
        public Predicate<object> Property;
        public string Error;
        public ModelRule(Predicate<object> predicate, string error)
        {
            Property = predicate;
            Error = error;
        }
    }
}
