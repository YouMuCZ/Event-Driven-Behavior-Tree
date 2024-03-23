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
		public BTGraphData GraphData { get; private set; } = new ();
		private readonly System.Collections.Generic.Dictionary<string, BTGraphNode> _nodes = new ();
		private readonly System.Collections.Generic.Dictionary<string, PackedScene> _nodesScenes = new ()
		{
			{"RootNode", ResourceLoader.Load<PackedScene>("res://addons/behavior_tree/src/scenes/nodes/root_node.tscn")},
		};
		
		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			Initialize();
		}

		private void Initialize()
		{
			// 初始视图位置
			ScrollOffset = - new Vector2(0, Size.Y * 0.4f);
			
			_graphPopupMenu = GetNode<PopupMenu>("GraphPopupMenu");
			_nodePopupMenu = GetNode<PopupMenu>("NodePopupMenu");
			_graphPopupMenu.IdPressed += OnGraphPopupMenuPressed;
			_nodePopupMenu.IdPressed += OnNodePopupMenuPressed;
			
			PopupRequest += OnShowMenu;
			NodeSelected += OnNodeSelected;
			CopyNodesRequest += OnCopyNodesRequest;
			PasteNodesRequest += OnPasteNodesRequest;
			ConnectionToEmpty += OnConnectionToEmpty;
			ConnectionRequest += OnConnectionRequest;
			DisconnectionRequest += OnDisconnectionRequest;
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
					case "StartNode":
						CreateNode<RootNode>(nodeType, data);
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
				_copyNodeData.Add(node.Serialize());
			}
		}
		
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

		private void OnNodeSelected(Node node)
		{
			// 更新检查器面板
			var btGraphNode = (BTGraphNode)node;
			EditorInterface.Singleton.InspectObject(btGraphNode.Meta, inspectorOnly:true);
		}

		private void OnConnectionRequest(StringName fromNode, long fromPort, StringName toNode, long toPort)
		{
			ConnectNode(fromNode, (int)fromPort, toNode, (int)toPort);
		}
		
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
		
		private void OnGraphPopupMenuPressed(long id)
		{
			switch (id)
			{
				case 0:
					break;
			}
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
		/// <param name="nodeType"></param>
		/// <param name="data"></param>
		/// <typeparam name="T"></typeparam>
		public T CreateNode<T>(string nodeType, Dictionary data = null) where T : BTGraphNode
		{
			if (!_nodesScenes.TryGetValue(nodeType, out var nodeScene)) return null;
			
			var newNode = nodeScene.Instantiate<T>();
			newNode.SetEditor(this);
			
			if (data == null)
			{
				newNode.Name= $"{newNode.Name}_1";
				newNode.PositionOffset = _cursorPos;
			}
			else
			{
				newNode.Deserialize(data);
			}
			
			AddChild(newNode, true);
			_nodes.Add(newNode.Name, newNode);
			
			NodeCreated?.Invoke(newNode);

			return newNode;
		}

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
			if (node.NodeType == "StartNode") return;
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
			if (node.NodeType == "StartNode") return;
			if (!_nodes.ContainsKey(node.Name)) return;
			
			_nodes.Remove(node.Name);
			DisconnectNodeConnection(node);
			NodeRemoved?.Invoke(node);
			node.QueueFree();
			RemoveChild(node);
		}

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
		/// Load deserialized <see cref="GraphData" /> from local tres file, and rebuild graph.
		/// </summary>
		/// <param name="data"></param>
		public void LoadData(BTGraphData data)
		{
			GraphData = data;

			Name = data.Filename?.Split(".")[0];

			foreach (var d in data.Nodes)
			{
				var nodeType = (string)d["NodeType"];
				switch (nodeType)
				{
					case "RootNode":
						CreateNode<RootNode>(nodeType, d);
						break;
				}
			}

			foreach (var kvp in _nodes)
			{
				kvp.Value.DeserializeDone();
			}

			foreach (var c in data.Connection)
			{
				ConnectNode((StringName)c["from_node"], (int)c["from_port"], (StringName)c["to_node"], (int)c["to_port"]);
			}
		}
		
		/// <summary>
		/// Return serialized node-graph data, include graph global param.
		/// </summary>
		/// <returns><see cref="GraphData" /></returns>
		public BTGraphData DumpsData()
		{
			Array<Dictionary> data = new ();
			foreach (var kvp in _nodes.Where(kvp => kvp.Value.IsInsideTree() && kvp.Value.Visible))
			{
				var nodeData = kvp.Value.Serialize();
				if (nodeData.Any()) data.Add(nodeData);
			}
			GraphData.Nodes = data;
			
			GraphData.Connection = GetConnectionList();
			return GraphData;
		}

		#region delegate && event
		
		public delegate void NodeCreatedEventHandler(BTGraphNode node);

		public event NodeCreatedEventHandler NodeCreated;
		
		public delegate void NodeRemovedEventHandler(BTGraphNode node);

		public event NodeRemovedEventHandler NodeRemoved;

		#endregion
	}
