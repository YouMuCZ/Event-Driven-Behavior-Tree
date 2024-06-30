#if TOOLS
using Godot;

[Tool, GlobalClass, Icon("res://addons/Dialogue/resources/icons/file_manager.svg")]
public partial class FileManager : Node
{
	public MainWindow MainWindow;
	public Workspace Workspace;
	
	private PackedScene _graphEdit = ResourceLoader.Load<PackedScene>("res://addons/behavior_tree/src/editor/view/graph/graph_eidt.tscn");
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}
	
	public void NewFile(string dir, string filename, string filepath)
	{
		var tabIndex = Workspace.GetTabEditor(filepath);
		
		if (tabIndex >= 0)
		{
			Workspace.SetCurrentTab(tabIndex);
		}
		else
		{
			CreateFile(dir, filename, filepath);
		}
	}

	public void OpenFile(string dir, string filename, string filepath)
	{
		/*
		var filepath = Path.Combine(dir, filename).Replace("\\", "/");
		*/
		var tabIndex = Workspace.GetTabEditor(filepath);
		
		if (tabIndex >= 0)
		{
			Workspace.SetCurrentTab(tabIndex);
		}
		else
		{
			var data = ResourceLoader.Load<BehaviorTree>(filepath, "", ResourceLoader.CacheMode.Replace);
			data.FileDir = dir;
			data.Filename = filename;
			data.Filepath = filepath;
			
			var editor = _graphEdit.Instantiate<BTGraphEdit>();
			editor.LoadData(data);
			Workspace.AddEditor(editor);
		}
		
		GD.Print("打开文件:", filename);
	}

	/// <summary>
	/// Create local res file.
	/// </summary>
	/// <param name="dir"></param>
	/// <param name="filename"></param>
	/// <param name="filepath"></param>
	public void CreateFile(string dir, string filename, string filepath)
	{
		var editor = _graphEdit.Instantiate<BTGraphEdit>();
		var data = new BehaviorTree()
		{
			FileDir = dir,
			Filename = filename,
			Filepath = filepath,
		};
		
		editor.LoadData(data);
		Workspace.AddEditor(editor);
		ResourceSaver.Save(data, filepath);
		
		GD.Print("新建文件: ", filepath);
	}

	public void SaveFile()
	{
		var editor = Workspace.GetCurrentEditor();
		
		if (editor == null) return;
		
		var data = editor.DumpsData();
		var error = ResourceSaver.Save(data, data.Filepath);
			
		if (error == Error.Ok)
		{
			GD.Print("Resource saved successfully!");
		}
		else
		{
			GD.Print("Failed to save resource. Error: " + error);
		}
	}

	public void SaveFile(string filepath)
	{
		var tabIndex = Workspace.GetTabEditor(filepath);
		if (tabIndex >= 0)
		{
			var editor = Workspace.GetTabEditor(tabIndex);
			var error = ResourceSaver.Save(editor.DumpsData(), filepath);
			
			if (error == Error.Ok)
			{
				GD.Print("Resource saved successfully!");
			}
			else
			{
				GD.Print("Failed to save resource. Error: " + error);
			}
		}
	}
}
#endif
