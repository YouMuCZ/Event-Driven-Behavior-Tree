using Godot;
using System;
using Godot.Collections;

[Tool]
public partial class Root : NodeMeta
{
    [NodeMeta] public new string NodeType { get; set; } = "Root";
    [NodeMeta] public new string NodeCategory { get; set; } = "Root";

    protected override void OnStart()
    {
        base.OnStart();
        
        if (Children == null || Children.Count == 0) return;

        MBehaviorTree.GetNodeByName(Children[0])?.Start();
    }

    protected override void OnStop()
    {
        base.OnStop();
        
        if (Children == null || Children.Count == 0) return;

        MBehaviorTree.GetNodeByName(Children[0])?.Stop();
    }
}
