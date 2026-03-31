using Godot;
using System;

public partial class BacterieMinigame : Node3D
{
    [Export] public PackedScene BacterieScene;
    [Export] public int MaxBacterien = 5;

    [Export] public Node3D mainNode;
    [Export] public Node3D SampleNode;
    [Export] public Camera3D mainCamera;

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
        if (BacterieScene == null) return;

        for (int i = 0; i < MaxBacterien; i++)
        {
            Bacterie instance = BacterieScene.Instantiate<Bacterie>();
            
            float x = (float)GD.RandRange(-3.5, 3.5);
            float y = (float)GD.RandRange(-2.0, 2.0);
            instance.Position = new Vector3(x, y, 0);

            // Belangrijk: Verbind het signaal
            instance.Geklikt += OnBacterieGeklikt;

            AddChild(instance);
            _overgebleven++;
        }
    }

    private void OnBacterieGeklikt()
    {
        _overgebleven--;
        GD.Print($"Nog {_overgebleven} over.");

        if (_overgebleven <= 0)
        {
            StopSpel();
        }
    }

    private void StopSpel()
    {
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
        
        GD.Print("Minigame voltooid, terug naar lab.");
    }
}

/*using Godot;
using System;

public partial class BacterieMinigame : Node3D
{
    [Export] public PackedScene BacterieScene; // Sleep hier je bacterie3d.tscn in!
    [Export] public int MaxBacterien = 10;
    
    private int _overgeblevenBacterien = 0;

    public override void _Ready()
    {
        // Maak de muis vrij zodat je kunt klikken
        Input.MouseMode = Input.MouseModeEnum.Visible;
        
        SpawnBacterien();
    }

    private void SpawnBacterien()
    {
        if (BacterieScene == null)
        {
            GD.PrintErr("FOUT: Sleep de bacterie3d.tscn in de Inspector bij BacterieScene!");
            return;
        }

        for (int i = 0; i < MaxBacterien; i++)
        {
            // Maak een nieuwe bacterie van de scene
            Bacterie instance = BacterieScene.Instantiate<Bacterie>();

            // Positie verspreiden (Pas aan op basis van je camera zicht)
            float x = (float)GD.RandRange(-4.0, 4.0);
            float y = (float)GD.RandRange(-2.5, 2.5);
            instance.Position = new Vector3(x, y, 0);

            // Verbind het signaal van de bacterie aan deze manager
            instance.Geklikt += OnBacterieGeklikt;

            AddChild(instance);
            _overgeblevenBacterien++;
        }
    }

    private void OnBacterieGeklikt()
    {
        _overgeblevenBacterien--;
        GD.Print("Bacterie vernietigd. Nog " + _overgeblevenBacterien + " over.");

        if (_overgeblevenBacterien <= 0)
        {
            VoltooiMinigame();
        }
    }

    private void VoltooiMinigame()
    {
        GD.Print("Minigame klaar! Terug naar het Lab.");
        // Zorg dat dit pad exact klopt met jouw hoofd-scene
        GetTree().ChangeSceneToFile("res://Scenes/Tutku/Lab.tscn");
    }
}*/

/*using Godot;
using System;

public partial class BacterieMinigame : Node3D
{
    // Dit zorgt ervoor dat je je bacterie-scene in de Inspector kunt slepen
    [Export] public PackedScene BacterieScene; 
    [Export] public int MaxBacterien = 10;
    
    private int _geteld = 0;

    public override void _Ready()
    {
        // 1. Maak de muis zichtbaar
        Input.MouseMode = Input.MouseModeEnum.Visible;
        
        // 2. Start het spawnen van bacteriën
        SpawnBacterien();
    }

    private void SpawnBacterien()
    {
        if (BacterieScene == null)
        {
            GD.PrintErr("FOUT: Je bent vergeten de Bacterie Scene in de Inspector te slepen!");
            return;
        }

        RandomNumberGenerator rng = new RandomNumberGenerator();
        rng.Randomize();

        for (int i = 0; i < MaxBacterien; i++)
        {
            // Maak een nieuwe bacterie aan
            var instance = BacterieScene.Instantiate<Bacterie>();

            // Bepaal een willekeurige plek (pas de getallen aan voor je schermgrootte)
            float x = rng.RandfRange(-5.0f, 5.0f);
            float y = rng.RandfRange(-3.0f, 3.0f);
            instance.Position = new Vector3(x, y, 0);

            // Verbind het klik-signaal zodat we kunnen tellen
            instance.Geklikt += CheckWin;

            // Voeg de bacterie toe aan de scene
            AddChild(instance);
            _geteld++;
        }
        
        GD.Print("Bacteriën gespawned: " + _geteld);
    }

    private void CheckWin()
    {
        _geteld--;
        GD.Print("Bacterie weg! Nog " + _geteld + " te gaan.");

        if (_geteld <= 0)
        {
            GD.Print("Gewonnen! Terug naar het lab.");
            GetTree().ChangeSceneToFi²le("res://Scenes/Tutku/Lab.tscn");
        }
    }
}*/

/*using Godot;
using System;

public partial class BacterieMinigame : Node3D
{
    [Export] public int TotaalBacterien = 5;
    private int _geteld = 0;

    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Visible;
        
        // Zoek alle bacteriën die je handmatig in de scene hebt gezet
        var bacterien = GetTree().GetNodesInGroup("Bacterien");
        _geteld = bacterien.Count;
        
        foreach (Node b in bacterien)
        {
            if (b is Bacterie bacterieNode)
            {
                bacterieNode.Geklikt += CheckWin;
            }
        }
    }

    private void CheckWin()
    {
        _geteld--;
        if (_geteld <= 0)
        {
            GetTree().ChangeSceneToFile("res://Scenes/Tutku/Lab.tscn");
        }
    }
}*/