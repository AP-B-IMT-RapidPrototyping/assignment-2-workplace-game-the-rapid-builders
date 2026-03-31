using Godot;
using System;

public partial class BacterieMinigame : Node3D
{
    [Export] public PackedScene BacterieScene; // Sleep hier je bacterie_3d.tscn in!
    [Export] public int MaxBacterien = 5;
    [Export] public PapierInteractie TafelScript; // Sleep hier je Area3D (Tafel) in!
// Verander dit als het nog op 'ProgressBarAftikken' stond
    [Export] public ProgressBarUpdate HartslagMeter;
    
    // Voeg dit toe voor een tijdslimiet:
    [Export] public float Tijdslimiet = 10.0f; // 10 seconden om te winnen

    private int _overgebleven;
    private SceneTreeTimer _minigameTimer;

    public override void _Ready()
    {
        // Maak de muis zichtbaar voor de minigame
        Input.MouseMode = Input.MouseModeEnum.Visible;
        
        _overgebleven = 0; // Reset teller
        SpawnBacterien();

        // Start de timer voor de minigame
        _minigameTimer = GetTree().CreateTimer(Tijdslimiet);
        _minigameTimer.Timeout += OnMinigameTimeout;
    }

    private void OnMinigameTimeout()
    {
        if (_overgebleven > 0)
        {
            GD.Print("Tijd is om! Minigame mislukt.");
            StopDeGame(false); // Je hebt niet gewonnen
        }
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
        if (_minigameTimer.TimeLeft <= 0) return; // Geen actie na timeout

        _overgebleven--;
        GD.Print("Bacterie vernietigd. Nog " + _overgebleven + " over.");

        if (_overgebleven <= 0)
        {
            GD.Print("Alle bacteriën zijn weg! Minigame gewonnen.");
            _minigameTimer.Timeout -= OnMinigameTimeout; // Ontkoppel de timeout event
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

        // --- DIT IS DE NIEUWE CODE VOOR DE HARTSLAG ---
        // Als de speler niet gewonnen heeft, is het GAME OVER voor de hartslagmeter
        if (!isGewonnen && HartslagMeter != null)
        {
            // Je hebt de meter al in de Inspector, nu zeggen we: Ga Dood!
            // Of gebruik HartslagMeter.DoeSchade(); als er alleen leven af moet
            HartslagMeter.GaDood(); 
        }
        // ----------------------------------------------

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

/*using Godot;
using System;

public partial class BacterieMinigame : Node3D
{
    [Export] public PackedScene BacterieScene; // Sleep hier je bacterie_3d.tscn in!
    [Export] public int MaxBacterien = 5;
    [Export] public PapierInteractie TafelScript; // Sleep hier je Area3D (Tafel) in!
    [Export] public ProgressBarAftikken HartslagMeter;
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
}*/