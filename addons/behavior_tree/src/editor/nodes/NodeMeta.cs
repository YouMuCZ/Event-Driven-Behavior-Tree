using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

[Tool]
public partial class NodeMeta : Resource
{
    public BTGraphNode Owner;
    
    #region Serialie/Deserialize meta
    
    /// <summary>需要存储到Resource本地文件里的参数和参数值。</summary>
    private readonly List<PropertyInfo> _metaPropertyInfo;
    /// <summary> 节点类型,需要事先定义 </summary>
    [NodeMeta] public string NodeType { get; private set; }
    [NodeMeta] public string NodeName { set => Owner.Name = value; get => Owner.Name; }
    [NodeMeta] public Vector2 NodePositionOffset { set => Owner.PositionOffset = value; get => Owner.PositionOffset; }
    
    public Dictionary Serialize()
    {
        var data = new Dictionary();
    	
        foreach (var property in _metaPropertyInfo)
        {
            data.Add(property.Name, Get(property.Name));
        }

        return data;
    }

    public void Deserialize(Dictionary data)
    {
        if (data == null) return;
    	
        foreach (var kvp in data)
        {
            Set((string)kvp.Key, kvp.Value);
        }
    }
    
    #endregion

    public NodeMeta(BTGraphNode owner)
    {
        Owner = owner;
        
        NodeType = NodeName.Split("_")[0];
        _metaPropertyInfo = new List<PropertyInfo>();
        
        // 加载用作序列化的meta数据
        _metaPropertyInfo = GetType().GetProperties()
            .Where(t => t.GetCustomAttributes(typeof(NodeMetaAttribute), true).Any())
            .ToList();
    }
}
