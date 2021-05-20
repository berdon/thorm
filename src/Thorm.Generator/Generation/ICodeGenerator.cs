using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Thorm.Generator.Generation
{
    public interface ICodeGenerator
    {
        Task<CompilationUnitSyntax> Generate(Compilation compilation, Action<CodeGeneratorOptions> options, CancellationToken ct = default);
    }
}