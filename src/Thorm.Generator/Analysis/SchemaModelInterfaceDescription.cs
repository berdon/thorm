using System.Collections.Generic;
using System.Collections;
using Microsoft.CodeAnalysis;

namespace Thorm.Generator.Analysis
{
    public class SchemaModelInterfaceDescription
    {
        public readonly INamedTypeSymbol Type;
        public readonly IEnumerable<SchemaModelColumnDescription> Columns;

        public SchemaModelInterfaceDescription(INamedTypeSymbol type, IEnumerable<SchemaModelColumnDescription> columns)
        {
            Type = type;
            Columns = columns;
        }
    }
}