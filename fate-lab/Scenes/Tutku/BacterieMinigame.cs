using Godot;
using System;

public partial class BacterieMinigame : Node3D
{
    [Export] public PackedScene BacterieScene;
    [Export] public int MaxBacterien = 5;

    [Export] public Node3D mainNode;
    [Export] public Node3D SampleNode;
    [Export] public Camera3D mainCamera;
    [Export] public PapierInteractie TafelScript; 
    [Export] public ProgressBarUpdate HartslagMeter;
    
    // Voeg dit toe voor een tijdslimiet:
    [Export] public float Tijdslimiet = 10.0f; // 10 seconden om te winnen

    public InteractionMicroscope DeMicroscoop;
    private int _overgebleven = 0;

    public override void _Ready()
    {
    }


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

    public async void StopDeGame(bool isGewonnen)
    {
        GD.Print("BacterieMinigame: StopDeGame aangeroepen.");

        if (isGewonnen)
        {
            var machines = GetTree().GetNodesInGroup("MachineGroup");
            var microscopen = GetTree().GetNodesInGroup("MicroscopeGroup");
            
            bool succesvolAfgehandeld = false;

            if (machines.Count > 0)
            {
                var machine = machines[0] as Main_TestMachineInteraction;
                if (machine != null && machine.Get("_huidigSample").As<Node3D>() != null) 
                {
                    machine.StopMiniGame();
                    GD.Print("BacterieMinigame: Machine succesvol afgehandeld.");
                    succesvolAfgehandeld = true;
                }
            }
            
            if (!succesvolAfgehandeld && microscopen.Count > 0)
            {
                var micro = microscopen[0] as Main_MicroscopeInteraction;
                if (micro != null)
                {
                    micro.StopMicroscoop();
                    GD.Print("BacterieMinigame: Microscoop succesvol afgehandeld.");
                }
            }
            if (!succesvolAfgehandeld && DeMicroscoop != null)
            {
                DeMicroscoop.StopMiniGame();
                GD.Print("BacterieMinigame: Directe Microscoop-link succesvol afgehandeld.");
                succesvolAfgehandeld = true;
            }

            if (!succesvolAfgehandeld)
            {
                GD.Print("FOUT: Geen machine of microscoop gevonden in groepen! We gebruiken de fallback.");
                HandmatigeCameraReset();
            }
        }
        else
        {
            HandmatigeCameraReset();
        }
    }

    // Kleine helper om de camera te resetten als de machine-aanroep faalt of bij verlies
    private void HandmatigeCameraReset()
    {
        // 1. Zet de minigame wereld uit (dit geldt voor beide apparaten)
        // We gebruiken 'this' omdat dit script op de minigame node staat
        this.ProcessMode = ProcessModeEnum.Disabled;
        this.Visible = false;

        // 2. Vertel de hoofdwereld dat hij weer mag draaien
        if (mainNode != null)
        {
            mainNode.ProcessMode = ProcessModeEnum.Inherit;
            mainNode.Visible = true;
        }

        // 3. Camera en Muis reset
        // We gebruiken de mainCamera die je in de Inspector hebt gesleept
        if (mainCamera != null)
        {
            mainCamera.MakeCurrent();
        }
        
        Input.MouseMode = Input.MouseModeEnum.Captured;
        
        GD.Print("BacterieMinigame: Handmatige reset uitgevoerd (fallback).");
    }
}
