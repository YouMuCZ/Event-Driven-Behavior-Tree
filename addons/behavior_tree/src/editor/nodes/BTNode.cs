using Godot;
using System;
using Godot.Collections;

[Tool]
public partial class BTNode : NodeMeta
{
    private BTNode _root;

    public BTNode(BTGraphNode owner) : base(owner)
    {
    }
}
