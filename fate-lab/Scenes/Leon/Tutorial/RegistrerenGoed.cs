using Godot;
using System;

public partial class RegistrerenGoed : Area3D
{
	[Export] public AudioStreamPlayer3D GeluidTrue;  // Sleep de 'True' node hierheen
    [Export] public AudioStreamPlayer3D GeluidFalse; // Sleep de 'False' node hierheen
    

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node3D body)
    {

        if (body is TutLungs Orgaan)
        {
            // Trim verwijdert eventuele spaties, ToLower maakt het ongevoelig voor hoofdletters
            string waarde = Orgaan.IsGoed.Trim();

            if (waarde == "Goed")
            {
                GD.Print("MATCH: Het is slecht! Speel True af.");
                GeluidTrue.Play();
            }
            else
            {
                GD.Print($"GEEN MATCH: Waarde was '{waarde}', maar we zochten 'Slecht'.");
                GeluidFalse.Play();
            }

            Orgaan.QueueFree();
        }

        if (body is TutBloodSample Bloed)
        {
            // Trim verwijdert eventuele spaties, ToLower maakt het ongevoelig voor hoofdletters
            string waarde = Bloed.IsGoed.Trim();

            if (waarde == "Goed")
            {
                GD.Print("MATCH: Het is slecht! Speel True af.");
                GeluidTrue.Play();
            }
            else
            {
                GD.Print($"GEEN MATCH: Waarde was '{waarde}', maar we zochten 'Slecht'.");
                GeluidFalse.Play();
            }

            Bloed.QueueFree();
        }

        if (body is TutDrugSample Drugs)
        {
            // Trim verwijdert eventuele spaties, ToLower maakt het ongevoelig voor hoofdletters
            string waarde = Drugs.IsGoed.Trim();

            if (waarde == "Goed")
            {
                GD.Print("MATCH: Het is slecht! Speel True af.");
                GeluidTrue.Play();
            }
            else
            {
                GD.Print($"GEEN MATCH: Waarde was '{waarde}', maar we zochten 'Slecht'.");
                GeluidFalse.Play();
            }

            Drugs.QueueFree();
        }

    }
}
