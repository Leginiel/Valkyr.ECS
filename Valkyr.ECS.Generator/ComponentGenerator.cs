using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Valkyr.ECS.Generator
{
  [Generator]
  [ExcludeFromCodeCoverage]
  public class ComponentGenerator : ISourceGenerator
  {
    public void Initialize(GeneratorInitializationContext context)
    {
      context.RegisterForSyntaxNotifications(() => new FieldAttributeSyntaxReceiver());
    }
    public void Execute(GeneratorExecutionContext context)
    {
      if (!(context.SyntaxContextReceiver is FieldAttributeSyntaxReceiver receiver))
        return;

      INamedTypeSymbol attributeSymbol = context.Compilation.GetTypeByMetadataName("Valkyr.ECS.ComponentAttribute");

#pragma warning disable RS1024 // Compare symbols correctly
      foreach (IGrouping<string, IFieldSymbol> group in receiver.Fields.GroupBy(f => f.GetAttributes().First(_ => _.AttributeClass.Name.Equals("ComponentAttribute")).ConstructorArguments[0].Value as string))
#pragma warning restore RS1024 // Compare symbols correctly
      {
        string componentSource = ProcessComponent(group.Key, group.ToList());
        context.AddSource($"{group.Key}Component.cs", componentSource);
      }
    }

    private static string ProcessComponent(string name, List<IFieldSymbol> properties)
    {
      string componentName = name + "Component";
      StringBuilder source = new StringBuilder($@"
namespace Valkyr.ECS.Components
{{
  public class {componentName} : IComponent
  {{
");
      ProcessConstructor(source, componentName, properties);
      foreach (IFieldSymbol fieldSymbol in properties)
      {
        ProcessField(source, fieldSymbol);
      }

      source.AppendLine("}");
      source.AppendLine("}");

      return source.ToString();
    }

    private static void ProcessConstructor(StringBuilder source, string name, List<IFieldSymbol> properties)
    {
      int i = 0;

      source.Append($"public {name} (");
      foreach (IFieldSymbol fieldSymbol in properties)
      {
        if (i == 0)
          source.Append($"{fieldSymbol.Type} {fieldSymbol.Name}");
        else
          source.Append($", {fieldSymbol.Type} {fieldSymbol.Name}");
        i++;
      }
      source.AppendLine(") {");
      foreach (IFieldSymbol fieldSymbol in properties)
      {
        source.AppendLine($"this.{GetFieldName(fieldSymbol)} = {fieldSymbol.Name};");
      }
      source.AppendLine("}");
    }

    private static void ProcessField(StringBuilder source, IFieldSymbol fieldSymbol)
    {
      string fieldName = GetFieldName(fieldSymbol);
      ITypeSymbol fieldType = fieldSymbol.Type;

      source.AppendLine($"public {fieldType} {fieldName} {{ get; }}");
    }

    private static string GetFieldName(IFieldSymbol symbol)
    {
      return symbol.Name[0].ToString().ToUpper() + symbol.Name.Substring(1);
    }
  }
}
