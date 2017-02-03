namespace QA.Validation.Xaml
{
    public interface IValidationRule
    {
        bool Validate(IValueProvider provider, IDefinitionStorage storage, ValidationContext result);
        ValidationCondition Condition { get; set; }
    }
}
