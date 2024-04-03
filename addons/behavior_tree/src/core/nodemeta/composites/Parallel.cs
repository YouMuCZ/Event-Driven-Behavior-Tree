using Godot;
using System;

[Tool]
public partial class Parallel : Composite
{
    [NodeMeta] public new string NodeType { get; set; } = "Parallel";
}
