using Godot;
using System;

[Tool]
public partial class Condition : NodeMeta
{
    [NodeMeta] public new string NodeType { get; set; } = "Condition";
    /// <summary> 节点名称,显示到<see cref="GraphNode"/>的<see cref="GraphNode.Title"/>参数上 </summary>
    [NodeMeta] public new string NodeName { get; set; } = "Condition";
    /// <summary> 当前节点类型,注册到<see cref="GraphEdit"/>的<see cref="PopupMenu"/>的子菜单栏中 </summary>
    [NodeMeta] public new string NodeCategory { get; set; } = "Condition";
}
