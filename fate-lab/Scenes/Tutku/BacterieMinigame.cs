using Godot;
using System;

public partial class BacterieMinigame : Node3D
{
    [Export] public PackedScene BacterieScene;
    [Export] public int MaxBacterien = 5;

    [Export] public Node3D mainNode;
    [Export] public Node3D SampleNode;
    [Export] public Camera3D mainCamera;
    [Export] public PapierInteractie TafelScript; // Sleep hier je Area3D (Tafel) in!

    private int _overgebleven = 0;

    public override void _Ready()
    {
        // Laat _Ready leeg, we starten via de interactie in de tutorial
    }

    // We halen de _Process weg omdat we het spel handmatig starten 
    // wanneer de speler bij de machine op E drukt.

    public void startSpel()
    {
        GD.Print("Interactie gestart!");

        // 1. Eerst de oude bacteriën van de vorige keer opruimen!
        RuimOudeBacterienOp();

        // 2. Reset de teller naar 0 voor de nieuwe ronde
        _overgebleven = 0;

        // 3. Muis zichtbaar maken voor het klikken
        Input.MouseMode = Input.MouseModeEnum.Visible;

        // 4. Nieuwe bacteriën spawnen
        SpawnBacterien();
        
        GD.Print("Nieuwe minigame gestart!");
    }

    private void RuimOudeBacterienOp()
    {
        // Zoek alle kinderen in deze scene
        foreach (Node kind in GetChildren())
        {
            // We controleren of het kind een Bacterie is (of check op groep "Bacterie")
            // zodat we niet per ongeluk de camera of lichten verwijderen.
            if (kind is Bacterie || kind.IsInGroup("Sample"))
            {
                kind.QueueFree(); // Verwijder de node uit de scene
            }
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
            GD.Print("spawn my pussy");
            // Bepaal een willekeurige positie voor de bacteriën
            float x = (float)GD.RandRange(-3.5, 3.5);
            float y = (float)GD.RandRange(-2.0, 2.0);
            instance.Position = new Vector3(x, y, 0);
            GD.Print("spawn my n****");

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
            await ToSignal(GetTree().CreateTimer(3.0f), SceneTreeTimer.SignalName.Timeout);
        }

        // Nu pas terug naar het lab
        // 1. De minigame node pauzeren en verbergen
            SampleNode.ProcessMode = ProcessModeEnum.Disabled;
            SampleNode.Visible = false;

            // 2. De tutorial weer aanzetten
            mainNode.ProcessMode = ProcessModeEnum.Inherit; 
            mainNode.Visible = true;

            // 3. Camera terug naar de speler
            mainCamera.MakeCurrent();

            // 4. Muis weer verbergen voor de First Person besturing
            Input.MouseMode = Input.MouseModeEnum.Captured;
    }
}
