using Godot;
using System;

public partial class Movement : CharacterBody3D
{
	[ExportGroup("Movement Settings")]
	[Export] public float Speed = 5.0f;
	[Export] public float MouseSensitivity = 0.002f;

	[ExportGroup("Interaction Settings")]
	[Export] public float InteractionDistance = 3.0f;
	[Export] public float PullPower = 20.0f;
	[Export] public float RotationSpeed = 3.0f; // Hoe snel het object draait met de pijltjes
	[Export] public RayCast3D InteractionRay;
	[Export] public Label InteractionLabel;
	[Export] public Marker3D HoldPosition;

	private Camera3D _camera;
	private RigidBody3D _carriedObject = null;
	private float _rotationX = 0f;

	public override void _Ready()
	{
		_camera = GetNode<Camera3D>("Camera3D");
		Input.MouseMode = Input.MouseModeEnum.Captured;

		if (InteractionRay != null)
		{
			InteractionRay.TargetPosition = new Vector3(0, 0, -InteractionDistance);
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion)
		{
			RotateY(-mouseMotion.Relative.X * MouseSensitivity);
			_rotationX -= mouseMotion.Relative.Y * MouseSensitivity;
			_rotationX = Mathf.Clamp(_rotationX, Mathf.DegToRad(-89f), Mathf.DegToRad(89f));
			_camera.Rotation = new Vector3(_rotationX, _camera.Rotation.Y, _camera.Rotation.Z);
		}

		if (@event.IsActionPressed("interact"))
		{
			if (_carriedObject == null) TryPickUp();
			else DropObject();
		}

		if (@event.IsActionPressed("ui_cancel"))
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}
	}

	private void TryPickUp()
	{
		if (InteractionRay == null || !InteractionRay.IsColliding()) return;

		var collider = InteractionRay.GetCollider();
		if (collider is RigidBody3D rb && rb.IsInGroup("pickable"))
		{
			_carriedObject = rb;
			_carriedObject.GravityScale = 0;
			_carriedObject.LinearDamp = 10;
			_carriedObject.AngularDamp = 10;
		}
	}

	private void DropObject()
	{
		if (_carriedObject == null) return;
		_carriedObject.GravityScale = 1;
		_carriedObject.LinearDamp = 0;
		_carriedObject.AngularDamp = 0;
		_carriedObject = null;
	}

	public override void _Process(double delta)
	{
		UpdateUI();
	}

	private void UpdateUI()
	{
		if (InteractionLabel == null || InteractionRay == null) return;

		if (_carriedObject == null && InteractionRay.IsColliding())
		{
			var collider = InteractionRay.GetCollider() as Node;
			if (collider != null && collider.IsInGroup("pickable"))
			{
				InteractionLabel.Text = "Press G to pick up";
				InteractionLabel.Visible = true;
			}
			else InteractionLabel.Visible = false;
		}
		else InteractionLabel.Visible = false;
	}

	public override void _PhysicsProcess(double delta)
	{
		// Speler beweging (ZQSD)
		Vector3 velocity = Velocity;
		if (!IsOnFloor()) velocity += GetGravity() * (float)delta;

		Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();

		// Object logica
		if (_carriedObject != null)
		{
			// 1. Positie: Naar hand trekken
			Vector3 targetPos = HoldPosition.GlobalPosition;
			Vector3 currentPos = _carriedObject.GlobalPosition;
			Vector3 directionToHand = targetPos - currentPos;
			_carriedObject.LinearVelocity = directionToHand * PullPower;

			// 2. Rotatie: Luisteren naar pijltjestoetsen
			Vector3 rotationInput = Vector3.Zero;

			// Pijl omhoog/omlaag (om de lokale X-as van de camera)
			rotationInput.X = Input.GetActionStrength("rotate_down") - Input.GetActionStrength("rotate_up");
			// Pijl links/rechts (om de wereld Y-as of lokale Y-as)
			rotationInput.Y = Input.GetActionStrength("rotate_right") - Input.GetActionStrength("rotate_left");

			if (rotationInput != Vector3.Zero)
			{
				// We berekenen de rotatie-as relatief aan de camera zodat 'omhoog' altijd 'omhoog' is op je scherm
				Vector3 rotDrive = (_camera.GlobalTransform.Basis.X * rotationInput.X) + (Vector3.Up * rotationInput.Y);
				_carriedObject.AngularVelocity = rotDrive * RotationSpeed;
			}
			else
			{
				// Als je geen pijltjes indrukt, stopt het object met draaien
				_carriedObject.AngularVelocity = Vector3.Zero;
			}

			// Veiligheid loslaten
			if (directionToHand.Length() > InteractionDistance + 1.5f) DropObject();
		}
	}
}
