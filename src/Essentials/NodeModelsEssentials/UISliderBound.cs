using System;
using System.Collections.Generic;
using Dynamo.Controls;
using Dynamo.Graph.Nodes;
using Dynamo.UI.Commands;
using Dynamo.Wpf;
using ProtoCore.AST.AssociativeAST;
using Autodesk.DesignScript.Runtime;
using NodeModelsEssentials.Controls;
using NodeModelsEssentials.Functions;
using System.Linq;
using Newtonsoft.Json;

namespace NodeModelsEssentials
{
    [NodeName("UI.SliderBound")]
    [NodeDescription("A sample Node Model with custom Wpf UI.")]
    [NodeCategory("NodeModelsEssentials")]
    [InPortNames("list")]
    [InPortTypes("List<object>")]
    [InPortDescriptions("A list of items.")]
    [OutPortNames(">")]
    [OutPortTypes("object")]
    [OutPortDescriptions("Item")]
    [IsDesignScriptCompatible]
    public class CustomUINodeModelWpfSliderBound : NodeModel
    {
        #region private members

        private int index;
        private int maximumValue;
        private int minimumValue;
        private int step;

        #endregion

        #region properties

        [IsVisibleInDynamoLibrary(false)]
        public DelegateCommand IncreaseCommand { get; set; }

        [IsVisibleInDynamoLibrary(false)]
        public DelegateCommand DecreaseCommand { get; set; }

        public int MinimumValue
        {
            get { return minimumValue; }
            set
            {
                minimumValue = value;
                RaisePropertyChanged("MinimumValue");
            }
        }
        public int MaximumValue
        {
            get { return maximumValue; }
            set
            {
                maximumValue = value;
                RaisePropertyChanged("MaximumValue");
            }
        }

        public int Index
        {
            get { return index; }
            set
            {
                index = value;
                //index = Math.Round(value, 2); ;
                RaisePropertyChanged("Index");

                OnNodeModified();
            }
        }

        #endregion

        #region constructor

        [JsonConstructor]
        private CustomUINodeModelWpfSliderBound(IEnumerable<PortModel> inPorts, IEnumerable<PortModel> outPorts) : base(inPorts, outPorts)
        {
        }

        public CustomUINodeModelWpfSliderBound()
        {
            RegisterAllPorts();

            IncreaseCommand = new DelegateCommand(IncreaseNumber);
            DecreaseCommand = new DelegateCommand(DecreaseNumber);

            MinimumValue = 0;
            MaximumValue = 100;
            step = 10;

            index = 5;
        }

        #endregion

        #region public methods

        [IsVisibleInDynamoLibrary(false)]
        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAsNodes)
        {
            if (!InPorts[0].Connectors.Any())
            {
                return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode()) };
            }

            var indexNode = AstFactory.BuildIntNode(index);
            
            var funcNode = AstFactory.BuildFunctionCall(
                new Func<List<object>, int, object>(NodeModelsEssentialsFunctions.GetItem),
                new List<AssociativeNode>() { inputAsNodes[0], indexNode }
                );

            return new[]
            {
                AstFactory.BuildAssignment(
                    GetAstIdentifierForOutputIndex(0), funcNode)
            };
        }

        #endregion

        #region command methods

        private void IncreaseNumber(object obj)
        {
            if (Index + step >= MaximumValue)
            {
                Index = MaximumValue;
            }
            else
            {
                Index += step;
            }
        }
        private void DecreaseNumber(object obj)
        {

            if (Index - step <= MinimumValue)
            {
                Index = MinimumValue;
            }
            else
            {
                Index += -step;
            }
        }

        #endregion
    }

    /// <summary>
    /// View customizer for CustomUINodeModel Node Model.
    /// </summary>
    public class CustomUINodeModelWpfSliderBoundViewCustomization : INodeViewCustomization<CustomUINodeModelWpfSliderBound>
    {
        public void CustomizeView(CustomUINodeModelWpfSliderBound model, NodeView nodeView)
        {
            var mySliderBound = new SliderBound();
            nodeView.inputGrid.Children.Add(mySliderBound);

            mySliderBound.DataContext = model;
        }

        public void Dispose() { }
    }
}



