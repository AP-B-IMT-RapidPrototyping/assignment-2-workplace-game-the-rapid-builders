using Godot;
using System;

public partial class MachineInteraction : Node3D
{
    // Verwijzingen naar nodes (Caching voorkomt fouten bij herladen)
    private Area3D _interactieZone;
    private Label3D _interactieLabel;
    private bool _spelerInBuurt = false;

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
    string scenePath = "res://Scenes/quinten/SampleMinigame.tscn"; 
    GD.Print("Wisselen naar scene: " + scenePath);
    GetTree().ChangeSceneToFile(scenePath);

    }
}
