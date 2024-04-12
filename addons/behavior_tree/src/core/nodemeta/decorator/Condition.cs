using Godot;
using System;

[Tool]
public partial class Condition : NodeMeta
{
    [NodeMeta] public override string NodeType { get; set; } = "Condition";
    [NodeMeta] public override string NodeCategory { get; set; } = "Condition";
}
