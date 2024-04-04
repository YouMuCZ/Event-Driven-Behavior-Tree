using Godot;
using System;

[Tool]
public partial class Wait : Task
{
    [NodeMeta] public new string NodeType { get; set; } = "Wait";
}
