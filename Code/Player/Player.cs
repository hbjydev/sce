namespace SCE.Player;

[Title("SCE Player Base")]
[Group("SCE")]
public partial class Player : Component
{
	protected override void OnUpdate()
	{
		OnMovementUpdate();
	}

	protected override void OnFixedUpdate()
	{
		base.OnFixedUpdate();

		OnMovementFixedUpdate();
	}

	protected override void OnStart()
	{
		OnMovementStart();
	}

	protected override void OnEnabled()
	{

	}

	protected override void OnDisabled()
	{

	}

	protected override void OnDestroy()
	{

	}

	protected override void DrawGizmos()
	{
		DrawMovementGizmos();
	}
}
