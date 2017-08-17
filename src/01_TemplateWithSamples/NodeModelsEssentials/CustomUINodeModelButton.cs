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

namespace NodeModelsEssentials.Examples
{
    [NodeName("Essentials.CustomUI.Button")]
    [NodeDescription("A sample Node Model with custom UI.")]
    [NodeCategory("NodeModelsEssentials")]
    [OutPortNames("Result")]
    [OutPortTypes("double")]
    [OutPortDescriptions("Current value of internal number property.")]
    [IsDesignScriptCompatible]
    public class CustomUINodeModelButton : NodeModel
    {
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
                RaisePropertyChanged("Number");

                OnNodeModified();
            }
        }

        #endregion

        #region constructor

        public CustomUINodeModelButton()
        {
            RegisterAllPorts();

            number = 0.0;
        }

        #endregion

        #region public methods

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
            var myButton = new Button();
            myButton.Name = "name";

            nodeView.inputGrid.Children.Add(myButton);

            myButton.Click += MyButton_Click;
            myButton.DataContext = model;
        }

        private void MyButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var button = sender as Button;
            var model = ((button.DataContext) as CustomUINodeModelButton);
            model.Number += 10;
            button.Content = model.Number.ToString();
        }

        public void Dispose() { }
    }
}
