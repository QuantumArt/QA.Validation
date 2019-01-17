namespace QA.Validation.Xaml
{
    public interface IDefinitionStorage
    {
        bool TryGetDefinition(string key, out PropertyDefinition definition);
        void OnDefinitionAdded(PropertyDefinition definition);
        PropertyDefinition[] GetAll();
    }
}
