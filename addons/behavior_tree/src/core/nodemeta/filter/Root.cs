using Godot;
using System;
using Godot.Collections;

[Tool]
public partial class Root : NodeMeta
{
    [NodeMeta] public new string NodeType { get; set; } = "Root";
    [NodeMeta] public new string NodeCategory { get; set; } = "Root";
}
