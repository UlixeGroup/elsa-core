using Elsa.Extensions;
using Elsa.JavaScript.Abstractions;
using Elsa.JavaScript.Models;
using Elsa.JavaScript.Services;
using Humanizer;

namespace Elsa.JavaScript.Providers;

/// <summary>
/// Produces <see cref="FunctionDefinition"/>s for common functions.
/// </summary>
internal class CommonFunctionsDefinitionProvider : FunctionDefinitionProvider
{
    private readonly ITypeAliasRegistry _typeAliasRegistry;

    public CommonFunctionsDefinitionProvider(ITypeAliasRegistry typeAliasRegistry)
    {
        _typeAliasRegistry = typeAliasRegistry;
    }
    
    protected override IEnumerable<FunctionDefinition> GetFunctionDefinitions(TypeDefinitionContext context)
    {
        yield return CreateFunctionDefinition(builder => builder
            .Name("setVariable")
            .Parameter("name", "string")
            .Parameter("value", "any"));
        
        yield return CreateFunctionDefinition(builder => builder
            .Name("getVariable")
            .Parameter("name", "string")
            .ReturnType("any"));
        
        yield return CreateFunctionDefinition(builder => builder
            .Name("isNullOrWhiteSpace")
            .Parameter("value", "string")
            .ReturnType("boolean"));
        
        yield return CreateFunctionDefinition(builder => builder
            .Name("isNullOrEmpty")
            .Parameter("value", "string")
            .ReturnType("boolean"));
        
        yield return CreateFunctionDefinition(builder => builder
            .Name("toJson")
            .Parameter("value", "any")
            .ReturnType("string"));
        
        // Variable getter and setters.
        foreach (var variable in context.Variables)
        {
            var pascalName = variable.Name.Pascalize();
            var variableType = variable.GetVariableType();
            var typeAlias = _typeAliasRegistry.TryGetAlias(variableType, out var alias) ? alias : "any";

            // get{Variable}.
            yield return CreateFunctionDefinition(builder => builder.Name($"get{pascalName}").ReturnType(typeAlias));
            
            // set{Variable}.
            yield return CreateFunctionDefinition(builder => builder.Name($"set{pascalName}").Parameter("value", typeAlias));
        }
    }
}