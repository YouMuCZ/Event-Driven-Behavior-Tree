using Godot;
using Godot.Collections;

[Tool]
public partial class Composite : NodeMeta
{
    [NodeMeta] public new string NodeCategory { get; set; } = "Composites";
    
    /// <summary> Storage composite's children node. </summary>
    private Array _children;
    
    // 无参数构造函数
    public Composite()
    {
        // 可以在这里进行初始化操作
    }

    public Composite(BTGraphNode graphNode) : base(graphNode)
    {
        _children = new Array();
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
