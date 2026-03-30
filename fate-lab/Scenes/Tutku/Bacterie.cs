using Godot;
using System;

public partial class Bacterie : StaticBody3D
{
    [Signal]
    public delegate void GekliktEventHandler();

    private Camera3D _camera;

    public override void _Ready()
    {
        // Vind de actieve camera om de muisklik naar de 3D wereld te vertalen
        _camera = GetViewport().GetCamera3D();
    }

    public override void _Input(InputEvent @event)
    {
        // Controleer op linkermuisklik
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
        {
            if (IsClickedOn(mouseEvent.Position))
            {
                GD.Print("Bacterie geraakt!");
                // Geef een seintje aan de Minigame Manager
                EmitSignal(SignalName.Geklikt); 
                
                // Verwijder de bacterie uit de scene
                QueueFree(); 
            }
        }
    }

    private bool IsClickedOn(Vector2 mousePos)
    {
        if (_camera == null) return false;

        var spaceState = GetWorld3D().DirectSpaceState;
        var query = PhysicsRayQueryParameters3D.Create(
            _camera.ProjectRayOrigin(mousePos),
            _camera.ProjectRayOrigin(mousePos) + _camera.ProjectRayNormal(mousePos) * 1000
        );
        
        var result = spaceState.IntersectRay(query);

        // Als we iets raken, controleer of dit object het is
        if (result.Count > 0)
        {
            Node3D collidedNode = (Node3D)result["collider"];
            return collidedNode == this;
        }
        return false;
    }
}