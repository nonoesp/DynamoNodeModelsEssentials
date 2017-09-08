using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Dynamo.Controls;
using Dynamo.Graph.Nodes;
using Dynamo.Events;
using Dynamo.UI.Commands;
using Dynamo.Wpf;
using ProtoCore.AST.AssociativeAST;
using Autodesk.DesignScript.Runtime;
using NodeModelsEssentials.Controls;
using NodeModelsEssentials.Functions;
using System.Linq;
using Dynamo.Graph.Workspaces;
using VMDataBridge;

namespace NodeModelsEssentials.Examples
{
    // This component uses the data bridging strategy
    // of the original Watch Dynamo node.

    [NodeName("UI.CopyableWatch")]
    [NodeDescription("A watch node from which you can copy text =).")]
    [NodeCategory("NodeModelsEssentials")]
    [InPortNames(">")]
    [InPortTypes("object")]
    [InPortDescriptions("An object.")]
    [OutPortNames(">")]
    [OutPortTypes("object")]
    [OutPortDescriptions("Item")]
    [IsDesignScriptCompatible]
    public class CustomUINodeModelCopyableWatch : NodeModel
    {
        #region private members

        private string text;
        private int index;

        #endregion

        #region properties

        public Action Executed;

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                RaisePropertyChanged("Text");
                this.OnNodeModified();
            }
        }

        #endregion

        #region constructor

        public CustomUINodeModelCopyableWatch()
        {
            Executed = new Action(() => {
                //MessageBox.Show("Executed!");
            });

            RegisterAllPorts();

            Text = "MyTextChanged";
            //IncreaseCommand = new DelegateCommand(IncreaseNumber);
            //DecreaseCommand = new DelegateCommand(DecreaseNumber);
            ExecutionEvents.GraphPostExecution += ExecutionEvents_GraphPostExecution;
            ExecutionEvents.GraphPreExecution += ExecutionEvents_GraphPreExecution;
            
        }

        private void ExecutionEvents_GraphPreExecution(Dynamo.Session.IExecutionSession session)
        {
            //session.
           
            if (InPorts[0].Connectors.Any())
            {
                // this.GetValue()
                // get parent
               // MessageBox.Show("There is an input! " + InPorts[0].Owner.GetValue(;
                //return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode()) };
            }
        }

        private void ExecutionEvents_GraphPostExecution(Dynamo.Session.IExecutionSession session)
        {
            //index++;
            //text += index.ToString();
        }



        #endregion

        #region public methods

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
            var str = data as string;
            if(str != null)
            {
                Text = str;
            } else
            {
                Text = "";
            }
            if(data is Autodesk.DesignScript.Geometry.Point && (data as Autodesk.DesignScript.Geometry.Point) != null)
            {
                Text = (data as Autodesk.DesignScript.Geometry.Point).ToString();
            }
            this.Executed();
        }

        [IsVisibleInDynamoLibrary(false)]
        public override IEnumerable<AssociativeNode> BuildOutputAst(List<AssociativeNode> inputAsNodes)
        {
            //if (!InPorts[0].Connectors.Any())
            //{
            //    return new[] { AstFactory.BuildAssignment(GetAstIdentifierForOutputIndex(0), AstFactory.BuildNullNode()) };
            //}

            var textNode = AstFactory.BuildStringNode(text);
            
            //var funcNode = AstFactory.BuildFunctionCall(
            //    new Func<List<object>, int, object>(NodeModelsEssentialsFunctions.SetTextValue),
            //    new List<AssociativeNode>() { inputAsNodes[0], textNode }
            //    );

            return new[]
            {
                AstFactory.BuildAssignment(
                    GetAstIdentifierForOutputIndex(0),
                    textNode),
                AstFactory.BuildAssignment(
                    AstFactory.BuildIdentifier(AstIdentifierBase + "_dummy"),
                    DataBridge.GenerateBridgeDataAst(GUID.ToString(), inputAsNodes[0]))
            };
        }

        #endregion

        #region command methods

        //private void IncreaseNumber(object obj)
        //{
        //    if (Index + step >= MaximumValue)
        //    {
        //        Index = MaximumValue;
        //    }
        //    else
        //    {
        //        Index += step;
        //    }
        //}
        //private void DecreaseNumber(object obj)
        //{

        //    if (Index - step <= MinimumValue)
        //    {
        //        Index = MinimumValue;
        //    }
        //    else
        //    {
        //        Index += -step;
        //    }
        //}

        #endregion
    }

    /// <summary>
    /// View customizer for CustomUINodeModel Node Model.
    /// </summary>
    public class CustomUINodeModelCopyableWatchViewCustomization : INodeViewCustomization<CustomUINodeModelCopyableWatch>
    {
        public void CustomizeView(CustomUINodeModelCopyableWatch model, NodeView nodeView)
        {
            var copyableWatch = new CopyableWatch();
            nodeView.inputGrid.Children.Add(copyableWatch);
            nodeView.inputGrid.ClipToBounds = true;
            
            //nodeView.MouseLeave += (sender, args) => { MessageBox.Show("MouseLeave"); };

            copyableWatch.DataContext = model;
            // add an event to watch properties of the workspace and set model.runtype
            //(nodeView.ViewModel.DynamoViewModel.Model.CurrentWorkspace as HomeWorkspaceModel).RunSettings.RunType

        }

        public void Dispose() { }
    }
}



