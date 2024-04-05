using Godot;
using System;

public partial class Character : CharacterBody2D
{
	private BehaviorTreePlayer _behaviorTree;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_behaviorTree = GetNode<BehaviorTreePlayer>("BehaviorTreePlayer");
		
		_behaviorTree.Start();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
