using System.Diagnostics;

namespace Thorm.Generator
{
    public class CodeGeneratorOptions
    {
        /// <summary>
        /// Whether or not to add <see cref="DebuggerStepThroughAttribute"/> to generated code.
        /// </summary>
        public bool DebuggerStepThrough { get; set; }
    }
}