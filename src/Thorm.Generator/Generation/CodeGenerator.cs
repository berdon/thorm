using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Thorm.Generator.Analysis;

namespace Thorm.Generator.Generation
{
    public class CodeGenerator : ICodeGenerator
    {
        private bool hasGenerated = false;

        public async Task<CompilationUnitSyntax> Generate(Compilation compilation, Action<CodeGeneratorOptions> options, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            if (!hasGenerated) throw new Exception("CodeGenerator can only generate once.");
            hasGenerated = true;

            var wellKnownTypes = new WellKnownTypes(compilation);

            var analyzer = new CodeAnalyzer();
            var analysis = analyzer.Analyze(compilation, wellKnownTypes, ct);

            // Do stuff with analysis

            return null;
        }
    }
}