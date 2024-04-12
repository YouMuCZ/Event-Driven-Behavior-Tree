using Godot;
using Godot.Collections;

[Tool]
public partial class Parallel : Composite
{
    [NodeMeta] public override string NodeType { get; set; } = "Parallel";
    
    public Parallel()
    {
        
    }
    
    public Parallel(BehaviorTree behaviorTree, Dictionary data) : base(behaviorTree, data)
    {
        
    }
    
    public Parallel(BehaviorTree behaviorTree, BTGraphNode mGraphNode, Dictionary data) : base(behaviorTree, mGraphNode, data)
    {

    }
}
