using Godot;
using System;

public partial class Knop3d : Area3D
{
    [Export] public main_orgaanspawner Spawner;
    [Export] private AnimationPlayer AnimPlayer;
    [Export] public RayCast3D PlayerRayCast;

    public override void _Ready()
    {
        // Extra veiligheid als je hem niet in de inspector zet
        if (AnimPlayer == null)
            AnimPlayer = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");
    }

    public override void _Process(double delta)
    {
        if (PlayerRayCast == null)
            return;

        // Check of raycast iets raakt
        if (PlayerRayCast.IsColliding())
        {
            var collider = PlayerRayCast.GetCollider();

            // DEBUG (optioneel)
            GD.Print("Raycast hit: ", collider);

            // BELANGRIJK: check of we deze knop raken
            if (IsSameObject(collider))
            {
                if (Input.IsKeyPressed(Key.G))
                {
                    Indrukken();
                }
            }
        }
    }

    private bool IsSameObject(object collider)
    {
        // Directe hit
        if (collider == this)
            return true;

        // Vaak hit je de CollisionShape3D → check parent
        if (collider is Node node)
        {
            if (node.GetParent() == this)
                return true;
        }

        return false;
    }

    public void Indrukken()
    {
        GD.Print("KNOP: Geactiveerd met de G-toets!");

        if (AnimPlayer != null && !AnimPlayer.IsPlaying())
        {
            AnimPlayer.Play("Indrukken");
        }

        if (Spawner != null)
        {
            Spawner.SpawnRij();
        }
    }
}