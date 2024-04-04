using System.Linq;
using Godot;
using Godot.Collections;

/// <summary>
/// <para>合成（Composite） 节点定义分支的根，以及执行该分支的基本规则。</para>
/// <para>您可以对其应用 装饰器（Decorators）节点，从而修改进入它们分支的条目，甚至取消执行中的条目。</para>
/// <para>此外，它们还可以连接服务（Services）节点，这些服务节点只有在合成节点的子节点正在被执行时才会激活。</para>
/// </summary>
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
