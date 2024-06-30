using Godot;
using Godot.Collections;

[Tool]
public partial class Wait : Task
{
    [NodeMeta] public override string NodeType { get; set; } = "Wait";

    [Export, NodeMeta] public float WaitTime { get; set; } = 0.1f;

    private Timer _timer = new();
    
    public Wait()
    {
        
    }
    
    public Wait(BehaviorTree behaviorTree, Dictionary data) : base(behaviorTree, data)
    {

    }
    
    public Wait(BehaviorTree behaviorTree, BTGraphNode mGraphNode, Dictionary data) : base(behaviorTree, mGraphNode, data)
    {

    }
    
    private void Execute()
    {
        Finish(true);
    }

    protected override void OnStart()
    {
        base.OnStart();
        
        _timer.WaitTime = WaitTime;
        _timer.OneShot = true;
        _timer.Autostart = false;
        _timer.Connect("timeout", new Callable(this, MethodName.Execute));
        
        MBehaviorTreePlayer.AddChild(_timer);
        _timer.Start();
    }

    protected override void Finish(bool success)
    {
        base.Finish(success);
        
        GD.Print(NodeName, " Finished ", WaitTime);
    }
}
