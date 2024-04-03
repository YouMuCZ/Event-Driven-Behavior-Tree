using Godot;
using System;

[Tool]
public partial class Condition : NodeMeta
{
    [NodeMeta] public new string NodeType { get; set; } = "Condition";
    [NodeMeta] public new string NodeCategory { get; set; } = "Condition";
}
