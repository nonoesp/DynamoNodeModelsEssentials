using System;
using System.Collections.Generic;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using System.Windows;
using Dynamo.Events;
using System.Linq;
using Newtonsoft.Json;

namespace NodeModelsEssentials
{
    /// <summary>
    /// A node that displays a message in the pre and post execution events of the graph.
    /// These events are fired whenever the graph is modified anywhere, and not only when
    /// the Essentials.Events node is modified.
    /// </summary>
    [NodeName("Essentials.Events")]
    [NodeDescription("Execute a method of a NodeModel on the pre and post graph execution events of the graph.")]
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
            var guid = GUID.ToString().Substring(0, 5);
            MessageBox.Show("Post execution event of node " + guid + ".");
        }

        private void ExecutionEvents_GraphPreExecution(Dynamo.Session.IExecutionSession session)
        {
            var guid = GUID.ToString().Substring(0, 5);
            MessageBox.Show("Pre execution event of node " + guid + ".");
        }

        public EssentialsEvents()
        {
            RegisterAllPorts();
            RegisterExecutionEvents();
        }

        /// <summary>
        /// Subscribe to pre and post graph execution events.
        /// </summary>
        public void RegisterExecutionEvents()
        {
            ExecutionEvents.GraphPreExecution += ExecutionEvents_GraphPreExecution;
            ExecutionEvents.GraphPostExecution += ExecutionEvents_GraphPostExecution;
        }

        /// <summary>
        /// Unsubscribe from pre and post graph execution events.
        /// </summary>
        public void UnregisterExecutionEvents()
        {
            ExecutionEvents.GraphPreExecution -= ExecutionEvents_GraphPreExecution;
            ExecutionEvents.GraphPostExecution -= ExecutionEvents_GraphPostExecution;
        }
        
        // It's important to unregister from execution events, otherwise this events
        // will still be called even when the node is removed from the Dynamo graph.
        public override void Dispose()
        {
            base.Dispose();
            UnregisterExecutionEvents();
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
