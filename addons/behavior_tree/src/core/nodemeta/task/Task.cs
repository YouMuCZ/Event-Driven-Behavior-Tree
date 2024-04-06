using Godot;
using Godot.Collections;

[Tool]
public partial class Task : NodeMeta
{
    [NodeMeta] public new string NodeType { get; set; } = "Task";
    [NodeMeta] public new string NodeCategory { get; set; } = "Task";

    public Task()
    {
        
    }

    public Task(BTGraphNode mGraphNode, Dictionary data) : base(mGraphNode, data)
    {

    }
    
    public Task(BehaviorTree behaviorTree, Dictionary data) : base(behaviorTree, data)
    {

    }
    
    protected override void Execute()
    {
        base.Execute();
    }

    protected override void OnStart()
    {
        base.OnStart();
    }

    protected override void OnStop()
    {
        base.OnStop();
    }

    protected override void OnChildFinished(NodeMeta child, bool success)
    {
        base.OnChildFinished(child, success);
    }
}
