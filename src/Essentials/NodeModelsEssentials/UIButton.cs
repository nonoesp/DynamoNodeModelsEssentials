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
using Microsoft.Practices.Prism.Commands;
using Newtonsoft.Json;
using DelegateCommand = Dynamo.UI.Commands.DelegateCommand;

namespace NodeModelsEssentials
{
    [NodeName("UI.Button")]
    [NodeDescription("A sample Node Model with custom UI.")]
    [NodeCategory("NodeModelsEssentials")]
    [OutPortNames("Result")]
    [OutPortTypes("double")]
    [OutPortDescriptions("Current value of internal number property.")]
    [IsDesignScriptCompatible]
    public class CustomUINodeModelButton : NodeModel
    {
        #region commands

        public DelegateCommand MyCommand { get; set; }

        #endregion

        #region private members

        private double number;

        #endregion

        #region properties
            
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
        private CustomUINodeModelButton(IEnumerable<PortModel> inPorts, IEnumerable<PortModel> outPorts) : base(inPorts, outPorts)
        {
        }

        public CustomUINodeModelButton()
        {
            RegisterAllPorts();

            MyCommand = new DelegateCommand(SayHello);

            number = 0.0;
        }

        #endregion

        #region public methods

        private static void SayHello(object obj)
        {
            MessageBox.Show("Hello Dynamo!");
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
                    GetAstIdentifierForOutputIndex(0), doubleNode)
            };
        }

        #endregion
    }

    /// <summary>
    /// View customizer for CustomUINodeModel Node Model.
    /// </summary>
    public class CustomUINodeModelViewCustomization : INodeViewCustomization<CustomUINodeModelButton>
    {
        public void CustomizeView(CustomUINodeModelButton model, NodeView nodeView)
        {
            var myWpfView = new MyWpfView();

            nodeView.inputGrid.Children.Add(myWpfView);

            myWpfView.DataContext = model;
        }

        public void Dispose() { }
    }
}
