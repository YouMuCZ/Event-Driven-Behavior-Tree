using Godot;

[Tool]
public partial class Workspace : VBoxContainer
{
    private TabBar _tabBar;
    private TabContainer _tabContainer;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    	_tabBar = GetNode<TabBar>("TabBar");
    	_tabContainer = GetNode<TabContainer>("TabContainer");

    	_tabBar.TabCloseDisplayPolicy = TabBar.CloseButtonDisplayPolicy.ShowAlways;
    	_tabBar.TabSelected += OnTabSelected;
    	_tabBar.TabClosePressed += OnTabClosePressed;
    }

    private void OnTabClosePressed(long tab)
    {
    	_tabBar.RemoveTab((int)tab);
    	_tabContainer.RemoveChild(GetTabEditor((int)tab));
    }

    private void OnTabSelected(long tab)
    {
    	_tabContainer.CurrentTab = (int)tab;
    }

    public void SetCurrentTab(int index)
    {
    	_tabBar.CurrentTab = index;
    	_tabContainer.CurrentTab = index;
    }

    public int GetTabCount()
    {
    	return _tabContainer.GetTabCount();
    }

    public BTGraphEdit GetCurrentEditor()
    {
    	return (BTGraphEdit)_tabContainer.GetCurrentTabControl();
    }
    
    public BTGraphEdit GetTabEditor(int index)
    {
    	return (BTGraphEdit)_tabContainer.GetTabControl(index);
    }
    
    public int GetTabEditor(string filepath)
    {
    	for (var i = 0; i < _tabContainer.GetTabCount(); i++)
    	{
    		var editor = GetTabEditor(i);
    		if (filepath == editor.BehaviorTreeData.Filepath) return i;
    	}

    	return -1;
    }
    
    /// <summary>
    /// 新增节点图到工作区中
    /// </summary>
    /// <param name="btGraphEdit"></param>
    public void AddEditor(BTGraphEdit btGraphEdit)
    {
    	_tabBar.AddTab(btGraphEdit.Name);
    	_tabContainer.AddChild(btGraphEdit);
    }
}
