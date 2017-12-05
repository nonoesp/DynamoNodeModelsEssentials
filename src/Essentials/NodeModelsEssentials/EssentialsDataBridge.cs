using System;
using System.Collections.Generic;
using System.Collections;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using NodeModelsEssentials.Functions;
using System.Linq;
using Newtonsoft.Json;
using System.Windows;

namespace NodeModelsEssentials
{
    /// <summary>
    /// A node that displays how a data bridge can be used within a Node Model.
    /// </summary>
    [NodeName("Essentials.DataBridge")]
    [NodeDescription("Empty.")]
    [NodeCategory("NodeModelsEssentials")]
    [InPortNames("A", "B", "C")]
    [InPortTypes("string", "string", "string")]
    [InPortDescriptions("A string.", "Another string.", "Another string.")]
    [OutPortNames("Out")]
    [OutPortTypes("string")]
    [OutPortDescriptions(
        "Resulting string.")]
    [IsDesignScriptCompatible]
    class EssentialsDataBridge : NodeModel
    {
        #region constructor

        [JsonConstructor]
        private EssentialsDataBridge(IEnumerable<PortModel> inPorts, IEnumerable<PortModel> outPorts) : base(inPorts, outPorts)
        {
        }

        public EssentialsDataBridge()
        {
            RegisterAllPorts();
        }

        #endregion

        #region data bridge callback

        /// <summary>
        /// Register the data bridge callback.
        /// </summary>
        protected override void OnBuilt()
        {
            base.OnBuilt();
            VMDataBridge.DataBridge.Instance.RegisterCallback(GUID.ToString(), DataBridgeCallback);
        }

        /// <summary>
        /// Unregister the data bridge callback.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            VMDataBridge.DataBridge.Instance.UnregisterCallback(GUID.ToString());
        }

        /// <summary>
        /// Callback method for DataBridge mechanism.
        /// This callback only gets called once after the BuildOutputAst Function is executed 
        /// This callback casts the response data object.
        /// </summary>
        /// <param name="data">The data passed through the data bridge.</param>
        private void DataBridgeCallback(object data)
        {
            ArrayList inputs = data as ArrayList;
            foreach (var input in inputs)
            {
                MessageBox.Show(input.ToString());
            }
        }

        #endregion

        #region ast

        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAstNodes)
        {
            if (!InPorts[0].Connectors.Any() || !InPorts[1].Connectors.Any() || !InPorts[2].Connectors.Any())
            {
                return new[]
                    {AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode())};
            }

            var functionCallNode = AstFactory.BuildFunctionCall(
                new Func<string, string, string, string>(NodeModelsEssentialsFunctions.ConcatenateThree),
                new List<AssociativeNode> { inputAstNodes[0], inputAstNodes[1], inputAstNodes[2] });

            return new[]
            {
                    AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), functionCallNode),
                    AstFactory.BuildAssignment(
                        AstFactory.BuildIdentifier(AstIdentifierBase + "_dummy"),
                        VMDataBridge.DataBridge.GenerateBridgeDataAst(GUID.ToString(), AstFactory.BuildExprList(inputAstNodes))
                    )
                };
        }

        #endregion

    }

}
