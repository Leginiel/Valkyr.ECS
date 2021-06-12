using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Valkyr.ECS.Generator;
using Xunit;
using Xunit.Abstractions;

namespace Valkyr.ECS.Tests
{
  public class ComponentGeneratorTest
  {
    private readonly ITestOutputHelper _output;

    [Component("ComponentGerneratorTest")]
    private int value;
    [Component("ComponentGerneratorTest")]
    private int value2;

    public ComponentGeneratorTest(ITestOutputHelper output)
    {
      _output = output;
    }

    [Fact]
    public void Generate_FieldWithComponentAttribute_SourceGenerated()
    {
      Assembly assembly = GetType().Assembly;
      Type type = assembly.GetType("Valkyr.ECS.Components.ComponentGerneratorTestComponent");
      List<string> expectedMember = new() { nameof(value).ToLower(), nameof(value2).ToLower() };


      type.Should().NotBeNull();

      foreach (PropertyInfo property in type.GetProperties())
      {
        expectedMember.Should().Contain(property.Name.ToLower());
      }
    }

    [Fact(Skip = "This Test is just do debug the source generator")]
    public void SourceGeneratorDebug()
    {
      string source = @"
using Valkyr.ECS;

  public class Test 
  {
    [Component(""Test"")]
    private int value;
  }

}
";
      string output = GetGeneratedOutput(source);

      Assert.NotNull(output);

      Assert.Equal("class Foo { }", output);
    }

    private string GetGeneratedOutput(string source)
    {
      var syntaxTree = CSharpSyntaxTree.ParseText(source);

      var references = new List<MetadataReference>();
      Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
      foreach (var assembly in assemblies)
      {
        if (!assembly.IsDynamic && !string.IsNullOrWhiteSpace(assembly.Location))
        {
          references.Add(MetadataReference.CreateFromFile(assembly.Location));
        }
      }

      var compilation = CSharpCompilation.Create("foo", new SyntaxTree[] { syntaxTree }, references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

      // TODO: Uncomment this line if you want to fail tests when the injected program isn't valid _before_ running generators
      // var compileDiagnostics = compilation.GetDiagnostics();
      // Assert.False(compileDiagnostics.Any(d => d.Severity == DiagnosticSeverity.Error), "Failed: " + compileDiagnostics.FirstOrDefault()?.GetMessage());

      ISourceGenerator generator = new ComponentGenerator();

      var driver = CSharpGeneratorDriver.Create(generator);
      driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var generateDiagnostics);
      Assert.False(generateDiagnostics.Any(d => d.Severity == DiagnosticSeverity.Error), "Failed: " + generateDiagnostics.FirstOrDefault()?.GetMessage());

      string output = outputCompilation.SyntaxTrees.Last().ToString();

      _output.WriteLine(output);

      return output;
    }
  }
}
