using Godot;
using System;

[Tool]
public partial class Selector : Composite
{
    [NodeMeta] public new string NodeName { get; set; } = "Selector";
    [NodeMeta] public new string NodeType { get; set; } = "Selector";
}
