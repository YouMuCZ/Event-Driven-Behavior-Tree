using Godot;
using System;
using Godot.Collections;

[Tool]
public partial class BehaviorTree : Resource
{
    [Export] public string FileDir;
    [Export] public string Filename;
    [Export] public string Filepath;
    [Export] public string EngineVersion = Engine.GetVersionInfo()["string"].ToString();
    [Export] public string PluginVersion = BehaviorTreePlugin.MConfigFile.GetValue("plugin", "version", "1.0.0").ToString();

    [Export] public Array<Dictionary> Nodes = new (){new Dictionary {{"NodeType", "RootNode"}},};
    [Export] public Array<Dictionary> Connection = new ();
    
    private BehaviorTreePlayer _treePlayer;
    public void Initialize(BehaviorTreePlayer treePlayer)
    {
        _treePlayer = treePlayer;
    }

    public bool Start()
    {
        return true;
    }

    public bool Stop()
    {
        return true;
    }
}
