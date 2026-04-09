using Godot;
using System;

public partial class main_DoosSlecht : Area3D
{
	[Export] public AudioStreamPlayer3D GeluidTrue;  // Het geluid voor de juiste bak
    [Export] public AudioStreamPlayer3D GeluidFalse; // Het geluid voor de verkeerde bak
    [Export] public ScoreManager ScoreLabel;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node3D body)
    {
        // We checken of het object de Metadata 'IsGoed' heeft
        if (body.HasMeta("IsGoed"))
        {
            string waarde = body.GetMeta("IsGoed").AsString();

            // In DEZE bak zoeken we specifiek naar de waarde "Slecht"
            if (waarde == "Slecht")
            {
                GD.Print("DOOS SLECHT: Match! Dit orgaan is inderdaad slecht.");
                GD.Print("DOOS SLECHT: Juiste match! +10");
                if (ScoreLabel != null) ScoreLabel.VoegPuntenToe(10);
                if (GeluidTrue != null) GeluidTrue.Play();
            }
            else
            {
                GD.Print($"DOOS SLECHT: Geen match. Orgaan was '{waarde}', maar we zochten 'Slecht'.");
                GD.Print($"DOOS SLECHT: Foutief orgaan ({waarde})! -5");
                if (ScoreLabel != null) ScoreLabel.VoegPuntenToe(-5);
                if (GeluidFalse != null) GeluidFalse.Play();
            }

            // Verwijder het orgaan uit de game
            body.QueueFree();
        }
        else
        {
            GD.Print("DOOS SLECHT: Dit object hoort hier sowieso niet: " + body.Name);
        }
    }
}
