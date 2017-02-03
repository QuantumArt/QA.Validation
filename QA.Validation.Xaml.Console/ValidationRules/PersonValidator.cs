using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QA.Validation.Xaml.Fluent;

namespace QA.Validation.Xaml.Console.ValidationRules
{
    public class BasicPersonValidator : AbstractValidator<Person>
    {
        protected override string OnResolvePath()
        {
            return "QA.Validation.Xaml.Console.ValidationRules.Basic.xaml";
        }
    }
}
