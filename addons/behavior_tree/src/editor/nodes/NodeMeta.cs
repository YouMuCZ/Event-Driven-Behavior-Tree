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
            
            var nodeType = "";
            var nodeName = "";
            var nodeCategory = "";
            
            // 获取类中已经初始化的参数属性
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                if (property.GetCustomAttributes(typeof(NodeMetaAttribute), true).Any())
                {
                    if (property.Name == "NodeCategory") nodeCategory = (string)property.GetValue(Activator.CreateInstance(type));
                    if (property.Name == "NodeType") nodeType = (string)property.GetValue(Activator.CreateInstance(type));
                    if (property.Name == "NodeName") nodeName = (string)property.GetValue(Activator.CreateInstance(type));
                    
                    // GD.Print($"Property Name: {property.Name}, Value:  {property.GetValue(Activator.CreateInstance(type))}");
                }
            }
            
            if (nodeCategory == null || nodeType == null) continue;
            
            if (NodeMetaCategory.TryGetValue(nodeCategory, out var categories))
            {
                var map = new System.Collections.Generic.Dictionary<string, string> { { "NodeType", nodeType }, {"NodeName", nodeName} };
                categories.Add(map);
            }
            else
            {
                var map = new System.Collections.Generic.Dictionary<string, string> { { "NodeType", nodeType }, {"NodeName", nodeName} };
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
    protected BTGraphNode Owner;
    
    #region Serialie/Deserialize meta
    
    /// <summary>需要存储到Resource本地文件里的参数和参数值。</summary>
    private readonly List<PropertyInfo> _metaPropertyInfo;
    
    [NodeMeta] public string NodeType { get; set; } = "Root";
    [NodeMeta] public string NodeName { get; set; } = "Root";
    [NodeMeta] public string NodeCategory { get; set; } = "Basic Nodes";
    [NodeMeta] public Vector2 NodePositionOffset { get; set; } = Vector2.Zero;
    
    #endregion

    public NodeMeta()
    {
        
    }

    public NodeMeta(BTGraphNode owner, Dictionary data)
    {
        Owner = owner;
        
        // 加载用作序列化的meta数据
        _metaPropertyInfo = new List<PropertyInfo>();
        _metaPropertyInfo = GetType().GetProperties()
            .Where(t => t.GetCustomAttributes(typeof(NodeMetaAttribute), true).Any())
            .ToList();
    }
    
    public Godot.Collections.Dictionary Serialize()
    {
        var data = new Godot.Collections.Dictionary();
    	
        foreach (var property in _metaPropertyInfo)
        {
            data.Add(property.Name, Get(property.Name));
        }

        return data;
    }

    public void Deserialize(Godot.Collections.Dictionary data)
    {
        if (data == null) return;
    	
        foreach (var kvp in data)
        {
            Set((string)kvp.Key, kvp.Value);
        }
    }
}
