using Godot;
using System;

[Tool]
public partial class Sequence : Composite
{
    [NodeMeta] public new string NodeName { get; set; } = "Sequence";
    [NodeMeta] public new string NodeType { get; set; } = "Sequence";
}
