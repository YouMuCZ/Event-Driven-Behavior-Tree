using Godot;
using Godot.Collections;

/// <summary>
/// 直接移动至（Move Directly Toward） 任务节点会将AI Pawn沿直线移向指定的Actor或位置（矢量）黑板条目，而不参考任何导航系统。如果需要AI按导航移动，请改用 移动至（Move To） 任务节点。
/// </summary>
[Tool]
public partial class MoveToward2D : Task
{
	[NodeMeta] public override string NodeType { get; set; } = "MoveToward2D";
	[Export, NodeMeta] public float Speed { get; set; } = 100;
	[Export, NodeMeta] public Vector2 TargetPosition { get; set; } = Vector2.Zero;

	private Vector2 _direction;
	private CharacterBody2D _character;
    
	public MoveToward2D()
	{
        
	}
    
	public MoveToward2D(BehaviorTree behaviorTree, Dictionary data) : base(behaviorTree, data)
	{

	}
    
	public MoveToward2D(BehaviorTree behaviorTree, BTGraphNode mGraphNode, Dictionary data) : base(behaviorTree, mGraphNode, data)
	{

	}

	protected override void OnStart()
	{
		base.OnStart();
		_character = MBehaviorTreePlayer.Character;
		_direction = (TargetPosition - _character.Position).Normalized(); // 计算方向
	}

	public override void PhysicsProcess(double delta)
	{
		base.PhysicsProcess(delta);
		
		MoveToward(delta);
	}

	private void MoveToward(double delta)
	{
		if (IsReached())
		{
			Finish(true);
		}
		else
		{
			var velocity  = _direction * Speed; // 根据速度和方向更新位置
			_character.Velocity = velocity;
			_character.MoveAndSlide();
		}
	}
	
	/// <summary>
	/// check if character has reached target position.
	/// </summary>
	/// <returns></returns>
	private bool IsReached()
	{
		if (_character.Position == TargetPosition) return true;

		if (_character.Position.DistanceTo(TargetPosition) < 1) return true;

		return false;
	}
}
