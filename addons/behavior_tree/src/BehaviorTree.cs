using Godot;
using System.Collections.Generic;
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
    
    private BehaviorTreePlayer _treePlayer;
    private Root _root;
    private Dictionary _nodeMetas = new ();
    /// <summary> 当前正在运行的节点栈 </summary>
    private readonly Stack<NodeMeta> _nodeStack = new();
    
    public void Initialize(BehaviorTreePlayer treePlayer)
    {
        _treePlayer = treePlayer;

        foreach (var data in NodeData)
        {
            NodeMeta meta;
            if ((string)data["NodeType"] == "Root")
            {
                meta = _root = new Root(this, data);
            }
            else
            {
                meta = new NodeMeta(this, data);
            }
            
            _nodeMetas.Add(meta.NodeName, meta);

            // if (meta.NodeName == "Root") _root = (Root)meta;
        }
        
        _nodeStack.Push(_root);
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
        if (_nodeMetas.TryGetValue(name, out var value))
            return (NodeMeta)value;
        
        return null;
    }
}
