using Godot;
using System;

public partial class MicroscoopInteractie : Node3D
{
    // 1. VOEG DEZE EXPORT TOE:
    // Hiermee kun je in de Godot Editor je 'Papier' node in dit vakje slepen.
    [Export] public PapierInteractie TafelScript;

    private Area3D _interactieZone;
    private Label3D _interactieLabel;
    private bool _spelerInBuurt = false;

    private const string InteractieActie = "interactie-bacterie";

    public override void _Ready()
    {
        _interactieZone = GetNode<Area3D>("Area3D");
        _interactieLabel = GetNode<Label3D>("Label3D");

        _interactieZone.BodyEntered += OnBodyEntered;
        _interactieZone.BodyExited += OnBodyExited;
        
        _interactieLabel.Visible = false;
    }

    public override void _Process(double delta)
    {
        if (_spelerInBuurt && Input.IsActionJustPressed(InteractieActie))
        {
            // 2. CHECK HIER DE TOESTEMMING:
            // We vragen aan het TafelScript: "Is de test geaccepteerd?"
            if (TafelScript != null && TafelScript.MagMinigameStarten())
            {
                StartMiniGame();
            }
            else
            {
                // Optioneel: Geef feedback aan de speler
                GD.Print("Toegang geweigerd: Accepteer eerst de test in het medisch dossier.");
                _interactieLabel.Text = "Accepteer eerst dossier!"; // Verander tijdelijk de tekst
            }
        }
    }

    private void OnBodyEntered(Node3D body)
    {
        if (body.IsInGroup("Player"))
        {
            _spelerInBuurt = true;
            
            // Pas de tekst aan op basis van of de test mag starten
            if (TafelScript != null && TafelScript.MagMinigameStarten()) {
                _interactieLabel.Text = "Druk E voor Microscoop";
            } else {
                _interactieLabel.Text = "Bekijk eerst dossier";
            }
            
            _interactieLabel.Visible = true;
        }
    }

    private void OnBodyExited(Node3D body)
    {
        if (body.IsInGroup("Player"))
        {
            _spelerInBuurt = false;
            _interactieLabel.Visible = false;
        }
    }

    private void StartMiniGame()
    {
        string scenePath = "res://Scenes/Tutku/bacterie_3d.tscn"; 
        GD.Print("Wisselen naar scene: " + scenePath);
        GetTree().ChangeSceneToFile(scenePath);
    }
}

/*using Godot;
using System;

public partial class MicroscoopInteractie : Node3D
{
    // Verwijzingen naar nodes (Caching voorkomt fouten bij herladen)
    private Area3D _interactieZone;
    private Label3D _interactieLabel;
    private bool _spelerInBuurt = false;

    [Export] public Node3D mainNode;
    [Export] public Node3D SampleNode;

    // De naam van je actie in de Input Map
    private const string InteractieActie = "interactie-bacterie";

    public override void _Ready()
    {
        // We zoeken de nodes op. Als ze niet bestaan, geeft Godot een foutmelding.
        _interactieZone = GetNode<Area3D>("Area3D");
        _interactieLabel = GetNode<Label3D>("Label3D");



        // Verbind de signals opnieuw
        _interactieZone.BodyEntered += OnBodyEntered;
        _interactieZone.BodyExited += OnBodyExited;
        
        // Zorg dat het label bij de start altijd onzichtbaar is
        _interactieLabel.Visible = false;
    }

    public override void _Process(double delta)
    {
        // Alleen checken als de speler daadwerkelijk in de zone staat
        if (_spelerInBuurt && Input.IsActionJustPressed(InteractieActie))
        {
            StartMiniGame();
        }
    }

    private void OnBodyEntered(Node3D body)
    {
        // Check of het de speler is (Zorg dat je speler in de groep "Player" zit!)
        if (body.IsInGroup("Player"))
        {
            _spelerInBuurt = true;
            _interactieLabel.Visible = true; // Toon de "E"
        }
    }

    private void OnBodyExited(Node3D body)
    {
        if (body.IsInGroup("Player"))
        {
            _spelerInBuurt = false;
            _interactieLabel.Visible = false; // Verberg de "E"
        }
    }

    private void StartMiniGame()
    {

        // Let op: het pad moet EXACT kloppen (hoofdletters/kleine letters)
        string scenePath = "res://Scenes/Tutku/bacterie_3d.tscn"; 
        GD.Print("Wisselen naar scene: " + scenePath);
        SampleNode.Visible = true;
        mainNode.Visible = true;

    }
}*/
