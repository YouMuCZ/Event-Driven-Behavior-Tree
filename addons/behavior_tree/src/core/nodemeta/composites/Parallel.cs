using System;
using Godot;
using Godot.Collections;

/// <summary>
/// 节点允许一个主任务节点沿整个的行为树执行。主任务完成后， 结束模式（Finish Mode） 中的设置会指示该节点是应该立即结束，同时中止次要树，还是应该推迟结束，直到次要树完成。
/// </summary>
[Tool]
public partial class Parallel : Composite
{
    public enum FinishMode
    {
        Immediate,  // 主任务完成后，后台树的执行将立即中止。
        Delayed,  // 主任务完成后，允许后台树继续执行直至完成。
        Success,  // 任意一个子节点成功，则此节点成功，否则当所有子节点失败时，此节点失败
        Failure,  // 任意一个子节点失败，则此节点失败，否则当所有子节点成功时，此节点成功
    }
    
    [NodeMeta] public override string NodeType { get; set; } = "Parallel";
    [NodeMeta] public FinishMode Mode { get; set; } = FinishMode.Success;

    private int _successCount;
    private int _failureCount;
    
    public Parallel()
    {
        
    }
    
    public Parallel(BehaviorTree behaviorTree, Dictionary data) : base(behaviorTree, data)
    {
        
    }
    
    public Parallel(BehaviorTree behaviorTree, BTGraphNode mGraphNode, Dictionary data) : base(behaviorTree, mGraphNode, data)
    {

    }
    
    protected override void OnStart()
    {
        base.OnStart();
        
        _successCount = 0;
        _failureCount = 0;
        
        foreach (var child in MChildrenInstance)
        {
            child.Start();
        }
    }
    
    protected override void OnStop()
    {
        base.OnStop();
        
        foreach (var child in MChildrenInstance)
        {
            child.Stop();
        }
    }
    
    protected override void OnChildFinished(NodeMeta child, bool success)
    {
        base.OnChildFinished(child, success);

        ChildExecuteIndex++;

        if (success) _successCount++;
        else _failureCount++;
        
        switch (Mode)
        {
            case FinishMode.Immediate:
                break;
            case FinishMode.Delayed:
                break;
            case FinishMode.Success:
                if (_successCount > 0)
                {
                    Stop();
                    Finish(true);
                }

                if (ChildExecuteIndex >= MChildrenInstance.Count)
                {
                    Stop();
                    Finish(false);
                }
                break;
            case FinishMode.Failure:
                if (_failureCount > 0)
                {
                    Stop();
                    Finish(false);
                }
                
                if (ChildExecuteIndex >= MChildrenInstance.Count)
                {
                    Stop();
                    Finish(true);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
