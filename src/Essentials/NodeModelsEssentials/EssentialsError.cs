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
    [NodeName("Essentials.Error")]
    [NodeDescription("Throws an error.")]
    [NodeCategory("NodeModelsEssentials")]
    [InPortNames("In")]
    [InPortTypes("string")]
    [InPortDescriptions("A string.")]
    [OutPortNames("Out")]
    [OutPortTypes("string")]
    [OutPortDescriptions(
        "Resulting string.")]
    [IsDesignScriptCompatible]

    public class Error : NodeModel
    {
        [JsonConstructor]
        private Error(IEnumerable<PortModel> inPorts, IEnumerable<PortModel> outPorts) : base(inPorts, outPorts)
        {
        }

        public Error()
        {
            RegisterAllPorts();
        }

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAsNodes)
        {
            if (!InPorts[0].Connectors.Any())
            {
                return new[] {
                    AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode())
                };
            }

            var errorFunctionNode =
                AstFactory.BuildFunctionCall(
                    new Func<string, string>(NodeModelsEssentialsFunctions.Error),
                    new List<AssociativeNode> { inputAsNodes[0] });

            return new[] {
                AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), errorFunctionNode)
            };
        }
    }
}
