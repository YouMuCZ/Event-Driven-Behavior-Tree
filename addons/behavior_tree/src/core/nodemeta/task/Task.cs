using Godot;
using Godot.Collections;

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
