using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Thorm.Generator.Analysis
{
    public class CodeAnalyzer : ICodeAnalyzer
    {
        private readonly ILogger logger;

        public CodeAnalyzer(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task<CodeAnalysis> Analyze(Compilation compilation, WellKnownTypes wellKnownTypes, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            foreach (var reference in this.compilation.References)
            {
                if (!(this.compilation.GetAssemblyOrModuleSymbol(reference) is IAssemblySymbol asm)) continue;
                this.ReferencedAssemblies.Add(asm);
            }

            // Recursively all assemblies considered known from the inspected assembly.
            ExpandKnownAssemblies(compilation.Assembly);

            // Add all types considered known from each known assembly.
            ExpandKnownTypes(this.KnownAssemblies);

            this.ExpandAssembliesWithGeneratedCode();


        }
        
        private async Task ExpandKnownAssemblies(IAssemblySymbol asm)
        {
            if (!this.KnownAssemblies.Add(asm))
            {
                return;
            }

            if (!asm.GetAttributes(this.knownAssemblyAttribute, out var attrs)) return;

            foreach (var attr in attrs)
            {
                var param = attr.ConstructorArguments.First();
                if (param.Kind != TypedConstantKind.Type)
                {
                    throw new ArgumentException($"Unrecognized argument type in attribute [{attr.AttributeClass.Name}({param.ToCSharpString()})]");
                }

                var type = (ITypeSymbol)param.Value;
                if (log.IsEnabled(LogLevel.Debug)) log.LogDebug($"Known assembly {type.ContainingAssembly} from assembly {asm}");

                // Check if the attribute has the TreatTypesAsSerializable property set.
                var prop = attr.NamedArguments.FirstOrDefault(a => a.Key.Equals("TreatTypesAsSerializable")).Value;
                if (prop.Type != null)
                {
                    var treatAsSerializable = (bool)prop.Value;
                    if (treatAsSerializable)
                    {
                        // When checking if a type in this assembly is serializable, always respond that it is.
                        this.AddAssemblyWithForcedSerializability(asm);
                    }
                }

                // Recurse on the assemblies which the type was declared in.
                ExpandKnownAssemblies(type.OriginalDefinition.ContainingAssembly);
            }
        }

        private async Task ExpandKnownTypes(IEnumerable<IAssemblySymbol> asm)
        {
            foreach (var a in asm)
            {
                if (!a.GetAttributes(this.considerForCodeGenerationAttribute, out var attrs)) continue;

                foreach (var attr in attrs)
                {
                    var typeParam = attr.ConstructorArguments.First();
                    if (typeParam.Kind != TypedConstantKind.Type)
                    {
                        throw new ArgumentException($"Unrecognized argument type in attribute [{attr.AttributeClass.Name}({typeParam.ToCSharpString()})]");
                    }

                    var type = (INamedTypeSymbol)typeParam.Value;
                    this.KnownTypes.Add(type);

                    var throwOnFailure = false;
                    var throwOnFailureParam = attr.ConstructorArguments.ElementAtOrDefault(2);
                    if (throwOnFailureParam.Type != null)
                    {
                        throwOnFailure = (bool)throwOnFailureParam.Value;
                        if (throwOnFailure) this.CodeGenerationRequiredTypes.Add(type);
                    }

                    if (log.IsEnabled(LogLevel.Debug)) log.LogDebug($"Known type {type}, Throw on failure: {throwOnFailure}");
                }
            }
        }
    }
}