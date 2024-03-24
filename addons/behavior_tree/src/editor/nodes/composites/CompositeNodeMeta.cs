using Godot;
using Godot.Collections;

[Tool]
public partial class CompositeNodeMeta : NodeMeta
{
    /// <summary> Storage composite's children node. </summary>
    private Array<NodeMeta> _children = new();
    
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

    public CompositeNodeMeta(BTGraphNode owner) : base(owner)
    {
    }
}
