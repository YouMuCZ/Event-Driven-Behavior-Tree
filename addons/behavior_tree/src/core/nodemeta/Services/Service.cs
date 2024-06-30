using Godot;
using Godot.Collections;

/// <summary>
/// <para>服务(Service)节点通常连接至合成(<see cref="Composite"/>)节点或任务(<see cref="Task"/>)节点，只要其分支被执行，它们就会以定义的频率执行。</para>
/// <para>这些节点常用于检查和更新黑板。它们取代了其他行为树系统中的传统平行（Parallel）节点。</para>
/// </summary>
[Tool]
public partial class Service : NodeMeta
{
    [NodeMeta] public override string NodeCategory { get; set; } = "Service";

    public Service()
    {
        
    }

    public Service(BehaviorTree behaviorTree, BTGraphNode mGraphNode, Dictionary data) : base(behaviorTree, mGraphNode, data)
    {

    }
    
    public Service(BehaviorTree behaviorTree, Dictionary data) : base(behaviorTree, data)
    {

    }
}
