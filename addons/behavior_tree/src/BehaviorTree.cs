using Godot;
using System;
using System.Collections;
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
        new Dictionary { {"NodeType", "Root"}, {"NodeName", "Root"}, {"NodeCategory", "Root"} }
    };
    
    [Export] public Array<Dictionary> Connection = new ();
    
    #endregion
    
    private BehaviorTreePlayer _treePlayer;
    private Dictionary _nodeMetas = new ();
    
    public void Initialize(BehaviorTreePlayer treePlayer)
    {
        _treePlayer = treePlayer;

        foreach (var data in NodeData)
        {
            var meta = new NodeMeta(data);
            _nodeMetas.Add(meta.NodeName, meta);
        }
    }

    public bool Start()
    {
        return true;
    }

    public void Interrupt()
    {
        
    }

    public bool Stop()
    {
        return true;
    }
}
