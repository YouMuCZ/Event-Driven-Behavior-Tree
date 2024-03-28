using System;
using System.Linq;
using System.Collections.Generic;
using Godot;
using Godot.Collections;


[Tool]
public partial class BTGraphEdit : GraphEdit
{
	public Signal Modified;
	
	/// <summary>面板右键菜单</summary>
	private PopupMenu _graphPopupMenu;
	/// <summary>节点右键菜单x</summary>
	private PopupMenu _nodePopupMenu;
	/// <summary>鼠标当前位置</summary>
	private Vector2 _cursorPos = Vector2.Zero;
	/// <summary>copy and paste temp data.</summary>
	private Array<Dictionary> _copyNodeData = new();
	public BehaviorTree BehaviorTreeData { get; private set; } = new ();
	private readonly System.Collections.Generic.Dictionary<string, BTGraphNode> _nodes = new ();
	private readonly System.Collections.Generic.Dictionary<string, PackedScene> _nodesScenes = new ()
	{
		{"Root", ResourceLoader.Load<PackedScene>("res://addons/behavior_tree/src/scenes/nodes/root_node.tscn")},
	};
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Initialize();
		Setup();
	}

	private void Initialize()
	{
		// 初始视图位置
		ScrollOffset = - new Vector2(0, Size.Y * 0.4f);
		
		_graphPopupMenu = GetNode<PopupMenu>("GraphPopupMenu");
		_nodePopupMenu = GetNode<PopupMenu>("NodePopupMenu");
		_nodePopupMenu.IdPressed += OnNodePopupMenuPressed;
		
		PopupRequest += OnShowMenu;
		NodeSelected += OnNodeSelected;
		CopyNodesRequest += OnCopyNodesRequest;
		PasteNodesRequest += OnPasteNodesRequest;
		ConnectionToEmpty += OnConnectionToEmpty;
		ConnectionRequest += OnConnectionRequest;
		DisconnectionRequest += OnDisconnectionRequest;
	}
	
	private void Setup()
	{
		foreach (var kvp in NodeMetaStorage.NodeMetaCategory)
		{
			var nodeCategory = kvp.Key;
			var submenu = new PopupMenu();
			submenu.Name = kvp.Key;
			// 使用lambda表达式来适配事件处理器签名
			submenu.IndexPressed += (index) => OnGraphPopupMenuPressed(submenu, (int)index);
			
			var itemIndex = 0;
			foreach (var variable in kvp.Value)
			{
				var nodeType = variable["NodeType"];
				var nodeName = variable["NodeName"];
				
				var data = new Dictionary
				{
					{ "NodeType", nodeType },
					{ "NodeName", nodeName},
					{"NodeCategory", nodeCategory},
					{"NodePositionOffset", Vector2.Zero}
				};
				
				submenu.AddItem(nodeName);
				submenu.SetItemMetadata(itemIndex, data);
				itemIndex += 1;
			}
			
			_graphPopupMenu.AddSubmenuNodeItem(nodeCategory, submenu);
		}
	}

	#region event callback
	/// <summary>
	/// Using temporary copied data to paste nodes
	/// </summary>
	private void OnPasteNodesRequest()
	{
		foreach (var data in _copyNodeData)
		{
			var positionOffset = data["NodePositionOffset"].AsVector2();
			data["NodePositionOffset"] = new Vector2(positionOffset.X + 50, positionOffset.Y + 50);
			
			var nodeType = (string)data["NodeType"];
			switch (nodeType)
			{
				case "Root":
					CreateNode<BTGraphNode>(data);
					break;
			}
		}
	}
	
	/// <summary>
	/// Copy nodes and store temporary data
	/// </summary>
	private void OnCopyNodesRequest()
	{
		var nodeSelected = GetNodeSelected();
		var btGraphNodes = nodeSelected as BTGraphNode[] ?? nodeSelected.ToArray();
		if (!btGraphNodes.Any()) return;
		
		_copyNodeData = new Array<Dictionary>();
		foreach (var node in btGraphNodes)
		{
			_copyNodeData.Add(node.Meta.Serialize());
		}
	}
	
	/// <summary>
	/// Emitted when user drags a connection from an output port into the empty space of the graph.
	/// </summary>
	/// <param name="fromNode"></param>
	/// <param name="fromPort"></param>
	/// <param name="releasePosition"></param>
	private void OnConnectionToEmpty(StringName fromNode, long fromPort, Vector2 releasePosition)
	{
		OnShowMenu(releasePosition);
		NodeCreated += OnNodeCreated;
		
		return;
		
		void OnNodeCreated(BTGraphNode node)
		{
			ConnectNode(fromNode, (int)fromPort, node.Name, 0);
			NodeCreated -= OnNodeCreated;
		}
	}
	
	/// <summary>
	/// <para>Emitted when the given <see cref="Godot.GraphElement"/> node is selected.</para>
	/// </summary>
	private void OnNodeSelected(Node node)
	{
		// 更新检查器面板
		var btGraphNode = (BTGraphNode)node;
		EditorInterface.Singleton.InspectObject(btGraphNode.Meta, inspectorOnly:true);
	}
	
	/// <summary>
	/// <para>Emitted to the GraphEdit when the connection between the <c>fromPort</c> of the <c>fromNode</c> <see cref="Godot.GraphNode"/> and the <c>toPort</c> of the <c>toNode</c> <see cref="Godot.GraphNode"/> is attempted to be created.</para>
	/// </summary>
	private void OnConnectionRequest(StringName fromNode, long fromPort, StringName toNode, long toPort)
	{
		ConnectNode(fromNode, (int)fromPort, toNode, (int)toPort);
	}
	
	/// <summary>
	/// <para>Emitted to the GraphEdit when the connection between <c>fromPort</c> of <c>fromNode</c> <see cref="Godot.GraphNode"/> and <c>toPort</c> of <c>toNode</c> <see cref="Godot.GraphNode"/> is attempted to be removed.</para>
	/// </summary>
	private void OnDisconnectionRequest(StringName fromNode, long fromPort, StringName toNode, long toPort)
	{
		DisconnectNode(fromNode, (int)fromPort, toNode, (int)toPort);
	}

	/// <summary>
	/// Provide a right-click context menu for users to create node instances
	/// </summary>
	/// <param name="position">Mouse local position</param>
	private void OnShowMenu(Vector2 position)
	{
		var pos = position + GlobalPosition + GetWindow().Position;
		var nodeSelected = GetNodeSelected();
		
		if (nodeSelected.Any())
		{
			_nodePopupMenu.Popup(new Rect2I(
				(int)pos.X, (int)pos.Y, _graphPopupMenu.Size.X, _graphPopupMenu.Size.Y)
			);
		}
		else
		{
			_graphPopupMenu.Popup(new Rect2I(
				(int)pos.X, (int)pos.Y, _graphPopupMenu.Size.X, _graphPopupMenu.Size.Y)
			);
		}
		
		// 鼠标位置和节点图的比例偏移量
		_cursorPos = (position + ScrollOffset) / Zoom;
	}
	
	/// <summary>
	/// 面板右键菜单,创建节点
	/// </summary>
	/// <param name="menu"></param>
	/// <param name="index"></param>
	private void OnGraphPopupMenuPressed(PopupMenu menu, int index)
	{
		var data = (Dictionary)menu.GetItemMetadata(index);
		
		// 更新节点位置
		data["NodePositionOffset"] = _cursorPos;
		
		CreateNode<BTGraphNode>(data:data, menu:true);
	}

	private void OnNodePopupMenuPressed(long id)
	{
		switch (id)
		{
			case 0:
				OnCopyNodesRequest();
				break;
			case 1:
				OnPasteNodesRequest();
				break;
			case 2:
				RemoveSelectedNode();
				break;
		}
	}
	
	#endregion
	
	/// <summary>
	/// <para>Create and init graph node <see cref="BTGraphNode"/>. If there has data, then deserialize node data.</para>
	/// </summary>
	/// <param name="data"></param>
	/// <param name="menu"> 右键菜单创建的节点 </param>
	/// <typeparam name="T"></typeparam>
	public T CreateNode<T>(Dictionary data = null, bool menu = false) where T : BTGraphNode
	{
		var nodeScene = _nodesScenes["Root"];
		var newNode = nodeScene.Instantiate<T>();
		
		newNode.Setup(this, data);
		// newNode.Title = $"{newNode.Meta.NodeName}_1";
		newNode.Name = $"{newNode.Meta.NodeName}_1";
		newNode.PositionOffset = _cursorPos;

		AddChild(newNode, true);
		_nodes.Add(newNode.Name, newNode);
		
		NodeCreated?.Invoke(newNode);

		return newNode;
	}
	
	/// <summary>
	/// Remove selected node by node popup menu.
	/// </summary>
	private void RemoveSelectedNode()
	{
		var nodeSelected = GetNodeSelected();
		foreach (var node in nodeSelected)
		{
			RemoveNode(node);
		}
	}
	
	public void RemoveNode(BTGraphNode node)
	{
		if (!IsInstanceValid(node)) return;
		// Can't remove start node.
		if (node.Meta.NodeType == "Root") return;
		if (!_nodes.ContainsKey(node.Name)) return;
		
		_nodes.Remove(node.Name);
		DisconnectNodeConnection(node);
		NodeRemoved?.Invoke(node);
		node.QueueFree();
		RemoveChild(node);
	}
	
	public void RemoveNode(string name)
	{
		var node = GetNodeByName(name);
		if (node == null) return;
		if (!IsInstanceValid(node)) return;
		// Can't remove start node.
		if (node.Meta.NodeType == "Root") return;
		if (!_nodes.ContainsKey(node.Name)) return;
		
		_nodes.Remove(node.Name);
		DisconnectNodeConnection(node);
		NodeRemoved?.Invoke(node);
		node.QueueFree();
		RemoveChild(node);
	}
	
	/// <summary>
	/// Get node by node name from temp nodes in this class.
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	public BTGraphNode GetNodeByName(string name)
	{
		return _nodes.GetValueOrDefault(name, null);
	}
	
	/// <summary>
	/// 返回当前节点的所有连接节点
	/// </summary>
	/// <param name="nodeName"></param>
	/// <returns></returns>
	public List<string> GetNextNodes(string nodeName)
	{
		var nodes = (
			from connection in GetConnectionList() where (string)connection["from"] == nodeName select (string)connection["to"]
			).ToList();
		return nodes;
	}
	
	/// <summary>
	/// Disconnect the node's all connection in graph.
	/// </summary>
	/// <param name="node"><see cref="T:Godot.GraphNode" /></param>
	private void DisconnectNodeConnection(GraphNode node)
	{
		foreach (var c in GetConnectionList())
		{
			var fromNode = (StringName)c["from_node"];
			var fromPort = (int)c["from_port"];
			var toNode = (StringName)c["to_node"];
			var toPort = (int)c["to_port"];
			
			if (fromNode == node.Name || toNode == node.Name)
			{
				DisconnectNode(fromNode, fromPort, toNode, toPort);
			}
		}
	}
	
	/// <summary>
	/// Return all selected nodes.
	/// </summary>
	/// <returns><see cref="BTGraphNode" /></returns>
	private IEnumerable<BTGraphNode> GetNodeSelected()
	{
		var nodeSelected = new List<BTGraphNode>();
		
		foreach (var kvp in _nodes)
		{
			if (!kvp.Value.IsInsideTree() || !kvp.Value.Visible) continue;
			if (kvp.Value.Selected) nodeSelected.Add(kvp.Value);
		}

		return nodeSelected;
	}
	
	/// <summary>
	/// Load deserialized <see cref="BehaviorTreeData" /> from local tres file, and rebuild graph.
	/// </summary>
	/// <param name="data"></param>
	public void LoadData(BehaviorTree data)
	{
		BehaviorTreeData = data;

		Name = data.Filename?.Split(".")[0];

		foreach (var d in data.Nodes)
		{
			var nodeType = (string)d["NodeType"];
			switch (nodeType)
			{
				case "Root":
					CreateNode<BTGraphNodeRoot>(d);
					break;
			}
		}

		// foreach (var kvp in _nodes)
		// {
		// 	kvp.Value.Meta.DeserializeDone();
		// }

		foreach (var c in data.Connection)
		{
			ConnectNode((StringName)c["from_node"], (int)c["from_port"], (StringName)c["to_node"], (int)c["to_port"]);
		}
	}
	
	/// <summary>
	/// Return serialized node-graph data, include graph global param.
	/// </summary>
	/// <returns><see cref="BehaviorTreeData" /></returns>
	public BehaviorTree DumpsData()
	{
		Array<Dictionary> data = new ();
		foreach (var kvp in _nodes.Where(kvp => kvp.Value.IsInsideTree() && kvp.Value.Visible))
		{
			var nodeData = kvp.Value.Meta.Serialize();
			if (nodeData.Any()) data.Add(nodeData);
		}
		BehaviorTreeData.Nodes = data;
		
		BehaviorTreeData.Connection = GetConnectionList();
		return BehaviorTreeData;
	}

	#region delegate && event
	
	public delegate void NodeCreatedEventHandler(BTGraphNode node);

	public event NodeCreatedEventHandler NodeCreated;
	
	public delegate void NodeRemovedEventHandler(BTGraphNode node);

	public event NodeRemovedEventHandler NodeRemoved;

	#endregion
}
