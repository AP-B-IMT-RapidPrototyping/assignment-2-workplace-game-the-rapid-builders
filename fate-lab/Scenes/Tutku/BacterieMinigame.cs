using Godot;
using System;

public partial class BacterieMinigame : Node3D
{
    [Export] public PackedScene BacterieScene; // Sleep hier je bacterie_3d.tscn in!
    [Export] public int MaxBacterien = 5;
    [Export] public PapierInteractie TafelScript; // Sleep hier je Area3D (Tafel) in!

    private int _overgebleven;

    public override void _Ready()
    {
        // Maak de muis zichtbaar voor de minigame
        Input.MouseMode = Input.MouseModeEnum.Visible;
        
        _overgebleven = 0; // Reset teller
        SpawnBacterien();
    }

    private void SpawnBacterien()
    {
        if (BacterieScene == null)
        {
            GD.PrintErr("FOUT: Geen BacterieScene toegewezen in de Inspector!");
            return;
        }

        for (int i = 0; i < MaxBacterien; i++)
        {
            Bacterie instance = BacterieScene.Instantiate<Bacterie>();

            // Bepaal een willekeurige positie voor de bacteriën
            float x = (float)GD.RandRange(-3.5, 3.5);
            float y = (float)GD.RandRange(-2.0, 2.0);
            instance.Position = new Vector3(x, y, 0);

            // Verbind het signaal van de bacterie aan de teller-functie
            instance.Geklikt += OnBacterieGeklikt;

            AddChild(instance);
            _overgebleven++;
        }
        GD.Print("Minigame gestart met " + _overgebleven + " bacteriën.");
    }

    private void OnBacterieGeklikt()
    {
        _overgebleven--;
        GD.Print("Bacterie vernietigd. Nog " + _overgebleven + " over.");

        if (_overgebleven <= 0)
        {
            GD.Print("Alle bacteriën zijn weg! Minigame gewonnen.");
            StopDeGame(true);
        }
    }

// Voeg 'async' toe aan de functie
public async void StopDeGame(bool isGewonnen)
{
    if (TafelScript != null)
    {
        TafelScript.UpdateStatus(isGewonnen);
    }

    // Zoek de HUD die je net hebt toegevoegd
    var hud = GetTree().Root.FindChild("MeldingUI", true, false) as HUD;
    
    if (hud != null)
    {
        string bericht = isGewonnen ? "Test voltooid! Ga terug naar het dossier." : "Test mislukt!";
        hud.ToonMelding(bericht);
        
        // Wacht 3 seconden zodat de speler het kan lezen
        await ToSignal(GetTree().CreateTimer(3.0f), "timeout");
    }

    // Nu pas terug naar het lab
    GetTree().ChangeSceneToFile("res://Scenes/Tutku/Lab.tscn");
}
}