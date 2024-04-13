using Godot;
using Godot.Collections;

/// <summary>
/// 节点按从上到下的顺序执行其子节点。当其中一个子节点执行成功时，选择器节点将停止执行。如果选择器的一个子节点成功运行，则选择器运行成功。如果选择器的所有子节点运行失败，则选择器运行失败。
/// </summary>
[Tool]
public partial class Selector : Composite
{
    [NodeMeta] public override string NodeType { get; set; } = "Selector";

    public Selector()
    {
        
    }
    
    public Selector(BehaviorTree behaviorTree, Dictionary data) : base(behaviorTree, data)
    {
        
    }
    
    public Selector(BehaviorTree behaviorTree, BTGraphNode mGraphNode, Dictionary data) : base(behaviorTree, mGraphNode, data)
    {
        
    }
    
    protected override void OnChildFinished(NodeMeta child, bool success)
    {
        base.OnChildFinished(child, success);
        
        //子节点运行成功失败则继续下一个,否则该节点运行成功
        if (!success)
        {
            Execute();
        }
        else
        {
            Finish(true);
        }
    }
}
