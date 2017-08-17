using System;
using System.Collections.Generic;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using NodeModelsEssentials.Functions;

namespace NodeModelsEssentials.Examples
{
    // <summary>
    // Sample NodeModel node called Multiply.
    /// github.com/teocomi: In order to execute AstFactory.BuildFunctionCall 
    /// the methods have to be in a separate assembly and be loaded by Dynamo separately
    /// File pkg.json defines which dll files are loaded
    // </summary>
    [NodeName("Essentials.Multiply")]
    [NodeDescription("Multiplies A x B.")]
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
            // If any of the input nodes is not connected, assign output node 0 a null node return value
            if (!HasConnectedInput(0) || !HasConnectedInput(1))
            {
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode()) };
            }

            // Build a function call to a C# function which lives in a different DLL assembly
            // and use input nodes 0 and 1 as input values in the fuction
            // Note that we specify input and output value types in new Func<double, double, double>
            // which means we have two double inputs and one double output
            var functionCall =
                AstFactory.BuildFunctionCall(
                    new Func<double, double, double>(NodeModelsEssentialsFunctions.Multiply),
                    new List<AssociativeNode> { inputAsNodes[0], inputAsNodes[1] });

            // Return an assigment of the generated Ast function call to output node 0
            return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall) };
        }
    }
}
