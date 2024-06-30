using Godot;
using System;

[GlobalClass]
public partial class BehaviorTreePlayer : Node
{
	private bool _debug;
	private Blackboard _blackboard;
	private BehaviorTree _behaviorTree;
	
	[Export] public CharacterBody2D Character;
	/// <summary> The current entity is executing a behavior tree, and each entity has exactly one <see cref="MBehaviorTree"/> running at any given time. </summary>
	[Export] public BehaviorTree MBehaviorTree
	{
		get => _behaviorTree;
		set
		{
			EmitSignal(SignalName.BehaviorTreeChanged, value);
			_behaviorTree = value;
		}
	}

	[Export] public Blackboard MBlackboard
	{
		get => _blackboard;
		set
		{
			EmitSignal(SignalName.BehaviorTreeBlackboardChanged, value);
			_blackboard = value;
		}
	}
	
	[Export] public bool DebugMode
	{
		set => _debug = value;
		get => _debug;
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Character ??= GetParent<CharacterBody2D>();
		
		_blackboard ??= new Blackboard(this);

		if (_behaviorTree != null)
		{
			_behaviorTree.MBehaviorTreePlayer = this;
			_behaviorTree.Initialize();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_behaviorTree.Process(delta);
	}

	public override void _PhysicsProcess(double delta)
	{
		_behaviorTree.PhysicsProcess(delta);
	}

	public void Start()
	{
		_behaviorTree.Start();
		EmitSignal(SignalName.BehaviorTreeStart, _behaviorTree);
	}

	public void Interrupt()
	{
		EmitSignal(SignalName.BehaviorTreeInterrupt, _behaviorTree);
	}

	public void Stop()
	{
		_behaviorTree.Stop();
		EmitSignal(SignalName.BehaviorTreeStop, _behaviorTree);
	}

	#region Signal Event
	
	[Signal] public delegate void BehaviorTreeStartEventHandler(BehaviorTree behaviorTree);
	[Signal] public delegate void BehaviorTreeInterruptEventHandler(BehaviorTree behaviorTree);
	[Signal] public delegate void BehaviorTreeStopEventHandler(BehaviorTree behaviorTree);
	[Signal] public delegate void BehaviorTreeChangedEventHandler(BehaviorTree behaviorTree);
	[Signal] public delegate void BehaviorTreeBlackboardChangedEventHandler(Blackboard blackboard);

	#endregion
	
}
