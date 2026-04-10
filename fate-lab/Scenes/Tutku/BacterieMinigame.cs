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
// Verander dit als het nog op 'ProgressBarAftikken' stond
    [Export] public ProgressBarUpdate HartslagMeter;
    
    // Voeg dit toe voor een tijdslimiet:
    [Export] public float Tijdslimiet = 10.0f; // 10 seconden om te winnen

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
        GD.Print("BacterieMinigame: StopDeGame aangeroepen.");

        if (isGewonnen)
        {
            // Zoek de machine via de groep (werkt altijd, ongeacht de naam of plek)
            var machines = GetTree().GetNodesInGroup("MachineGroup");
            
            if (machines.Count > 0)
            {
                // We pakken de eerste node uit de groep en casten hem naar jouw script
                var machine = machines[0] as Main_TestMachineInteraction;
                if (machine != null)
                {
                    GD.Print("BacterieMinigame: Machine gevonden via groep! Roep StopMiniGame aan...");
                    machine.StopMiniGame();
                    return; // Stop hier, want de machine regelt de rest (camera, muis, etc.)
                }
            }
            else
            {
                GD.PrintErr("CRITISCHE FOUT: Geen node gevonden in 'MachineGroup'! Heb je de groep toegevoegd in de Editor?");
            }
        }

        // Fallback voor als er geen machine is of je hebt verloren
        SampleNode.ProcessMode = ProcessModeEnum.Disabled;
        SampleNode.Visible = false;
        mainNode.ProcessMode = ProcessModeEnum.Inherit; 
        mainCamera.MakeCurrent();
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    // Kleine helper om de camera te resetten als de machine-aanroep faalt of bij verlies
    private void HandmatigeCameraReset()
    {
        SampleNode.ProcessMode = ProcessModeEnum.Disabled;
        SampleNode.Visible = false;
        mainNode.ProcessMode = ProcessModeEnum.Inherit; 
        mainNode.Visible = true;
        mainCamera.MakeCurrent();
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }
}
