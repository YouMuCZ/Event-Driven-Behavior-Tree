using Godot;
using Godot.Collections;

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
}
