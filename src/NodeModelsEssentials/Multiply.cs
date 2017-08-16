using System;
using System.Collections.Generic;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using NodeModelsEssentials.Functions;
namespace NodeModelsEssentials.FirstNode
{
    // <summary>
    // Sample NodeModel node called Multiply.
    /// github.com/teocomi: In order to execute AstFactory.BuildFunctionCall 
    /// the methods have to be in a separate assembly and be loaded by Dynamo separately
    /// File pkg.json defines which dll files are loaded
    // </summary>
    [NodeName("NodeModelsEssentials.Multiply")]
    [NodeDescription("Multiplies A x B")]
    [NodeCategory("NodeModelsEssentials")]
    [InPortNames("A", "B")]
    [InPortTypes("double", "double")]
    [InPortDescriptions("Number A", "Number B")]
    [OutPortNames("C")]
    [OutPortTypes("double")]
    [OutPortDescriptions("Product of A x B")]
    [IsDesignScriptCompatible]
    public class Multiply : NodeModel
    {
        public Multiply()
        {
            RegisterAllPorts();
        }

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAsNodes)
        {
            if (!HasConnectedInput(0) || !HasConnectedInput(1))
            {
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode()) };
            }

            var functionCall =
                AstFactory.BuildFunctionCall(
                    new Func<double, double, double>(NodeModelsEssentialsFunctions.Multiply),
                    new List<AssociativeNode> { inputAsNodes[0], inputAsNodes[1] });

            return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall) };
        }
    }
}
