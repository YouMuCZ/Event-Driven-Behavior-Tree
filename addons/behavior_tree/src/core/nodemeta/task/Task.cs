using Godot;
using Godot.Collections;

/// <summary>
/// 任务节点的功能是实现操作，例如移动AI或调整黑板值。它们可以连接至 装饰器（Decorators）节点 或 服务（Services）节点。
/// </summary>
[Tool]
public partial class Task : NodeMeta
{
    [NodeMeta] public override string NodeCategory { get; set; } = "Task";

    public Task()
    {
        
    }

    public Task(BehaviorTree behaviorTree, BTGraphNode mGraphNode, Dictionary data) : base(behaviorTree, mGraphNode, data)
    {

    }
    
    public Task(BehaviorTree behaviorTree, Dictionary data) : base(behaviorTree, data)
    {

    }
}
