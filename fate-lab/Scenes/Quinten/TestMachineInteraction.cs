using Godot;
using System;

public partial class TestMachineInteraction : Node3D
{
    private Area3D _interactieZone;
    private Label3D _interactieLabel;
    
    private bool _spelerInBuurt = false;
    private bool _sampleInBuurt = false; // Nieuwe check voor de sample

    private const string InteractieActie = "interactie-bacterie";

    public override void _Ready()
    {
        _interactieZone = GetNode<Area3D>("Area3D");
        _interactieLabel = GetNode<Label3D>("Label3D");

        _interactieZone.AreaEntered += OnAreaEntered; // Gebruik AreaEntered als je sample een Area3D is
        _interactieZone.AreaExited += OnAreaExited;
        
        // Als je sample een RigidBody3D of CharacterBody3D is, gebruik dan BodyEntered:
        _interactieZone.BodyEntered += OnBodyEntered;
        _interactieZone.BodyExited += OnBodyExited;
        
        _interactieLabel.Visible = false;
    }

    public override void _Process(double delta)
    {
        // Interactie kan alleen als BEIDE waar zijn
        if (_spelerInBuurt && _sampleInBuurt && Input.IsActionJustPressed(InteractieActie))
        {
            StartMiniGame();
        }
    }

    private void OnBodyEntered(Node3D body)
    {
        // Check voor de speler
        if (body.IsInGroup("Player"))
        {
            _spelerInBuurt = true;
            UpdateLabelVisibility();
        }
        
        // Check of het object de sample is
        if (body.IsInGroup("Sample"))
        {
			GD.Print("sample in the house");
            _sampleInBuurt = true;
            UpdateLabelVisibility();
        }
    }

    private void OnBodyExited(Node3D body)
    {
        if (body.IsInGroup("Player"))
        {
            _spelerInBuurt = false;
            UpdateLabelVisibility();
        }

        if (body.IsInGroup("Sample"))
        {
            _sampleInBuurt = false;
            UpdateLabelVisibility();
        }
    }

    // Helper methodes voor Area3D (mocht je sample een Area zijn ipv een Body)
    private void OnAreaEntered(Area3D area) => OnBodyEntered(area);
    private void OnAreaExited(Area3D area) => OnBodyExited(area);

    private void UpdateLabelVisibility()
    {
        // Toon het label alleen als de speler er is én de sample er is
        _interactieLabel.Visible = (_spelerInBuurt && _sampleInBuurt);
    }

    private void StartMiniGame()
    {
        string scenePath = "res://Scenes/quinten/SampleMinigame.tscn"; 
        GD.Print("Sample aanwezig! Starten van: " + scenePath);
        GetTree().ChangeSceneToFile(scenePath);
    }
}