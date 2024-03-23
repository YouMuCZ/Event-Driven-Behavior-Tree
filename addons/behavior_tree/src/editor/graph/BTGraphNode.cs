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
    protected BTGraphEdit NBtGraphEdit;
    public BTNode Meta = new ();

    #region Serialie/Deserialize meta

    private List<PropertyInfo> _metaPropertyInfo = new();
    
    [NodeMeta] public string NodeType { get; private set; }
    [NodeMeta] public StringName NodeName { set => Name = value; get => Name; }
    [NodeMeta] public Vector2 NodePositionOffset { set => PositionOffset = value; get => PositionOffset; }

    public Dictionary Serialize()
    {
    	var data = new Dictionary();
    	
    	foreach (var property in _metaPropertyInfo)
    	{
    		data.Add(property.Name, Get(property.Name));
    	}

    	return data;
    }

    public virtual void Deserialize(Dictionary data)
    {
    	if (data == null) return;
    	
    	foreach (var kvp in data)
    	{
    		Set((string)kvp.Key, kvp.Value);
    	}
    }

    public virtual void DeserializeDone()
    {
	    
    }

    #endregion
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    	NodeType = Name.ToString().Split("_")[0];
    	
    	// 加载用作序列化的meta数据
    	_metaPropertyInfo = GetType().GetProperties()
    		.Where(t => t.GetCustomAttributes(typeof(NodeMetaAttribute), true).Any())
    		.ToList();
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

    public void SetEditor(BTGraphEdit btGraphEdit)
    {
	    NBtGraphEdit = btGraphEdit;
	    NBtGraphEdit.NodeRemoved += OnNodeRemoved;
    }

    protected virtual void OnNodeRemoved(BTGraphNode node)
    {
	    
    }
}
