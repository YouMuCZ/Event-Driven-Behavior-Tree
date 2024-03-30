using Godot;
using System;
using Godot.Collections;

[Tool]
public partial class Root : NodeMeta
{
    [NodeMeta] public new string NodeType { get; set; } = "Root";
    /// <summary> 节点名称,显示到<see cref="GraphNode"/>的<see cref="GraphNode.Title"/>参数上 </summary>
    [NodeMeta] public new string NodeName { get; set; } = "Root";
    /// <summary> 当前节点类型,注册到<see cref="GraphEdit"/>的<see cref="PopupMenu"/>的子菜单栏中 </summary>
    [NodeMeta] public new string NodeCategory { get; set; } = "Root";
    
    public Root()
    {
        
    }
    
    public Root(BTGraphNode graphNode) : base(graphNode)
    {
        
    }
}
