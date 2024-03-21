using Godot;
using System;

namespace Cinematics.Dialogue.Nodes
{
	[Tool]
	public partial class SelectChildNode : DGraphNode
	{
		[NodeMeta] public string Parent;
		[NodeMeta] public int ParentSlot;
		[NodeMeta] public string OptionText { get => Option.Text; set => Option.Text = value; }

		public LineEdit Option => GetNode<LineEdit>("option");
	}
}
