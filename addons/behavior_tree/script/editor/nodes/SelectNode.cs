using System.Linq;
using Godot;
using Godot.Collections;

namespace Cinematics.Dialogue.Nodes
{
	[Tool]
	public partial class SelectNode : DGraphNode
	{
		[NodeMeta] public Dictionary<int, string> Item2Node { get; set; } = new();
		
		private Button _addButton;
		private PackedScene _selectItemPackedScene = ResourceLoader.Load<PackedScene>("res://addons/Dialogue/edit/nodes/select_node_item.tscn");
		
		public override void _Ready()
		{
			base._Ready();
			_addButton = GetNode<Button>("Toolbar/AddButton");

			_addButton.Pressed += OnAddButtonPressed;
		}

		private void OnAddButtonPressed()
		{
			var index = GetChildCount();
			SetSlot(index, false, 0, Colors.White, true, 0, Colors.White);
			
			var item = _selectItemPackedScene.Instantiate<SelectNodeItem>();
			item.Slot = index;
			item.ItemDeleted += OnItemDeleted;
			AddChild(item);
			
			var childNode = NEditor.CreateNode<SelectChildNode>("SelectChildNode");
			childNode.Option.TextChanged += item.OnTextChanged;
			childNode.PositionOffset = new Vector2(PositionOffset.X + Size.X + 10, PositionOffset.Y + (index-1) * childNode.Size.Y);
			NEditor.ConnectNode(Name, index - 1, childNode.Name, 0);
			
			if (Item2Node.TryGetValue(item.GetIndex(), out var value))
			{
				Item2Node[item.GetIndex()] = childNode.Name;
			}
			else
			{
				Item2Node.Add(item.GetIndex(), childNode.Name);
			}
		}
		
		/// <summary>
		/// 节点面板删除子节点
		/// </summary>
		/// <param name="item"></param>
		private void OnItemDeleted(SelectNodeItem item)
		{
			var itemIndex = item.GetIndex();
			
			if (Item2Node.TryGetValue(itemIndex, out var value))
			{
				NEditor.RemoveNode(value);
			}

			var index = itemIndex + 1;
			for (; index < GetChildCount(); index++)
			{
				if (Item2Node.TryGetValue(index, out var name))
				{
					Item2Node[index - 1] = name;
					NEditor.DisconnectNode(Name, index - 1, name, 0);
					NEditor.ConnectNode(Name, index - 2, name, 0);
				}
			}

			if (itemIndex == GetChildCount())
			{
				NEditor.DisconnectNode(Name, itemIndex - 1, Item2Node[itemIndex], 0);
			}
			
			Item2Node.Remove(GetChildCount() - 1);
			RemoveChild(GetChildOrNull<SelectNodeItem>(itemIndex));
		}

		public override void DeserializeDone()
		{
			base.DeserializeDone();

			foreach (var kvp in Item2Node)
			{
				var item = _selectItemPackedScene.Instantiate<SelectNodeItem>();
				item.Slot = kvp.Key;
				item.ItemDeleted += OnItemDeleted;
				AddChild(item);
				SetSlot(kvp.Key, false, 0, Colors.White, true, 0, Colors.White);
				
				var node = (SelectChildNode)NEditor.GetNodeByName(kvp.Value);
				node.Option.TextChanged += item.OnTextChanged;
				item.OnTextChanged(node.Option.Text);
			}
		}
	}
}
