using System;
using System.Collections.Generic;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using NodeModelsEssentials.Functions;
using Newtonsoft.Json;
using System.Linq;

namespace NodeModelsEssentials
{
    // <summary>
    // This node displays a non-desirable way of using the AST.
    // We pass function call nodes (say, multiplyFunctionNode)
    // both to the sumFunctionNode and to one of the node outputs.
    // This causes the AST to execute the multiply function twice.
    // In this case, the overload is not too big, but for CPU-intensive
    // tasks or for non-deterministic results (where the returning
    // value of a function might be different two consecutive executions)
    // the sum node and the output port will have different values.
    // </summary>
    [NodeName("Essentials.AstNotReusingFunctionCall")]
    [NodeDescription("Performs multiple operations with the inputs.")]
    [NodeCategory("NodeModelsEssentials")]
    [InPortNames("A", "B")]
    [InPortTypes("double", "double")]
    [InPortDescriptions("Number A", "Number B")]
    [OutPortNames("AxB", "A+B", "A-B", "A/B", "SUM")]
    [OutPortTypes("double", "double", "double", "double", "double")]
    [OutPortDescriptions(
        "Product of A x B",
        "Addition of A and B",
        "Subtraction of A and B",
        "Division of A by B",
        "Sum of all outputs")]
    [IsDesignScriptCompatible]
    public class AstNotReusingFunctionCall : NodeModel
    {

        #region constructor

        [JsonConstructor]
        private AstNotReusingFunctionCall(IEnumerable<PortModel> inPorts, IEnumerable<PortModel> outPorts) : base(inPorts, outPorts)
        {
        }

        public AstNotReusingFunctionCall()
        {
            RegisterAllPorts();
        }

        #endregion

        #region ast

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAsNodes)
        {
            if (!InPorts[0].Connectors.Any() || !InPorts[1].Connectors.Any())
            {
                return new[] {
                    AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildIntNode(1)),
                    AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(1), AstFactory.BuildIntNode(2)),
                    AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(2), AstFactory.BuildIntNode(3)),
                    AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(3), AstFactory.BuildIntNode(4)),
                    AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(4), AstFactory.BuildIntNode(5))
                };
            }

            var multiplyFunctionNode =
                AstFactory.BuildFunctionCall(
                    new Func<double, double, double>(NodeModelsEssentialsFunctions.Multiply),
                    new List<AssociativeNode> { inputAsNodes[0], inputAsNodes[1] });

            var addFunctionNode =
                AstFactory.BuildFunctionCall(
                    new Func<double, double, double>(NodeModelsEssentialsFunctions.Add),
                    new List<AssociativeNode> { inputAsNodes[0], inputAsNodes[1] });

            var subtractFunctionNode =
                AstFactory.BuildFunctionCall(
                    new Func<double, double, double>(NodeModelsEssentialsFunctions.Subtract),
                    new List<AssociativeNode> { inputAsNodes[0], inputAsNodes[1] });

            var divideFunctionNode =
                AstFactory.BuildFunctionCall(
                    new Func<double, double, double>(NodeModelsEssentialsFunctions.Divide),
                    new List<AssociativeNode> { inputAsNodes[0], inputAsNodes[1] });

            var sumFunctionNode =
                AstFactory.BuildFunctionCall(
                    new Func<double, double, double, double, double>(NodeModelsEssentialsFunctions.Sum),
                    new List<AssociativeNode> {
                        multiplyFunctionNode,
                        addFunctionNode,
                        subtractFunctionNode,
                        divideFunctionNode });

            return new[] {
                AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), multiplyFunctionNode),
                AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(1), addFunctionNode),
                AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(2), subtractFunctionNode),
                AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(3), divideFunctionNode),
                AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(4), sumFunctionNode),
            };
        }

        #endregion
    }
}
