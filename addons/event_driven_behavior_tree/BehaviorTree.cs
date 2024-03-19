#if TOOLS
using Godot;


[Tool]
public partial class BehaviorTree : EditorPlugin
{
	private Control _editorInstance;
	private PackedScene _editor = ResourceLoader.Load<PackedScene>("res://addons/event_driven_behavior_tree/scenes/editor.tscn");
		
	public override void _EnterTree()
	{
		// Initialization of the plugin goes here.
		_editorInstance = _editor.Instantiate<Control>();
		AddControlToBottomPanel(_editorInstance, "事件行为树");
	}

	public override void _ExitTree()
	{
		// Clean-up of the plugin goes here.
		_editorInstance?.QueueFree();
	}
}
#endif
