#if TOOLS
using Godot;

[Tool]
public partial class MainWindow : Control
{
	public BehaviorTreePlugin Plugin;
	
	private Workspace _workspace;
	private MenuButton _fileMenu;
	private MenuButton _debugMenu;
	private LinkButton _version;
	private FileDialog _newDialog;
	private FileDialog _saveDialog;
	private FileDialog _openDialog;
	private FileManager _fileManager;
	private TextureButton _lPanelButton;
	private VSplitContainer _variablesPanel;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InitMenu();
		InitParam();
		InitEvent();
	}

	/// <summary>
	/// 初始化GUI
	/// </summary>
	private void InitMenu()
	{
		_version     = GetNode<LinkButton>("Main/StatusBar/Version");
		_fileMenu    = GetNode<MenuButton>("Main/ToolBar/FileMenu");
		_debugMenu   = GetNode<MenuButton>("Main/ToolBar/DebugMenu");
		_workspace   = GetNode<Workspace>("Main/Panel/Workspace");
		_fileManager = GetNode<FileManager>("FileManager");
		_newDialog   = GetNode<FileDialog>("Main/Window/NewFile");
		_saveDialog  = GetNode<FileDialog>("Main/Window/SaveFile");
		_openDialog  = GetNode<FileDialog>("Main/Window/OpenFile");
		_lPanelButton = GetNode<TextureButton>("Main/StatusBar/LPanelButton");
		_variablesPanel = GetNode<VSplitContainer>("Main/Panel/Variables");
	}
	
	/// <summary>
	/// 初始化参数
	/// </summary>
	private void InitParam()
	{
		_fileManager.MainWindow = this;
		_fileManager.Workspace = _workspace;
		_version.Text = (string)BehaviorTreePlugin.MConfigFile.GetValue("plugin", "version", "1.0.0");
	}
	
	/// <summary>
	/// 绑定事件
	/// </summary>
	private void InitEvent()
	{
		_newDialog.FileSelected += OnNewFile;
		_openDialog.FileSelected += OnOpenFile;
		_saveDialog.FileSelected += OnSaveAsFile;
		
		_fileMenu.GetPopup().IndexPressed += OnFileMenuPressed;
		_lPanelButton.Pressed += OnLPanelButtonPressed;
		
		// 注册快捷键
		var shortcut = new Shortcut();
		var saveEvent = new InputEventKey()
		{
			CtrlPressed = true, // 按下Ctrl键
			ShiftPressed = false,  // 未按下Shift键
			AltPressed = false,    // 未按下Alt键
			Keycode = Key.S, // 按下S键
		};
		shortcut.Events.Add(saveEvent);
		_fileMenu.GetPopup().SetItemShortcut(2, shortcut);
	}

	private void OnLPanelButtonPressed()
	{
		_variablesPanel.Visible = !_variablesPanel.Visible;
	}

	/// <summary>
	/// 文件菜单
	/// </summary>
	/// <param name="index">0:新建 1:打开 2:保存 3:另存为</param>
	private void OnFileMenuPressed(long index)
	{
		switch (index)
		{
			case 0:
				_newDialog.PopupCentered();
				break;
			case 1:
				_openDialog.PopupCentered();
				break;
			case 2:
				_fileManager.SaveFile();
				break;
			case 3:
				_saveDialog.PopupCentered();
				break;
		}
	}

	private void OnNewFile(string filepath)
	{
		_fileManager.NewFile(_newDialog.CurrentDir, _newDialog.CurrentFile, filepath);
	}

	private void OnOpenFile(string filepath)
	{
		_fileManager.OpenFile(_openDialog.CurrentDir, _openDialog.CurrentFile, filepath);
	}

	private void OnSaveAsFile(string filepath)
	{
		_fileManager.SaveFile(filepath);
	}
}
#endif
