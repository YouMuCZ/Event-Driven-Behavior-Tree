using System;
using Godot;
using System.Collections.Generic;
using System.Reflection;
using Godot.Collections;

[Tool]
public partial class BehaviorTree : Resource
{
    #region behavior tree parm meta
    
    [Export] public string FileDir;
    [Export] public string Filename;
    [Export] public string Filepath;
    [Export] public string EngineVersion = Engine.GetVersionInfo()["string"].ToString();
    [Export] public string PluginVersion = BehaviorTreePlugin.MConfigFile.GetValue("plugin", "version", "1.0.0").ToString();

    [Export] public Array<Dictionary> NodeData = new (){
        new Dictionary
        {
            {"NodeType", "Root"}, 
            {"NodeName", "Root"}, 
            {"NodeCategory", "Root"},
            {"NodePositionOffset", Vector2.Zero},
        }
    };
    
    [Export] public Array<Dictionary> Connection = new ();
    
    #endregion
    
    public Dictionary NodeMetaClasses = new ();
    public BehaviorTreePlayer MBehaviorTreePlayer;
    
    private Root _root;
    /// <summary> 当前正在运行的节点栈 </summary>
    public Stack<NodeMeta> NodeStack = new();
    
    public void Initialize()
    {
        var namespaceName = typeof(BehaviorTree).Namespace;
        
        // 获取程序集
        var assembly = Assembly.GetExecutingAssembly();
        // 获取程序集中的所有类型
        // var types = assembly.GetTypes();
        //
        // foreach (var type in types)
        // {
        //     if (!type.IsSubclassOf(typeof(NodeMeta))) continue;
        //     
        //     GD.Print(type);
        // }
        
        NodeMetaClasses.Clear();

        foreach (var data in NodeData)
        {
            var nodeName = (string)data["NodeName"]; 
            var nodeType = (string)data["NodeType"];
            
            // 获取类型信息 创建实例
            var type = assembly.GetType($"{namespaceName}.{nodeType}");
            if (type != null)
            {
                var instance = (NodeMeta)Activator.CreateInstance(type, new object[] { this, data });
                
                if (instance is null) continue;
                
                NodeMetaClasses.Add(nodeName, instance);
                
                if (instance is Root r) _root = r;
            }
        }

        foreach (var kvp in NodeMetaClasses)
        {
            var meta = (NodeMeta)kvp.Value;
            meta.Initialize();
        }
    }
    
    /// <summary>
    /// 主动开启当前行为树,如果没有节点或者缺失根节点，则运行失败
    /// </summary>
    /// <returns></returns>
    public bool Start()
    {
        if (_root == null) return false;
        if (_root.Status == Enums.Status.Running) return false;
        
        _root.Start();
        
        return true;
    }
    
    public void Process(double delta)
    {
        if (NodeStack.Count > 0)
        {
            var nodeMeta = NodeStack.Peek();
            nodeMeta.Process(delta);
        }
    }
    
    public void PhysicsProcess(double delta)
    {
        if (NodeStack.Count > 0)
        {
            var nodeMeta = NodeStack.Peek();
            nodeMeta.PhysicsProcess(delta);
        }
    }
    
    /// <summary>
    /// 主动停止当前运行中的行为树
    /// </summary>
    /// <returns></returns>
    public bool Stop()
    {
        if (_root.Status != Enums.Status.Running) return false;

        _root.Stop();
        
        return true;
    }

    public NodeMeta GetNodeByName(string name)
    {
        if (NodeMetaClasses.TryGetValue(name, out var value))
            return (NodeMeta)value;
        
        return null;
    }
    
    public void PushNodeStack(NodeMeta nodeMeta)
    {
        GD.Print("PushNodeStack ", nodeMeta);
        if (!NodeStack.Contains(nodeMeta)) NodeStack.Push(nodeMeta);
    }

    public void PopNodeStack(NodeMeta nodeMeta)
    {
        GD.Print("PopNodeStack ", NodeStack.Peek());
        if (NodeStack.Contains(nodeMeta))  NodeStack.Pop();
    }
}
