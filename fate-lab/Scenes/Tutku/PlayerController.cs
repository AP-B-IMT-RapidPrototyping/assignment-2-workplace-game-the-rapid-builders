using Godot;
using System;
public partial class PlayerController : CharacterBody3D
{
    [Export] public float Speed = 5.0f;
    [Export] public float JumpVelocity = 4.5f;
    [Export] public float MouseSensitivity = 0.002f;
    private Camera3D _camera;
    // Haal de zwaartekracht op uit de projectinstellingen
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
    public override void _Ready()
    {
        // Zoek de camera op
        _camera = GetNode<Camera3D>("Camera3D");
        
        // Vang de muis zodat je rond kunt kijken zonder dat de cursor over het scherm vliegt
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }
    public override void _Input(InputEvent @event)
    {
        // Muisbeweging om rond te kijken
        if (@event is InputEventMouseMotion mouseMotion)
        {
            // 1. Draai de hele speler links/rechts (Y-as)
            RotateY(-mouseMotion.Relative.X * MouseSensitivity);
            // 2. Bereken de nieuwe verticale rotatie voor de camera (omhoog/omlaag)
            Vector3 camRotation = _camera.Rotation;
            camRotation.X -= mouseMotion.Relative.Y * MouseSensitivity;
            // 3. STABILITEIT: Zorg dat je niet verder dan 85 graden omhoog of omlaag kijkt
            // Dit voorkomt dat je beeld gaat tollen of ondersteboven flitst.
            camRotation.X = Mathf.Clamp(camRotation.X, Mathf.DegToRad(-85), Mathf.DegToRad(85));
            _camera.Rotation = camRotation;
        }
        // Druk op ESC om je muis weer te bevrijden (handig voor testen)
        if (Input.IsActionJustPressed("ui_cancel"))
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }
    }
    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;
        // Voeg zwaartekracht toe
        if (!IsOnFloor())
            velocity.Y -= gravity * (float)delta;
        // Springen
        if (Input.IsActionJustPressed("jump") && IsOnFloor())
            velocity.Y = JumpVelocity;
        // Krijg de richting op basis van de Input Map (Zorg dat deze namen in je Project Settings staan!)
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
    }
}
/*using Godot;
using System;
public partial class PlayerController : CharacterBody3D
{
    [Export] public float Speed = 5.0f;
    [Export] public float JumpVelocity = 4.5f;
    [Export] public float MouseSensitivity = 0.002f;
    private Camera3D _camera;
    // Haal de zwaartekracht op uit de projectinstellingen
    public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
    public override void _Ready()
    {
        _camera = GetNode<Camera3D>("Camera3D");
        // Vang de muis zodat je rond kunt kijken zonder dat de cursor over het scherm vliegt
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }
    public override void _Input(InputEvent @event)
    {// Muisbeweging om rond te kijken
        if (@event is InputEventMouseMotion mouseMotion)
        {
            // 1. Draai de hele speler links/rechts (Y-as)
            RotateY(-mouseMotion.Relative.X * MouseSensitivity);
            // 2. Bereken de nieuwe verticale rotatie voor de camera (omhoog/omlaag)
            Vector3 camRotation = _camera.Rotation;
            camRotation.X -= mouseMotion.Relative.Y * MouseSensitivity;
            // 3. STABILITEIT: Zorg dat je niet verder dan 85 graden omhoog of omlaag kijkt
            // Dit voorkomt dat je beeld gaat tollen of ondersteboven flitst.
            camRotation.X = Mathf.Clamp(camRotation.X, Mathf.DegToRad(-85), Mathf.DegToRad(85));
            _camera.Rotation = camRotation;
        }
        // Druk op ESC om je muis weer te bevrijden (handig voor testen)
        if (Input.IsActionJustPressed("ui_cancel"))
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
        }
    }
    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;
        // Voeg zwaartekracht toe
        if (!IsOnFloor())
            velocity.Y -= gravity * (float)delta;
        // Springen
        if (Input.IsActionJustPressed("jump") && IsOnFloor())
            velocity.Y = JumpVelocity;
        // Krijg de richting op basis van de Input Map (Zorg dat deze namen in je Project Settings staan!)
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
    }
}*/