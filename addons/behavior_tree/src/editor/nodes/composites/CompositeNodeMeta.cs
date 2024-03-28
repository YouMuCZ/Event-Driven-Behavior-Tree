using Godot;
using Godot.Collections;

[Tool]
public partial class CompositeNodeMeta : NodeMeta
{
    [NodeMeta] public new string NodeName { get; set; } = "Composite";
    [NodeMeta] public new string NodeType { get; set; } = "Composite";
    [NodeMeta] public new string NodeCategory { get; set; } = "Composites";
    
    /// <summary> Storage composite's children node. </summary>
    private Array<NodeMeta> _children;
    
    // 无参数构造函数
    public CompositeNodeMeta()
    {
        // 可以在这里进行初始化操作
    }

    public CompositeNodeMeta(BTGraphNode owner, Dictionary data) : base(owner, data)
    {
        _children = new Array<NodeMeta>();
    }
    
    public override Array<Dictionary> _GetPropertyList()
    {
        var properties = new Array<Dictionary>
        {
            new()
            {
                { "name", "_children" },
                { "type", (int)Variant.Type.Array },
                { "hint", (int)PropertyHint.ResourceType },
                { "usage", (int)PropertyUsageFlags.Storage},
                { "hint_string", "NodeMeta" },
            }
        };

        return properties;
    }
}
