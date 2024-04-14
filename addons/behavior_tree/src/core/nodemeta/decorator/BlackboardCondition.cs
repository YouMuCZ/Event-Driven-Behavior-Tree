using Godot;
using System;
using Godot.Collections;

[Tool]
public partial class BlackboardCondition : Condition
{
    [NodeMeta] public override string NodeType { get; set; } = "Blackboard";
    
    
    public BlackboardCondition()
    {
        
    }
    
    public BlackboardCondition(BehaviorTree behaviorTree, Dictionary data) : base(behaviorTree, data)
    {
        
    }
    
    public BlackboardCondition(BehaviorTree behaviorTree, BTGraphNode mGraphNode, Dictionary data) : base(behaviorTree, mGraphNode, data)
    {

    }

}
