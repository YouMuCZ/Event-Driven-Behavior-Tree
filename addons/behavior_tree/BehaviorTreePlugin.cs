#if TOOLS
using Godot;

/// <summary>
/// 基于事件驱动的行为树编辑器,参考自虚幻5行为树插件,https://dev.epicgames.com/documentation/zh-cn/unreal-engine/behavior-trees-in-unreal-engine
/// </summary>
[Tool]
public partial class BehaviorTreePlugin : EditorPlugin
{
	public static readonly ConfigFile MConfigFile = new ();
		
	private MainWindow _mainPanelInstance;
	private PackedScene _mainPanel = ResourceLoader.Load<PackedScene>("res://addons/behavior_tree/src/editor/view/main_window.tscn");
		
	public override void _EnterTree()
	{
		_mainPanelInstance = _mainPanel.Instantiate<MainWindow>();
		
		// Add the main panel to the editor's main viewport.
		EditorInterface.Singleton.GetEditorMainScreen().AddChild(_mainPanelInstance);
		// Hide the main panel. Very much required.
		_MakeVisible(false);
		NodeMetaStorage.Setup();
			
		MConfigFile.Load("res://addons/behavior_tree/plugin.cfg");
	}

	public override void _ExitTree()
	{
		_mainPanelInstance?.QueueFree();
	}

	public override bool _HasMainScreen()
	{
		return true;
	}

	public override void _MakeVisible(bool visible)
	{
		if (_mainPanelInstance != null)
		{
			_mainPanelInstance.Visible = visible;
		}
	}

	public override string _GetPluginName()
	{
		return "Behavior Tree";
	}

	public override Texture2D _GetPluginIcon()
	{
		return EditorInterface.Singleton.GetEditorTheme().GetIcon("Node", "EditorIcons");
	}
}
#endif
