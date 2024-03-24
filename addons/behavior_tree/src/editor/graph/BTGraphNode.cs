using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

/// <summary>
/// inherited from GraphNode.
/// </summary>
[Tool]
public partial class BTGraphNode : GraphNode
{
    private BTGraphEdit _graphEdit;
    /// <summary>
    /// 节点的数据结构和业务逻辑
    /// </summary>
    public NodeMeta Meta;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
	    
    }

    public override void _GuiInput(InputEvent @event)
    {
    	base._GuiInput(@event);
    	
    	// 4.2版本没有右键节点事件,所以自己写一个
    	if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Right })
    	{
    		if (!@event.IsPressed()) return;
    		Selected = true;
    	}
    }

    public void Initialize(BTGraphEdit graphEdit)
    {
	    Meta = new NodeMeta(this);
	    
	    _graphEdit = graphEdit;
	    _graphEdit.NodeRemoved += OnNodeRemoved;
    }

    protected virtual void OnNodeRemoved(BTGraphNode node)
    {
	    
    }
}
