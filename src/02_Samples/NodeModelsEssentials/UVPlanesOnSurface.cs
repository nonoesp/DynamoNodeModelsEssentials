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
    // Given a surface, returns a grid of UV tangent planes.
    // </summary>
    [NodeName("Essentials.UVPlanesOnSurface")]
    [NodeDescription("Given a surface, returns a grid of UV tangent planes.")]
    [NodeCategory("NodeModelsEssentials")]
    [InPortNames("surface", "uCount", "vCount")]
    [InPortTypes("Surface", "int", "int")]
    [InPortDescriptions("Surface to evaluate", "Count u", "Count v")]
    [OutPortNames("Surface")]
    [OutPortTypes("Plane[][]")]
    [OutPortDescriptions("UV tangent planes.")]
    [IsDesignScriptCompatible]
    public class UVPlanesOnSurface : NodeModel
    {
        public UVPlanesOnSurface()
        {
            RegisterAllPorts();
        }

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAsNodes)
        {
            if (!InPorts[0].Connectors.Any() || !InPorts[1].Connectors.Any() || !InPorts[2].Connectors.Any())
            {
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode()) };
            }

            var functionCall =
                AstFactory.BuildFunctionCall(
                    new Func<Surface, int ,int, List<List<Plane>>>(NodeModelsEssentialsFunctions.UVPlanesOnSurface),
                    new List<AssociativeNode> { inputAsNodes[0], inputAsNodes[1], inputAsNodes[2] });

            // Return an assigment of the generated Ast function call to output node 0
            return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall) };
        }
    }
}
