using Godot;
using System;

[Tool]
public partial class Parallel : Composite
{
    [NodeMeta] public new string NodeName { get; set; } = "Parallel";
    [NodeMeta] public new string NodeType { get; set; } = "Parallel";
}
