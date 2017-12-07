using System;
using System.Collections.Generic;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using NodeModelsEssentials.Functions;
using System.Linq;
using Newtonsoft.Json;

namespace NodeModelsEssentials
{
    // <summary>
    // Sample Node Model called MultiplyMulti.
    // It returns the product of two numbers and a verbose string.
    // </summary>
    [NodeName("Essentials.Timeout")]
    [NodeDescription("Sleeps for a bit and throws an error if the sleeping times out. Determine the maximum duration a node can run for (and time out if it surpasses it).")]
    [NodeCategory("NodeModelsEssentials")]
    [InPortNames("Sleep", "Timeout")]
    [InPortTypes("int", "int")]
    [InPortDescriptions("Sleep duration.", "Timeout duration")]
    [OutPortNames("Message")]
    [OutPortTypes("string")]
    [OutPortDescriptions("Resulting string.")]
    [IsDesignScriptCompatible]

    public class Timeout : NodeModel
    {
        [JsonConstructor]
        private Timeout(IEnumerable<PortModel> inPorts, IEnumerable<PortModel> outPorts) : base(inPorts, outPorts)
        {
        }

        public Timeout()
        {
            RegisterAllPorts();
        }

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAsNodes)
        {
            if (!InPorts[0].Connectors.Any() || !InPorts[1].Connectors.Any())
            {
                return new[] {
                    AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode())
                };
            }

            var timeoutFunctionNode =
                AstFactory.BuildFunctionCall(
                    new Func<int, int, string>(NodeModelsEssentialsFunctions.Timeout),
                    new List<AssociativeNode> { inputAsNodes[0], inputAsNodes[1] });

            return new[] {
                AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), timeoutFunctionNode)
            };
        }
    }
}
