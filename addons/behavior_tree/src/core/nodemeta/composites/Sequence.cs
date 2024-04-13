using Godot;
using System;
using Godot.Collections;

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
