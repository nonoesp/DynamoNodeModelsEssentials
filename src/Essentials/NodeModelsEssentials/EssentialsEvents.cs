using System;
using System.Collections.Generic;
using System.Collections;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using NodeModelsEssentials.Functions;
using System.Linq;
using Newtonsoft.Json;
using System.Windows;
using Dynamo.Events;

namespace NodeModelsEssentials
{
    /// <summary>
    /// A node that displays a message in the pre and post execution events of the graph.
    /// These events are fired whenever the graph is modified anywhere, and not only when
    /// the Essentials.Events node is modified.
    /// </summary>
    [NodeName("Essentials.Events")]
    [NodeDescription("Empty.")]
    [NodeCategory("NodeModelsEssentials")]
    [InPortNames("In")]
    [InPortTypes("string")]
    [InPortDescriptions("A string.")]
    [OutPortNames("Out")]
    [OutPortTypes("string")]
    [OutPortDescriptions("Resulting string.")]
    [IsDesignScriptCompatible]
    class EssentialsEvents : NodeModel
    {
        #region constructor

        [JsonConstructor]
        private EssentialsEvents(IEnumerable<PortModel> inPorts, IEnumerable<PortModel> outPorts) : base(inPorts, outPorts)
        {
            RegisterExecutionEvents();
        }

        private void ExecutionEvents_GraphPostExecution(Dynamo.Session.IExecutionSession session)
        {
            MessageBox.Show("Post execution event of this node.");
        }

        private void ExecutionEvents_GraphPreExecution(Dynamo.Session.IExecutionSession session)
        {
            MessageBox.Show("Pre execution event of this node.");
        }

        public EssentialsEvents()
        {
            RegisterAllPorts();
            RegisterExecutionEvents();
        }

        public void RegisterExecutionEvents()
        {
            ExecutionEvents.GraphPreExecution += ExecutionEvents_GraphPreExecution;
            ExecutionEvents.GraphPostExecution += ExecutionEvents_GraphPostExecution;
        }

        #endregion

        #region ast

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            if (!InPorts[0].Connectors.Any())
            {
                return new[]
                    {AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode())};
            }

            return new[]
            {
                AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), inputAstNodes[0])
            };
        }

        #endregion

    }

}
