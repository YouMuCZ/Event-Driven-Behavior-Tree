using Godot;
using System;
using Cinematics.Dialogue;
using Cinematics.Dialogue.Nodes;

[Tool]
public partial class SelectNodeItem : HBoxContainer
{
	public int Slot;
	public string OptionText;
	
	private LineEdit _option;
	private Button _deleteButton;
	private SelectNode _selectNode;
	private SelectChildNode _selectChildNode;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_option = GetNode<LineEdit>("option");
		_deleteButton = GetNode<Button>("delete");

		_deleteButton.Pressed += OnDeletePressed;
	}

	private void OnDeletePressed()
	{
		ItemDeleted?.Invoke(this);
	}

	public void OnTextChanged(string newText)
	{
		_option ??= GetNode<LineEdit>("option");
		_option.Text = newText;
	}
	
	public delegate void ItemRemovedEventHandler(SelectNodeItem item);

	public event ItemRemovedEventHandler ItemDeleted;
}
