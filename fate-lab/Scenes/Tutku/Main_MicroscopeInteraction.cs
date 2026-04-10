using Godot;
using System;

public partial class Main_MicroscopeInteraction : Node3D
{
    // --- Nodes & UI ---
    private Area3D _interactieZone;
    private Label3D _interactieLabel;
    
    // --- Status Checks ---
    private bool _spelerInBuurt = false;
    private bool _orgaanInBuurt = false;
    private Node3D _huidigOrgaan = null;
    private bool _isBezigMetMicroscoop = false;

    // --- Export variabelen (Slepen in de Inspector) ---
    [Export] public Node3D mainNode;       // De gewone wereld
    [Export] public Node3D MicroscoopWorld; // De wereld van de microscoop-minigame
    [Export] public Camera3D mainCamera;   // Speler camera
    [Export] public Camera3D microCamera;  // Microscoop camera

    private const string InteractieActie = "interactie-bacterie"; // Of een eigen actie zoals "interactie-micro"

    public override void _Ready()
    {
        // Forceer dat de microscoop ook vindbaar is (voor als de minigame terug moet praten)
        AddToGroup("MicroscopeGroup");

        _interactieZone = GetNode<Area3D>("Area3D");
        _interactieLabel = GetNode<Label3D>("Label3D");

        _interactieZone.BodyEntered += OnBodyEntered;
        _interactieZone.BodyExited += OnBodyExited;

        _interactieLabel.Visible = false;
        MicroscoopWorld.ProcessMode = ProcessModeEnum.Disabled;
        MicroscoopWorld.Visible = false;

        GD.Print("MICROSCOOP: Systeem gereed voor organen.");
    }

    public override void _Process(double delta)
    {
        if (!_isBezigMetMicroscoop && _spelerInBuurt && _orgaanInBuurt && Input.IsActionJustPressed(InteractieActie))
        {
            StartMicroscoopMinigame();
        }
    }

    private void OnBodyEntered(Node3D body)
    {
        if (body.IsInGroup("Player"))
        {
            _spelerInBuurt = true;
			UpdateLabelVisibility();
        }
        
        // CHECK: Alleen accepteren als het een ORGAN is en INFECTED
        if (!_isBezigMetMicroscoop && body.IsInGroup("Organ"))
        {
			if (body.IsInGroup("infected")) 
        	{
				GD.Print($"MICROSCOOP: Geïnfecteerd orgaan gedetecteerd: {body.Name}");
				_huidigOrgaan = body;
				_orgaanInBuurt = true;
				UpdateLabelVisibility();
			}
			else
			{
				GD.Print($"FOUT: {body.Name} is niet geïnfecteerd.");			

			}
        }
    }

    private void OnBodyExited(Node3D body)
    {
        if (_isBezigMetMicroscoop && body == _huidigOrgaan) return;

        if (body.IsInGroup("Player")) _spelerInBuurt = false;
        if (body == _huidigOrgaan)
        {
            _huidigOrgaan = null;
            _orgaanInBuurt = false;
        }
        UpdateLabelVisibility();
    }

    private void UpdateLabelVisibility()
    {
        _interactieLabel.Visible = (_spelerInBuurt && _orgaanInBuurt && !_isBezigMetMicroscoop);
    }

    private void StartMicroscoopMinigame()
    {
        if (_huidigOrgaan == null) return;

		_isBezigMetMicroscoop = true;
		GD.Print("MICROSCOOP: Minigame start...");

		// 1. Wissel werelden
		mainNode.ProcessMode = ProcessModeEnum.Disabled;
		MicroscoopWorld.ProcessMode = ProcessModeEnum.Always;
		MicroscoopWorld.Visible = true;

		// 2. Camera & Muis
		microCamera.MakeCurrent();
		Input.MouseMode = Input.MouseModeEnum.Visible;

		// 3. --- DIT IS DE STAP DIE JE MIST ---
		// We zoeken het script dat de bacteriën moet spawnen op de MicroscoopWorld node
		var minigameScript = MicroscoopWorld as BacterieMinigame ?? MicroscoopWorld.GetNodeOrNull<BacterieMinigame>("BacterieMinigame");

		if (minigameScript != null)
		{
			minigameScript.startSpel(); 
			GD.Print("MICROSCOOP: BacterieMinigame.startSpel() succesvol aangeroepen!");
		}
		else
		{
			GD.PrintErr("MICROSCOOP FOUT: Kan het BacterieMinigame script niet vinden op MicroscoopWorld!");
		}
    }

    // Deze functie wordt aangeroepen als de minigame klaar is
    public void StopMicroscoop()
    {
        GD.Print("MICROSCOOP: Opschonen orgaan...");

        if (_huidigOrgaan != null && IsInstanceValid(_huidigOrgaan))
        {
            _huidigOrgaan.SetMeta("IsGoed", "Goed");
            if (_huidigOrgaan.IsInGroup("infected")) _huidigOrgaan.RemoveFromGroup("infected");

            // Verwijder bacteriën/stippen op het orgaan
            foreach (Node child in _huidigOrgaan.GetChildren())
            {
                if (child is MeshInstance3D && child.Name != "MeshInstance3D") child.QueueFree();
            }
        }

        // Resets
        _isBezigMetMicroscoop = false;
        _huidigOrgaan = null;
        _orgaanInBuurt = false;

        MicroscoopWorld.ProcessMode = ProcessModeEnum.Disabled;
        MicroscoopWorld.Visible = false;
        mainNode.ProcessMode = ProcessModeEnum.Inherit;
        mainCamera.MakeCurrent();
        Input.MouseMode = Input.MouseModeEnum.Captured;
        
        UpdateLabelVisibility();
    }
}