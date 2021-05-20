using Microsoft.CodeAnalysis;

namespace Thorm.Generator.Analysis
{
    public class SchemaModelColumnDescription
    {
        public readonly INamedTypeSymbol Name;
        public readonly IPropertySymbol Property;

        public SchemaModelColumnDescription(INamedTypeSymbol name, IPropertySymbol property)
        {
            Name = name;
            Property = property;
        }
    }
}