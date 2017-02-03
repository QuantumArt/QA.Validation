using QA.Validation.Xaml.Fluent;
using QA.Validation.Xaml.Tests.Model;

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
