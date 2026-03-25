using Godot;
using System;

public partial class SampleMinigame : Node3D
{
     // Dit zorgt ervoor dat je je bacterie-scene in de Inspector kunt slepen
    [Export] public PackedScene InfectionScene; 
    [Export] public int MaxInfections = 5;
   private int _overgebleven;

    
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
        if (InfectionScene == null)
        {
            GD.PrintErr("FOUT: Je bent vergeten de Bacterie Scene in de Inspector te slepen!");
            return;
        }
        RandomNumberGenerator rng = new RandomNumberGenerator();
        rng.Randomize();
        for (int i = 0; i < MaxInfections; i++)
        {
            // Maak een nieuwe bacterie aan
            var instance = InfectionScene.Instantiate<MinigameInteraction>();
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
            GetTree().ChangeSceneToFile("res://Scenes/Quinten/Testlab-quinten/TestLabQuinten.tscn");
        }
    }
}












/*using Godot;
using System;

public partial class SampleMinigame : Node3D
{
     // Dit zorgt ervoor dat je je bacterie-scene in de Inspector kunt slepen
    [Export] public PackedScene InfectionScene; 
    [Export] public int MaxInfections = 5;
   private int _overgebleven;

    
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
        if (InfectionScene == null)
        {
            GD.PrintErr("FOUT: Je bent vergeten de Bacterie Scene in de Inspector te slepen!");
            return;
        }
        RandomNumberGenerator rng = new RandomNumberGenerator();
        rng.Randomize();
        for (int i = 0; i < MaxInfections; i++)
        {
            // Maak een nieuwe bacterie aan
            var instance = InfectionScene.Instantiate<MinigameInteraction>();
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