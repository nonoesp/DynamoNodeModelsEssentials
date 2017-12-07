using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Dynamo.Controls;
using Dynamo.Graph.Nodes;
using ProtoCore.AST.AssociativeAST;
using Autodesk.DesignScript.Runtime;
using Dynamo.Wpf;
using NodeModelsEssentials.Functions;
using NodeModelsEssentials.Controls;
using Dynamo.UI.Commands;
using VMDataBridge;
using Newtonsoft.Json;

namespace NodeModelsEssentials
{
    [NodeName("UI.ButtonFunction")]
    [NodeDescription("A sample Node Model with custom UI.")]
    [NodeCategory("NodeModelsEssentials")]
    [OutPortNames("Result")]
    [OutPortTypes("double")]
    [OutPortDescriptions("Current value of internal number property.")]
    [IsDesignScriptCompatible]
    public class CustomUINodeModelButtonFunction : NodeModel
    {
        #region commands

        public DelegateCommand MyCommand { get; set; }

        #endregion

        #region private members

        private double number;

        #endregion

        #region properties

        public Action Executed;

        public double Number
        {
            get { return number; }
            set
            {
                number = value;
                number = Math.Round(value, 2);
                RaisePropertyChanged("Number");

                OnNodeModified();
            }
        }

        #endregion

        #region constructor

        [JsonConstructor]
        private CustomUINodeModelButtonFunction(IEnumerable<PortModel> inPorts, IEnumerable<PortModel> outPorts) : base(inPorts, outPorts)
        {
        }

        public CustomUINodeModelButtonFunction()
        {
            Executed = new Action(() => {
                MessageBox.Show("Executed!");
            });

            RegisterAllPorts();

            MyCommand = new DelegateCommand(SayHello);

            number = 0.0;
        }

        #endregion

        #region data bridge

        public override void Dispose()
        {
            base.Dispose();
            DataBridge.Instance.UnregisterCallback(GUID.ToString());
        }

        protected override void OnBuilt()
        {
            base.OnBuilt();
            DataBridge.Instance.RegisterCallback(GUID.ToString(), DataBridgeCallback);
        }

        private void DataBridgeCallback(object data)
        {
            this.Executed();
        }

        #endregion

        #region public methods

        private void SayHello(object obj)
        {
            OnNodeModified(forceExecute: true);
            //MessageBox.Show("Hello Dynamo!");
        }

        [IsVisibleInDynamoLibrary(false)]
        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAsNodes)
        {
            var doubleNode = AstFactory.BuildDoubleNode(number);

            //var funcNode = AstFactory.BuildFunctionCall(
            //    new Func<double, double, double>(NodeModelsEssentialsFunctions.Multiply),
            //    new List<AssociativeNode>() { doubleNode, doubleNode }
            //    );

            return new[]
            {
                AstFactory.BuildAssignment(
                    GetAstIdentifierForOutputIndex(0),
                    doubleNode),
                AstFactory.BuildAssignment(
                    AstFactory.BuildIdentifier(AstIdentifierBase + "_dummy"),
                    DataBridge.GenerateBridgeDataAst(GUID.ToString(), doubleNode))
            };
        }

        #endregion
    }

    /// <summary>
    /// View customizer for CustomUINodeModel Node Model.
    /// </summary>
    public class CustomUINodeModelButtonFunctionViewCustomization : INodeViewCustomization<CustomUINodeModelButtonFunction>
    {
        private MyWpfView myWpfView = null;
        public void CustomizeView(CustomUINodeModelButtonFunction model, NodeView nodeView)
        {
            myWpfView = new MyWpfView();
            myWpfView.button.Content = "Changed";
            //myWpfView.button.Click += (object sender, RoutedEventArgs e) => {
            //    model.Number++;
            //};
            nodeView.inputGrid.Children.Add(myWpfView);

            myWpfView.DataContext = model;
        }


        public void Dispose() {
            // unsubscribe myWpfView.button from click event?
        }
    }
}
