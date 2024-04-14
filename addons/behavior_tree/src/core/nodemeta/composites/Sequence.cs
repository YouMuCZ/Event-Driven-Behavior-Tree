using Godot;
using System;
using Godot.Collections;

/// <summary>
/// 节点按从上到下的顺序执行其子节点。当其中一个子节点失败时，序列节点也将停止执行。如果有子节点失败，那么序列就会失败。如果该序列的所有子节点运行都成功执行，则序列节点成功。
/// </summary>
[Tool]
public partial class Sequence : Composite
{
    [NodeMeta] public override string NodeType { get; set; } = "Sequence";
    
    public Sequence()
    {
        
    }
    
    public Sequence(BehaviorTree behaviorTree, Dictionary data) : base(behaviorTree, data)
    {
        
    }
    
    public Sequence(BehaviorTree behaviorTree, BTGraphNode mGraphNode, Dictionary data) : base(behaviorTree, mGraphNode, data)
    {
        
    }

    protected override void OnChildFinished(NodeMeta child, bool success)
    {
        base.OnChildFinished(child, success);
        
        //子节点运行成功 继续下一个,否则该节点运行失败
        if (success)
        {
            Execute();
        }
        else
        {
            Finish(false);
        }
    }
}
