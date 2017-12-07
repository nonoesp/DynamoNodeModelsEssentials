using System;
using System.Collections.Generic;
using System.Collections;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using NodeModelsEssentials.Functions;
using System.Linq;
using Newtonsoft.Json;

namespace NodeModelsEssentials
{
    // <summary>
    // This node displays a desirable way of using the AST.
    // We pass an assignment of the returning value of a function call node
    // (say, multiplyFunctionNode) to one of the output nodes, and pass
    // its identifier to the sumFunctionNode.
    // This makes the AST to only execute the multiply function once,
    // reusing the return value in both places.
    // This ensures that CPU-intensive tasks will only execute once,
    // and that the result of non-deterministic functions (where the returning
    // value of a function might be different two consecutive executions)
    // will be consistent if used in multiple places (as it will only be
    // executed once).
    // </summary>
    [NodeName("Essentials.AstReusingFunctionCall")]
    [NodeDescription("This node displays a desirable way to use the AST (Abstract Syntax Tree) where function calls are executed only once and return values are re-used.")]
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
    class AstReusingFunctionCall : NodeModel
    {
        [JsonConstructor]
        private AstReusingFunctionCall(IEnumerable<PortModel> inPorts, IEnumerable<PortModel> outPorts) : base(inPorts, outPorts)
        {
        }

        public AstReusingFunctionCall()
        {
            RegisterAllPorts();
        }

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

            var multiplyNodeIdentifier = AstFactory.BuildIdentifier(GUID.ToString() + "_multiply");
            var multiplyNode = AstFactory.BuildAssignment(multiplyNodeIdentifier, multiplyFunctionNode);
            var addNodeIdentifier = AstFactory.BuildIdentifier(GUID.ToString() + "_add");
            var addNode = AstFactory.BuildAssignment(addNodeIdentifier, addFunctionNode);
            var subtractNodeIdentifier = AstFactory.BuildIdentifier(GUID.ToString() + "_subtract");
            var subtractNode = AstFactory.BuildAssignment(subtractNodeIdentifier, subtractFunctionNode);
            var divideNodeIdentifier = AstFactory.BuildIdentifier(GUID.ToString() + "_divide");
            var divideNode = AstFactory.BuildAssignment(divideNodeIdentifier, divideFunctionNode);

            var sumFunctionNode =
                AstFactory.BuildFunctionCall(
                    new Func<double, double, double, double, double>(NodeModelsEssentialsFunctions.Sum),
                    new List<AssociativeNode> {
                        multiplyNodeIdentifier,
                        addNodeIdentifier,
                        subtractNodeIdentifier,
                        divideNodeIdentifier });

            return new[] {
                AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), multiplyNode),
                AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(1), addNode),
                AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(2), subtractNode),
                AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(3), divideNode),
                AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(4), sumFunctionNode)
            };
        }
    }
}