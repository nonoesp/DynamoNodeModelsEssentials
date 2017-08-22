using System;
using System.Collections.Generic;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using NodeModelsEssentials.Functions;

namespace NodeModelsEssentials.Examples
{
    /*
      * This example shows how to create a node model for Dynamo
      * subclassing the NodeModel class.
     
      * Nodes that subclass NodeModel follow a different loading path
      * than zero touch nodes. The assembly which contains
      * this node needs to be located in the 'nodes' folder in
      * Dynamo in order to be loaded at startup.
     */

    // <summary>
    // Sample NodeModel node called Multiply.
    /// github.com/teocomi: In order to execute AstFactory.BuildFunctionCall 
    /// the methods have to be in a separate assembly and be loaded by Dynamo separately
    /// File pkg.json defines which dll files are loaded
    // </summary>

    // The NodeName attribute is what will display on 
    // top of the node in Dynamo
    [NodeName("Essentials.Multiply")]

    // The description will display in the tooltip
    // and in the help window for the node.
    [NodeDescription("Multiplies A x B.")]

    // The NodeCategory attribute determines how your
    // node will be organized in the library. You can
    // specify your own category or use one of the 
    // built-ins provided in BuiltInNodeCategories.
    [NodeCategory("NodeModelsEssentials")]

    // The InPortNames attribute determines the
    // amount of input ports of your node and their names.
    [InPortNames("A", "B")]

    // The InPortTypes attribute determines the
    // variable types of your input ports.
    [InPortTypes("double", "double")]

    // The InPortDescriptions attribute sets the description
    // of your input ports in the tooltip shown when you hover them.
    [InPortDescriptions("Number A", "Number B")]

    // The OutPortNames attribute determines the
    // amount of output ports of your node and their names.
    [OutPortNames("C")]

    // The OutPortTypes attribute determines the
    // variable types of your output ports.
    [OutPortTypes("double")]

    // The OutPortDescriptions attribute sets the description
    // of your output ports in the tooltip shown when you hover them.
    [OutPortDescriptions("Product of A x B")]

    // Add the IsDesignScriptCompatible attribute to ensure
    // that it gets loaded in Dynamo.
    [IsDesignScriptCompatible]
    public class Multiply : NodeModel
    {
        /// <summary>
        /// The constructor for a NodeModel is used to create
        /// the input and output ports and specify the argument
        /// lacing.
        /// </summary>
        public Multiply()
        {
            // This call is required to ensure that your ports are
            // properly created.
            RegisterAllPorts();

            // The argument lacing is the way in which Dynamo handles
            // inputs of lists. If you don't want your node to
            // support argument lacing, you can set this to LacingStrategy.Disabled.
            // ArgumentLacing = LacingStrategy.CrossProduct;
        }

        /// <summary>
        /// If this method is not overriden, Dynamo will, by default
        /// pass data through this node. But we wouldn't be here if
        /// we just wanted to pass data through the node, so let's 
        /// try using the data.
        /// </summary>
        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAsNodes)
        {
            // When you create your own UI node you are responsible
            // for generating the abstract syntax tree (AST) nodes which
            // specify what methods are called, or how your data is passed
            // when execution occurs.

            // WARNING!!!
            // Do not throw an exception during AST creation. If you
            // need to convey a failure of this node, then use
            // AstFactory.BuildNullNode to pass out null.

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

            // Using the AstFactory class, we can build AstNode objects
            // that assign doubles, assign function calls, build expression lists, etc.
            return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCall) };
        }
    }
}
