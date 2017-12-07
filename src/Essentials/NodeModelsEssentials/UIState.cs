using System;
using System.Collections.Generic;
using Dynamo.Controls;
using Dynamo.Graph.Nodes;
using Dynamo.UI.Commands;
using Dynamo.Wpf;
using Dynamo.Events;
using ProtoCore.AST.AssociativeAST;
using Autodesk.DesignScript.Runtime;
using NodeModelsEssentials.Controls;
using NodeModelsEssentials.Functions;
using Newtonsoft.Json;

namespace NodeModelsEssentials
{
    [NodeName("UI.State")]
    [NodeDescription("A sample Node Model with custom Wpf UI.")]
    [NodeCategory("NodeModelsEssentials")]
    [OutPortNames("Number", "Date")]
    [OutPortTypes("double", "string")]
    [OutPortDescriptions("Double", "Date")]
    [IsDesignScriptCompatible]
    public class CustomUINodeModelWpfState : NodeModel
    {
        #region private members

        private double number;
        private double maximumValue;
        private double minimumValue;
        private double step;
        private string state;
        private double executions;
        //private double xPosition;

        #endregion

        #region properties

        [IsVisibleInDynamoLibrary(false)]
        public DelegateCommand IncreaseCommand { get; set; }

        [IsVisibleInDynamoLibrary(false)]
        public DelegateCommand DecreaseCommand { get; set; }

        public string State
        {
            get { return state;  }
            set
            {
                state = value;
                RaisePropertyChanged("State");
            }
        }

        public double Executions
        {
            get { return executions;  }
            set
            {
                executions = value;
                RaisePropertyChanged("Executions");
            }
        }

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

                //OnNodeModified();
            }
        }

        #endregion

        #region constructor

        [JsonConstructor]
        private CustomUINodeModelWpfState(IEnumerable<PortModel> inPorts, IEnumerable<PortModel> outPorts) : base(inPorts, outPorts)
        {
        }

        public CustomUINodeModelWpfState()
        {
            RegisterAllPorts();

            IncreaseCommand = new DelegateCommand(IncreaseNumber);
            DecreaseCommand = new DelegateCommand(DecreaseNumber);

            MinimumValue = 0.0;
            MaximumValue = 100.0;
            step = 10.0;
            state = "Initialized";

            // Create an event handler for pre- or post-graph execution
            // Note that you need to include using Dynamo.Events; at the top of your file
            ExecutionEvents.GraphPostExecution += ExecutionEvents_GraphPostExecution;
            ExecutionEvents.GraphPreExecution += ExecutionEvents_GraphPreExecution;
            CanUpdatePeriodically = true;
        }

        private void ExecutionEvents_GraphPreExecution(Dynamo.Session.IExecutionSession session)
        {
            Number++;
        }

        private void ExecutionEvents_GraphPostExecution(Dynamo.Session.IExecutionSession session)
        {
            Executions++;
            this.OnNodeModified(forceExecute: true);
        }

        #endregion

        #region public methods

        [IsVisibleInDynamoLibrary(false)]
        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAsNodes)
        {
            var doubleNode = AstFactory.BuildDoubleNode(Number);
            var dateFuncNode = AstFactory.BuildFunctionCall(
                new Func<string>(NodeModelsEssentialsFunctions.GetDate),
                new List<AssociativeNode>() { }

                );

            //var funcNode = AstFactory.BuildFunctionCall(
            //    new Func<double, double, double>(NodeModelsEssentialsFunctions.Multiply),
            //    new List<AssociativeNode>() { doubleNode, doubleNode }
            //    );

            return new[]
            {
                AstFactory.BuildAssignment(
                    GetAstIdentifierForOutputIndex(0), doubleNode),
                AstFactory.BuildAssignment(
                    GetAstIdentifierForOutputIndex(1), dateFuncNode)
            };
        }

        #endregion

        #region command methods

        private void IncreaseNumber(object obj)
        {
            if (Number + step >= MaximumValue)
            {
                Number = MaximumValue;
            }
            else
            {
                Number += step;
            }
        }
        private void DecreaseNumber(object obj)
        {

            if (Number - step <= MinimumValue)
            {
                Number = MinimumValue;
            }
            else
            {
                Number += -step;
            }
        }

        #endregion
    }

    /// <summary>
    /// View customizer for CustomUINodeModel Node Model.
    /// </summary>
    public class CustomUINodeModelWpfStateViewCustomization : INodeViewCustomization<CustomUINodeModelWpfState>
    {
        public void CustomizeView(CustomUINodeModelWpfState model, NodeView nodeView)
        {
            var stateDynamoControl = new StateDynamoControl();
            nodeView.inputGrid.Children.Add(stateDynamoControl);
            stateDynamoControl.DataContext = model;
        }

        public void Dispose() { }
    }
}
