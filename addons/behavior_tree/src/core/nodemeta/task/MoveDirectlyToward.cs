using Godot;
using Godot.Collections;

/// <summary>
/// 直接移动至（Move Directly Toward） 任务节点会将AI Pawn沿直线移向指定的Actor或位置（矢量）黑板条目，而不参考任何导航系统。如果需要AI按导航移动，请改用 移动至（Move To） 任务节点。
/// </summary>
[Tool]
public partial class MoveDirectlyToward : Task
{
	[NodeMeta] public override string NodeType { get; set; } = "MoveDirectlyToward";
	[Export, NodeMeta] public double Speed { get; set; }
	[Export, NodeMeta] public Vector2 TargetPosition { get; set; }
    
	public MoveDirectlyToward()
	{
        
	}
    
	public MoveDirectlyToward(BehaviorTree behaviorTree, Dictionary data) : base(behaviorTree, data)
	{

	}
    
	public MoveDirectlyToward(BehaviorTree behaviorTree, BTGraphNode mGraphNode, Dictionary data) : base(behaviorTree, mGraphNode, data)
	{

	}

	public override void PhysicsProcess(double delta)
	{
		base.PhysicsProcess(delta);

		MoveTo(delta);
	}

	private void MoveTo(double delta)
	{
		var position = MBehaviorTreePlayer.Character.Position;
		if (position != TargetPosition)
		{
			MBehaviorTreePlayer.Character.Position = position.Lerp(TargetPosition, (float)(Speed * delta));
		}
	}
}
