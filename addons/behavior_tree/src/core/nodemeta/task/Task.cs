using Godot;
using System;

[Tool]
public partial class Task : NodeMeta
{
    [NodeMeta] public new string NodeType { get; set; } = "Task";
    [NodeMeta] public new string NodeCategory { get; set; } = "Task";
}
