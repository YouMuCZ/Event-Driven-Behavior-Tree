using Godot;
using Godot.Collections;

/// <summary>
/// 装饰器节点 （在其他行为树系统中也称为条件语句）连接到 合成（Composite） 或 任务（Task） 节点，并定义树中的分支，甚至单个节点是否可以执行。
/// </summary>
[Tool]
public partial class Condition : NodeMeta
{
    [NodeMeta] public override string NodeCategory { get; set; } = "Decorator";
    
    public Condition()
    {
        
    }
    
    public Condition(BehaviorTree behaviorTree, Dictionary data) : base(behaviorTree, data)
    {
        
    }
    
    public Condition(BehaviorTree behaviorTree, BTGraphNode mGraphNode, Dictionary data) : base(behaviorTree, mGraphNode, data)
    {

    }
}
