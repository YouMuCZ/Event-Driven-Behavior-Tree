using Godot;
using System;

[Tool, GlobalClass]
public partial class BehaviorTreePlayer : Node
{
	/// <summary> 当前正在运行的行为树 </summary>
	private BehaviorTree _behaviorTree;
	[Export] public BehaviorTree BehaviorTree
	{
		get => _behaviorTree;
		set
		{
			EmitSignal(SignalName.BehaviorTreeChanged, value);
			_behaviorTree = value;
		}
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_behaviorTree?.Initialize(this);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void Start()
	{
		_behaviorTree.Start();
		EmitSignal(SignalName.BehaviorTreeStart, _behaviorTree);
	}

	public void Stop()
	{
		_behaviorTree.Stop();
		EmitSignal(SignalName.BehaviorTreeStop, _behaviorTree);
	}

	#region Signal Event
	[Signal] public delegate void BehaviorTreeStartEventHandler(BehaviorTree behaviorTree);
	[Signal] public delegate void BehaviorTreeStopEventHandler(BehaviorTree behaviorTree);
	[Signal] public delegate void BehaviorTreeChangedEventHandler(BehaviorTree behaviorTree);

	#endregion


}
