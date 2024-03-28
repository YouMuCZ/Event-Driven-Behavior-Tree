using Godot;
using System;
using Godot.Collections;

[Tool]
public partial class RootNodeMeta : NodeMeta
{
    public RootNodeMeta()
    {
        
    }
    
    public RootNodeMeta(BTGraphNode owner, Dictionary data) : base(owner, data)
    {
        
    }
}
