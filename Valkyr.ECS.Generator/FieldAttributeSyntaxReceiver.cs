using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Valkyr.ECS.Generator
{
  [ExcludeFromCodeCoverage]
  internal class FieldAttributeSyntaxReceiver : ISyntaxContextReceiver
  {
    public List<IFieldSymbol> Fields { get; } = new List<IFieldSymbol>();

    public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
      if (context.Node is FieldDeclarationSyntax fieldDeclarationSyntax
          && fieldDeclarationSyntax.AttributeLists.Count > 0)
      {
        foreach (VariableDeclaratorSyntax variable in fieldDeclarationSyntax.Declaration.Variables)
        {
          IFieldSymbol fieldSymbol = context.SemanticModel.GetDeclaredSymbol(variable) as IFieldSymbol;
          if (fieldSymbol.GetAttributes().Any(ad => ad.AttributeClass.ToDisplayString() == "Valkyr.ECS.ComponentAttribute"))
          {
            Fields.Add(fieldSymbol);
          }
        }
      }
    }
  }
}
