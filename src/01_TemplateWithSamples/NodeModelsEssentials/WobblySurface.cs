using System;
using System.Collections.Generic;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using NodeModelsEssentials.Functions;
using Autodesk.DesignScript.Geometry;
using System.Linq;

namespace NodeModelsEssentials.Examples
{

    // <summary>
    // Given some size parameters, returns a lofted surface with random bumps.
    // </summary>
    [NodeName("Essentials.WobblySurface")]
    [NodeDescription("Given some size parameters, returns a lofted surface with random bumps.")]
    [NodeCategory("NodeModelsEssentials")]
    [InPortNames("baseX", "baseY", "baseZ", "width", "length", "maxHeight", "uCount", "vCount")]
    [InPortTypes("double", "double", "double", "double", "double", "double", "int", "int")]
    [InPortDescriptions("baseX", "baseY", "baseZ", "width", "length", "maxHeight", "uCount", "vCount")]
    [OutPortNames("Surface")]
    [OutPortTypes("Surface")]
    [OutPortDescriptions("Lofted surface with random bumps.")]
    [IsDesignScriptCompatible]
    public class WobblySurface : NodeModel
    {
        public WobblySurface()
        {
            RegisterAllPorts();
        }

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAsNodes)
        {

                if (
                !InPorts[0].Connectors.Any() ||
                !InPorts[1].Connectors.Any() ||
                !InPorts[2].Connectors.Any() ||
                !InPorts[3].Connectors.Any() ||
                !InPorts[4].Connectors.Any() ||
                !InPorts[5].Connectors.Any() ||
                !InPorts[6].Connectors.Any() ||
                !InPorts[7].Connectors.Any()
                )
            {
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode()) };
            }

            var functionCall =
                AstFactory.BuildFunctionCall(
                    new Func<double, double, double, double, double, double, int, int, Surface>(NodeModelsEssentialsFunctions.WobblySurface),
                    new List<AssociativeNode> {
                        inputAsNodes[0],
                        inputAsNodes[1],
                        inputAsNodes[2],
                        inputAsNodes[3],
                        inputAsNodes[4],
                        inputAsNodes[5],
                        inputAsNodes[6],
                        inputAsNodes[7],
                    });

            // Return an assigment of the generated Ast function call to output node 0
            return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall) };
        }
    }
}
