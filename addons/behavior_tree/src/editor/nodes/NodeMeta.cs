using Godot;

[Tool]
public partial class NodeMeta : Resource
{
    [Export] public StringName Name { set; get; }
}
