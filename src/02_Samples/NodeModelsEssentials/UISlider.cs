using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Dynamo.Controls;
using Dynamo.Graph.Nodes;
using Dynamo.UI.Commands;
using Dynamo.Wpf;
using ProtoCore.AST.AssociativeAST;
using Autodesk.DesignScript.Runtime;
using NodeModelsEssentials.Controls;
using System.Linq;

namespace NodeModelsEssentials.Examples
{
    [NodeName("UI.Slider")]
    [NodeDescription("A sample Node Model with custom Wpf UI.")]
    [NodeCategory("NodeModelsEssentials")]
    [OutPortNames(">")]
    [OutPortTypes("double")]
    [OutPortDescriptions("Double")]
    [IsDesignScriptCompatible]
    public class CustomUIWpfNodeModel : NodeModel
    {
        #region private members

        private double number;
        private double maximumValue;
        private double minimumValue;
        private double step;

        #endregion

        #region properties

        [IsVisibleInDynamoLibrary(false)]
        public DelegateCommand IncreaseCommand { get; set; }

        [IsVisibleInDynamoLibrary(false)]
        public DelegateCommand DecreaseCommand { get; set; }

        public double MinimumValue
        {
            get { return minimumValue; }
            set
            {
                minimumValue = value;
                RaisePropertyChanged("MinimumValue");
            }
        }
        public double MaximumValue
        {
            get { return maximumValue; }
            set
            {
                maximumValue = value;
                RaisePropertyChanged("MaximumValue");
            }
        }

        public double Number
        {
            get { return number; }
            set
            {
                number = Math.Round(value, 2); ;
                RaisePropertyChanged("Number");

                OnNodeModified();
            }
        }

        #endregion

        #region constructor

        public CustomUIWpfNodeModel()
        {
            RegisterAllPorts();

            IncreaseCommand = new DelegateCommand(IncreaseNumber);
            DecreaseCommand = new DelegateCommand(DecreaseNumber);

            MinimumValue = 0.0;
            MaximumValue = 100.0;
            step = 10.0;
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

        #region command methods

        private void IncreaseNumber(object obj)
        {
            if (Number + step >= MaximumValue)
            {
                Number = MaximumValue;
            } else
            {
                Number += step;
            }
        }
        private void DecreaseNumber(object obj)
        {

            if (Number - step <= MinimumValue)
            {
                Number = MinimumValue;
            } else
            {
                Number += -step;
            }
        }

        #endregion
    }

    /// <summary>
    /// View customizer for CustomUINodeModel Node Model.
    /// </summary>
    public class CustomUIWpfNodeModelViewCustomization : INodeViewCustomization<CustomUIWpfNodeModel>
    {
        public void CustomizeView(CustomUIWpfNodeModel model, NodeView nodeView)
        {
            var myFirstDynamoControl = new MyFirstDynamoControl();
            nodeView.inputGrid.Children.Add(myFirstDynamoControl);

            myFirstDynamoControl.DataContext = model;
        }

        public void Dispose() { }
    }
}
