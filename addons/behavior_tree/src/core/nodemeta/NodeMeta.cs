using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;
using Godot;
using Godot.Collections;


#if TOOLS
/// <summary>
/// 通过特性在程序集中获取所有注册节点
/// </summary>
[Tool]
public class NodeMetaStorage
{
    public static System.Collections.Generic.Dictionary<string, List<System.Collections.Generic.Dictionary<string, string>>> NodeMetaCategory = new ();
    
    /// <summary>
    /// 初始化右键菜单选项
    /// </summary>
    public static void Setup()
    {
        // 获取程序集
        var assembly = Assembly.GetExecutingAssembly();
        // 获取程序集中的所有类型
        var types = assembly.GetTypes();
        
        // 遍历所有类
        foreach (var type in types)
        {
            if (!type.IsSubclassOf(typeof(NodeMeta))) continue;
            
            string nodeType = null;
            string nodeCategory = null;
            
            // 获取类中已经初始化的参数属性
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                if (property.Name == "NodeCategory") nodeCategory = (string)property.GetValue(Activator.CreateInstance(type));
                if (property.Name == "NodeType") nodeType = (string)property.GetValue(Activator.CreateInstance(type));
                // if (property.Name == "NodeName") nodeName = (string)property.GetValue(Activator.CreateInstance(type));
                
                // if (property.GetCustomAttributes(typeof(NodeMetaAttribute), true).Any())
                // {
                //     if (property.Name == "NodeCategory") nodeCategory = (string)property.GetValue(Activator.CreateInstance(type));
                //     if (property.Name == "NodeType") nodeType = (string)property.GetValue(Activator.CreateInstance(type));
                //     if (property.Name == "NodeName") nodeName = (string)property.GetValue(Activator.CreateInstance(type));
                //     
                //     // GD.Print($"Property Name: {property.Name}, Value:  {property.GetValue(Activator.CreateInstance(type))}");
                // }
            }
            
            // TODO: 后续应该有config专门用来做编辑器设置用
            if (nodeType is null or "Root" || nodeCategory is null) continue;
            
            if (NodeMetaCategory.TryGetValue(nodeCategory, out var categories))
            {
                var map = new System.Collections.Generic.Dictionary<string, string> { { "NodeType", nodeType }};
                categories.Add(map);
            }
            else
            {
                var map = new System.Collections.Generic.Dictionary<string, string> { { "NodeType", nodeType }};
                var newTypes = new List<System.Collections.Generic.Dictionary<string, string>> { map };
                NodeMetaCategory.Add(nodeCategory, newTypes);
            }
        }
    }
}
#endif


/// <summary>
/// 节点的元数据结构,用来存储数据和实现业务逻辑,因为使用了反射来获取程序集里的所有类的NodeMetaAttribute,所以需要初始化构造函数.
/// </summary>
[Tool]
public partial class NodeMeta : Resource
{
    #region Basics

    /// <summary> 根节点 </summary>
    private Root _root;
    /// <summary> Edit graph node </summary>
    private BTGraphNode _graphNode;
    /// <summary> <see cref="BehaviorTree"/> </summary>
    private BehaviorTree _behaviorTree;

    /// <summary> Storage composite's children node. </summary>
    public Array<NodeMeta> Children { set; get; } = new ();

    #endregion
    
    #region Serialie/Deserialize meta
    
    /// <summary>需要存储到Resource本地文件里的参数和参数值。</summary>
    private readonly List<PropertyInfo> _metaPropertyInfo;
    
    /// <summary> 节点类型,将会展示到<see cref="GraphNode.Title"/>作为节点面板名称. </summary>
    [NodeMeta] public string NodeType { get; set; }
    /// <summary> 节点名称,等价于<see cref="GraphNode.Name"/>由于<see cref="GraphEdit"/>是以Name作为节点索引参数. </summary>
    [NodeMeta] public string NodeName
    {
        private set { }
        get => _graphNode?.Name;
    }

    /// <summary> 当前节点类型,注册到<see cref="GraphEdit"/>的<see cref="PopupMenu"/>的子菜单栏中 </summary>
    [NodeMeta] public string NodeCategory { get; set; }

    /// <summary> <see cref="GraphNode.PositionOffset"/> </summary>
    [NodeMeta]
    public Vector2 NodePositionOffset
    {
        private set { }
        get => _graphNode?.PositionOffset ?? Vector2.Zero;
    }
    /// <summary> 执行索引,标记该节点在行为树中被执行的顺序</summary>
    [NodeMeta] public int ExecuteIndex { get; set; }
    
    /// <summary>
    /// 序列化meta数据,用来存储到resource本地文件上
    /// </summary>
    /// <returns></returns>
    public Dictionary Serialize()
    {
        var data = new Dictionary();
    	
        foreach (var property in _metaPropertyInfo)
        {
            data.Add(property.Name, Get(property.Name));
        }

        return data;
    }
    
    /// <summary>
    /// 反序列化数据
    /// </summary>
    /// <param name="data"></param>
    public void Deserialize(Dictionary data)
    {
        if (data == null || _graphNode == null) return;
        
        foreach (var kvp in data)
        {
            Set((string)kvp.Key, kvp.Value);
        }
    }
    
    #endregion

    #region delegate, event, signal
    /// <summary>
    /// 节点运行状态 <see cref="Enums.State"/>
    /// </summary>
    [Signal] public delegate void NodeRunningResultEventHandler(BehaviorTree behaviorTree);

    #endregion

    #region 构造函数

    public NodeMeta()
    {
        
    }

    public NodeMeta(BTGraphNode graphNode, Dictionary data)
    {
        _graphNode = graphNode;
        
        Deserialize(data);
        
        _graphNode.Title = (string)data["NodeType"];
        _graphNode.Name = (string)data["NodeName"]??NodeType;
        _graphNode.PositionOffset = (Vector2)data["NodePositionOffset"];
        
        // 加载用作序列化的meta数据
        _metaPropertyInfo = new List<PropertyInfo>();
        _metaPropertyInfo = GetType().GetProperties()
            .Where(t => t.GetCustomAttributes(typeof(NodeMetaAttribute), true).Any())
            .ToList();
    }
    
    public NodeMeta(Dictionary data)
    {
        Deserialize(data);
    }

    #endregion
    
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
