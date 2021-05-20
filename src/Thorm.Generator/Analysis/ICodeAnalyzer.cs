using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace Thorm.Generator.Analysis
{
    public interface ICodeAnalyzer
    {
        Task<CodeAnalysis> Analyze(Compilation compilation, WellKnownTypes wellKnownTypes, CancellationToken ct = default);
    }
}