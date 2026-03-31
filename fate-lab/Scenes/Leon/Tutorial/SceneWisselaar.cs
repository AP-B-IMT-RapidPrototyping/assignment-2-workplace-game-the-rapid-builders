using Godot;
using System;

public partial class SceneWisselaar : Area3D
{
    // Dit maakt een vakje in de Inspector waar je een .tscn bestand in kunt slepen
    [Export] public PackedScene NieuweScene;

    public override void _Ready()
    {
        // Verbind het signaal
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node3D body)
    {
        // We kijken of de node 'player' heet OF in de groep 'Player' zit (hoofdlettergevoelig!)
        if (body.Name == "player" || body.IsInGroup("Player"))
        {
            if (NieuweScene != null)
            {
                GD.Print("Speler gedetecteerd! Wisselen naar: " + NieuweScene.ResourcePath);
                
                // Wissel naar de gekoppelde scene
                GetTree().ChangeSceneToPacked(NieuweScene);
            }
            else
            {
                GD.PrintErr("FOUT: Je bent vergeten een scene in de Inspector te slepen!");
            }
        }
    }
}