using Godot;
using Godot.Collections;


[Tool]
public partial class GraphData : Resource
{
    [Export] public string FileDir;
    [Export] public string Filename;
    [Export] public string Filepath;
    [Export] public string EngineVersion = Engine.GetVersionInfo()["string"].ToString();
    [Export] public string PluginVersion = BehaviorTreePlugin.DConfigFile.GetValue("plugin", "version").ToString();

    [Export] public Array<Dictionary> Nodes = new (){new Dictionary {{"NodeType", "RootNode"}},};
    [Export] public Array<Dictionary> Connection = new ();
}
