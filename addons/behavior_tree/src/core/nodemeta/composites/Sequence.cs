using Godot;
using System;
using Godot.Collections;

[Tool]
public partial class Sequence : Composite
{
    [NodeMeta] public new string NodeType { get; set; } = "Sequence";

    private int _childExecuteIndex = -1;
    
    public Sequence()
    {
        
    }
    
    public Sequence(BehaviorTree behaviorTree, Dictionary data) : base(behaviorTree, data)
    {
        
    }

    protected override void Execute()
    {
        base.Execute();
        
        _childExecuteIndex++;

        if (_childExecuteIndex >= Children.Count)
        {
            Finish(true);
            return;
        }
        
        MBehaviorTree?.GetNodeByName(Children[_childExecuteIndex])?.Start();
    }

    protected override void OnStart()
    {
        base.OnStart();
        
        _childExecuteIndex = -1;
        
        Execute();
    }

    protected override void OnStop()
    {
        base.OnStop();
        
        MBehaviorTree?.GetNodeByName(Children[_childExecuteIndex])?.Stop();
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
