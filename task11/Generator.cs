using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Task11
{
    public class CodeGenerator
    {
        public ICalculator GenerateCalculator(string code)
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);

            var references = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                Metadata
                Reference.CreateFromFile(typeof(ICalculator).Assembly.Location),
                MetadataReference.CreateFromFile(Path.Combine(Path.GetDirectoryName(typeof(object).Assembly.Location)!, "System.Runtime.dll")),
                MetadataReference.CreateFromFile(Path.Combine(Path.GetDirectoryName(typeof(object).Assembly.Location)!, "netstandard.dll"))
            };

            CSharpCompilation compilation = CSharpCompilation.Create(
                "DynamicCalculatorAssembly",
                new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using var ms = new MemoryStream();
            var result = compilation.Emit(ms);

            if (!result.Success)
                      
            {
                var failures = result.Diagnostics.Where(d => d.IsWarningAsError || d.Severity == DiagnosticSeverity.Error);
                throw new Exception("Ошибка компиляции: " + string.Join("\n", failures.Select(f => f.GetMessage())));
            }

            ms.Seek(0, SeekOrigin.Begin);
            Assembly assembly = AssemblyLoadContext.Default.LoadFromStream(ms);

            var type = assembly.GetType("Calculator");
            return (ICalculator)Activator.CreateInstance(type)!;
        }
    }
}