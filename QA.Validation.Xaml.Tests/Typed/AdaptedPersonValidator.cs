using System;
using QA.Validation.Xaml.Adapters;
using QA.Validation.Xaml.Fluent;
using QA.Validation.Xaml.Tests.Model;
using QA.Validation.Xaml.Tests.Util;

namespace QA.Validation.Xaml.Tests.Typed
{
    public class AdaptedPersonValidator : AbstractValidator<Person, ReflectionModelAdapter<Person>>
    {
        protected AdaptedPersonValidator(Func<AbstractValidator<Person, ReflectionModelAdapter<Person>>, 
            ReflectionModelAdapter<Person>> adapterProvider)
            : base(adapterProvider)
        {

        }

        /// <summary>
        /// Создает экземпляр валидатора. 
        /// Лучше использовать CreateInstance
        /// </summary>
        [Obsolete]
        public AdaptedPersonValidator()
            : base(obj => new ReflectionModelAdapter<Person>())
        {

        }

        public static AdaptedPersonValidator CreateInstance()
        {
            var adapter = new ReflectionModelAdapter<Person>();
            return new AdaptedPersonValidator(obj => adapter);
        }

        protected override string OnResolvePath()
        {
            return ValidatorConstants.Basic_Dictionary;
        }

    }
}
