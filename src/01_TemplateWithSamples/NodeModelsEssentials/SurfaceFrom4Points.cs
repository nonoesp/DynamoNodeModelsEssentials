using System;
using System.Collections.Generic;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using NodeModelsEssentials.Functions;
using Autodesk.DesignScript.Geometry;

namespace NodeModelsEssentials.SurfaceFrom4Points
{
    // <summary>
    // A simple node to create a surface from four points.
    // </summary>
    [NodeName("Essentials.SurfaceFrom4Points")]
    [NodeDescription("A simple node to create a surface from four points.")]
    [NodeCategory("NodeModelsEssentials")]
    [InPortNames("point1", "point2", "point3", "point4")]
    [InPortTypes(
        "Autodesk.DesignScript.Geometry.Point",
        "Autodesk.DesignScript.Geometry.Point",
        "Autodesk.DesignScript.Geometry.Point",
        "Autodesk.DesignScript.Geometry.Point")]
    [InPortDescriptions("First point", "Second point", "Third point", "Fourth point")]
    [OutPortNames("Surface")]
    [OutPortTypes("Autodesk.DesignScript.Geometry.Surface")]
    [OutPortDescriptions("A surface defined by four corner points.")]
    [IsDesignScriptCompatible]
    public class SurfaceFrom4Points : NodeModel
    {
        public SurfaceFrom4Points()
        {
            RegisterAllPorts();
        }

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAsNodes)
        {
            if (!HasConnectedInput(0) || !HasConnectedInput(1) || !HasConnectedInput(2) || !HasConnectedInput(3))
            {
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode()) };
            }

            var functionCall =
                AstFactory.BuildFunctionCall(
                    new Func<Point, Point, Point, Point, Surface>(NodeModelsEssentialsFunctions.SurfaceFrom4Points),
                    new List<AssociativeNode> { inputAsNodes[0], inputAsNodes[1], inputAsNodes[2], inputAsNodes[3] });

            // Return an assigment of the generated Ast function call to output node 0
            return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall) };
        }
    }
}
