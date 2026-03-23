using Godot;
using System;
public partial class Bacterie : StaticBody3D
{
    [Signal]
    public delegate void GekliktEventHandler();
    private Camera3D _camera;
    public override void _Ready()
    {
        // Vind de camera in de scene
        _camera = GetViewport().GetCamera3D();
    }
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        
        // Verwerk muisklik
        if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
            {
                // Controleer of we op deze bacterie klikken met raycast
                if (IsClickedOn(mouseEvent.Position))
                {
                    GD.Print("Bacterie geraakt!");
                    EmitSignal(SignalName.Geklikt);
                    
                    // Verwijder de bacterie
                    QueueFree();
                }
            }
        }
    }
    private bool IsClickedOn(Vector2 mousePos)
    {
        if (_camera == null) return false;
        // Maak een raycast naar de 3D wereld
        var spaceState = GetWorld3D().DirectSpaceState;
        var query = PhysicsRayQueryParameters3D.Create(
            _camera.ProjectRayOrigin(mousePos),
            _camera.ProjectRayOrigin(mousePos) + _camera.ProjectRayNormal(mousePos) * 1000
        );
        var result = spaceState.IntersectRay(query);
        // Check of we deze bacterie hebben geraakt
        if (result.Count > 0)
        {
            Node3D collidedNode = (Node3D)result["collider"];
            return collidedNode == this;
        }
        return false;
    }
}
/*using Godot;
using System;
public partial class Bacterie : StaticBody3D
{
    [Signal] public delegate void GekliktEventHandler();
    public void OnInputEvent(Node camera, InputEvent @event, Vector3 pos, Vector3 norm, long shapeIdx)
    {
        if (@event is InputEventMouseButton m && m.Pressed && m.ButtonIndex == MouseButton.Left)
        {
            EmitSignal(SignalName.Geklikt);
            QueueFree();
        }
    }
}*/