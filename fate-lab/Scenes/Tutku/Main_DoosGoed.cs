using Godot;
using System;

public partial class Main_DoosGoed : Area3D
{
	[Export] public AudioStreamPlayer3D GeluidTrue;  // Het 'Goed gedaan' geluid
    [Export] public AudioStreamPlayer3D GeluidFalse; // Het 'Foute bak' geluid

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node3D body)
    {
        // We checken of het object de Metadata 'IsGoed' heeft (de sticker van de spawner)
        if (body.HasMeta("IsGoed"))
        {
            string waarde = body.GetMeta("IsGoed").AsString();

            // Check of het in deze bak hoort (DoosGoed zoekt naar "Goed")
            if (waarde == "Goed")
            {
                GD.Print("DOOS GOED: Match! Punten erbij.");
                if (GeluidTrue != null) GeluidTrue.Play();
            }
            else
            {
                GD.Print($"DOOS GOED: Fout! Dit orgaan was '{waarde}' en hoort hier niet.");
                if (GeluidFalse != null) GeluidFalse.Play();
            }

            // Verwijder het orgaan uit de game (het zit nu in de doos)
            body.QueueFree();
        }
        else
        {
            // Dit is voor als je er per ongeluk een pen of de speler zelf in gooit
            GD.Print("DOOS GOED: Dit object heeft geen statuslabel: " + body.Name);
        }
    }
}
