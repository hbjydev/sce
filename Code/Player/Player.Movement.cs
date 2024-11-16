using Sandbox.Citizen;

namespace SCE.Player;

public partial class Player
{
    [Property]
    [Category("Components")]
    public GameObject Camera { get; set; }

    [Property]
    [Category("Components")]
    [Title("Character Controller")]
    public CharacterController Controller { get; set; }

    [Property]
    [Category("Components")]
    public CitizenAnimationHelper AnimationHelper { get; set; }

    /// <summary>
    /// How fast the player can walk (units/sec)
    /// </summary>
    [Property]
    [Range(0f, 400f, 1f)]
    [Category("Movement")]
    public float WalkSpeed { get; set; } = 70f;

    /// <summary>
    /// How fast the player can run (units/sec)
    /// </summary>
    [Property]
    [Range(0f, 400f, 1f)]
    [Category("Movement")]
    public float RunSpeed { get; set; } = 140f;

    /// <summary>
    /// How fast the player can sprint (units/sec)
    /// </summary>
    [Property]
    [Range(0f, 400f, 1f)]
    [Category("Movement")]
    public float SprintSpeed { get; set; } = 250f;

    /// <summary>
    /// How powerfully the player can jump (units/sec)
    /// </summary>
    [Property]
    [Range(0f, 1000f, 10f)]
    [Category("Movement")]
    public float JumpStrength { get; set; } = 250f;

    /// <summary>
    /// The key used to toggle between walking and running
    /// </summary>
    [Property]
    [Category("Keybinds")]
    [InputAction]
    public string ToggleWalkAction { get; set; } = "walk";
    bool isWalking = false;

    /// <summary>
    /// The key used to trigger sprinting
    /// </summary>
    [Property]
    [Category("Keybinds")]
    [InputAction]
    public string SprintAction { get; set; } = "run";

    /// <summary>
    /// The key used to trigger a Jump
    /// </summary>
    [Property]
    [Category("Keybinds")]
    [InputAction]
    public string JumpAction { get; set; } = "jump";

    /// <summary>
    /// The key used to trigger a crouch
    /// </summary>
    [Property]
    [Category("Keybinds")]
    [InputAction]
    public string CrouchAction { get; set; } = "duck";
    bool isCrouched = false;

    /// <summary>
    /// Where the camera rotates around and the aim originates from
    /// </summary>
    [Property]
    [Category("Movement")]
    public Vector3 EyePosition { get; set; }

    public Angles EyeAngles { get; set; }
    Transform _initialCameraTransform;

    private void OnMovementStart()
    {
        if (Camera != null)
            _initialCameraTransform = Camera.LocalTransform;
    }

    private void OnMovementUpdate()
    {
        EyeAngles += Input.AnalogLook;
        EyeAngles = EyeAngles.WithPitch(MathX.Clamp(EyeAngles.pitch, -40f, 80f));
        WorldRotation = Rotation.FromYaw(EyeAngles.yaw);

        if (Camera != null)
            Camera.LocalTransform = _initialCameraTransform.RotateAround(EyePosition, EyeAngles.WithYaw(0f));
    }

	private void OnMovementFixedUpdate()
	{
        if (Controller == null) return;

        if (Input.Pressed(ToggleWalkAction)) isWalking = !isWalking;
        if (Input.Pressed(CrouchAction)) isCrouched = !isCrouched;

        var wishSpeed = isCrouched ? WalkSpeed : Input.Down(SprintAction) ? SprintSpeed : (isWalking ? WalkSpeed : RunSpeed);
        var wishVelocity = Input.AnalogMove.Normal * wishSpeed * WorldRotation;
        Controller.Accelerate(wishVelocity);

        if (Controller.IsOnGround) {
            Controller.Acceleration = 10f;
            Controller.ApplyFriction(5f, 10f);

            if (Input.Pressed(JumpAction)) {
                Controller.Punch(Vector3.Up * JumpStrength);
                AnimationHelper?.TriggerJump();
            }

            AnimationHelper.DuckLevel = isCrouched ? 1 : 0;
        } else {
            Controller.Acceleration = 5f;
            Controller.Velocity += Scene.PhysicsWorld.Gravity * Time.Delta;
        }

        Controller.Move();

        if (AnimationHelper != null) {
            AnimationHelper.IsGrounded = Controller.IsOnGround;
            AnimationHelper.WithVelocity(Controller.Velocity);
        }
	}

    private void DrawMovementGizmos()
    {
        Gizmo.Draw.LineSphere(EyePosition, 10f);
    }
}