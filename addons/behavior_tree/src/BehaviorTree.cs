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

    [Export] public Array<Dictionary> Nodes = new (){
        new Dictionary { {"NodeType", "Root"}, {"NodeName", "Root"}, {"NodeCategory", "Root"} }
    };
    
    [Export] public Array<Dictionary> Connection = new ();
    
    #endregion
    
    private BehaviorTreePlayer _treePlayer;
    
    public void Initialize(BehaviorTreePlayer treePlayer)
    {
        _treePlayer = treePlayer;
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
