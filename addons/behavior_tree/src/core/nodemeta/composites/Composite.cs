using System.Linq;
using Godot;
using Godot.Collections;

[Tool]
public partial class Composite : NodeMeta
{
    [NodeMeta] public new string NodeCategory { get; set; } = "Composites";

    // 无参数构造函数
    public Composite()
    {
        // 可以在这里进行初始化操作
    }

    public Composite(Dictionary graphNode) : base(graphNode)
    {
        
    }
}
