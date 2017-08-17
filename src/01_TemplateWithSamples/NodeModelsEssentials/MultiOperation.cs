using System;
using System.Collections.Generic;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using NodeModelsEssentials.Functions;

namespace NodeModelsEssentials.Examples
{
    // <summary>
    // Sample Node Model called MultiplyMulti.
    // It returns the product of two numbers and a verbose string.
    // </summary>
    [NodeName("Essentials.MultiOperation")]
    [NodeDescription("Performs multiple operations with the inputs.")]
    [NodeCategory("NodeModelsEssentials")]
    [InPortNames("A", "B")]
    [InPortTypes("double", "double")]
    [InPortDescriptions("Number A", "Number B")]
    [OutPortNames("AxB", "A+B", "A-B", "A/B")]
    [OutPortTypes("double", "double")]
    [OutPortDescriptions(
        "Product of A x B",
        "Addition of A and B",
        "Subtraction of A and B",
        "Division of A by B")]
    [IsDesignScriptCompatible]
    public class MultiOperation : NodeModel
    {
        public MultiOperation()
        {
            RegisterAllPorts();
        }

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAsNodes)
        {
            if (!HasConnectedInput(0) || !HasConnectedInput(1))
            {
                return new[] {
                    AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode()),
                    AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(1), AstFactory.BuildNullNode()),
                    AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(2), AstFactory.BuildNullNode()),
                    AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(3), AstFactory.BuildNullNode())
                };
            }

            var multiplyFunctionCall =
                AstFactory.BuildFunctionCall(
                    new Func<double, double, double>(NodeModelsEssentialsFunctions.Multiply),
                    new List<AssociativeNode> { inputAsNodes[0], inputAsNodes[1] });

            var addFunctionCall =
                AstFactory.BuildFunctionCall(
                    new Func<double, double, double>(NodeModelsEssentialsFunctions.Add),
                    new List<AssociativeNode> { inputAsNodes[0], inputAsNodes[1] });
            
            var subtractFunctionCall =
                AstFactory.BuildFunctionCall(
                    new Func<double, double, double>(NodeModelsEssentialsFunctions.Subtract),
                    new List<AssociativeNode> { inputAsNodes[0], inputAsNodes[1] });
            
            var divideFunctionCall =
                AstFactory.BuildFunctionCall(
                    new Func<double, double, double>(NodeModelsEssentialsFunctions.Divide),
                    new List<AssociativeNode> { inputAsNodes[0], inputAsNodes[1] });

            return new[] {
                AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), multiplyFunctionCall),
                AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(1), addFunctionCall),
                AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(2), subtractFunctionCall),
                AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(3), divideFunctionCall)
            };
        }
    }
}
