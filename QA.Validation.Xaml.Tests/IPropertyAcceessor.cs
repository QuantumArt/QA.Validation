namespace QA.Validation.Xaml.Tests
{
    interface IPropertyAccessor
    {
        object Get(object target);
        void Set(object target, object value);
    }
}
