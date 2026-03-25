using Godot;
using System;

public partial class BacterieMinigame : Node3D
{
    [Export] public PackedScene BacterieScene; // Vergeet niet de .tscn hierin te slepen!
    [Export] public int MaxBacterien = 5;
    
    private int _overgebleven;

    public override void _Ready()
    {
        // Maak de muis vrij
        Input.MouseMode = Input.MouseModeEnum.Visible;
        SpawnBacterien();
    }

    private void SpawnBacterien()
    {
        if (BacterieScene == null) return;

        for (int i = 0; i < MaxBacterien; i++)
        {
            Bacterie instance = BacterieScene.Instantiate<Bacterie>();
            
    // Verklein het bereik (bijv. van 4.5 naar 3.5)
            float x = (float)GD.RandRange(-3.5, 3.5); // Minder breed
            float y = (float)GD.RandRange(-2.0, 2.0); // Minder hoog
            instance.Position = new Vector3(x, y, 0);

            // Verbind de klik aan de teller
            instance.Geklikt += OnBacterieGeklikt;

            AddChild(instance);
            _overgebleven++;
        }
    }

    private void OnBacterieGeklikt()
    {
        _overgebleven--;
        GD.Print("Nog " + _overgebleven + " over.");

        if (_overgebleven <= 0)
        {
            // Terug naar het lab
            GetTree().ChangeSceneToFile("res://Scenes/Quinten/Testlab-quinten/TestLabQuinten.tscn");
        }
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
            GetTree().ChangeSceneToFile("res://Scenes/Tutku/Lab.tscn");
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