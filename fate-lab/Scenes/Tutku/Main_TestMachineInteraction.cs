using Godot;
using System;

public partial class Main_TestMachineInteraction : Node3D
{
	// --- Nodes & UI ---
    private Area3D _interactieZone;
    private Label3D _interactieLabel;
    
    // --- Status Checks ---
    private bool _spelerInBuurt = false;
    private bool _sampleInBuurt = false;
	private Node3D _huidigSample = null;
	private bool _isBezigMetMinigame = false;

    // --- Geëxporteerde Variabelen (In te vullen in de Inspector) ---
    [Export] public Node3D mainNode;      // Je Tutorial/Hoofdwereld
    [Export] public Node3D SampleNode;    // Je Minigame wereld
    [Export] public Camera3D mainCamera;  // De camera van de speler
    [Export] public Camera3D sampleCamera;// De camera van de minigame
	[Signal] public delegate void MinigameKlaarEventHandler();

    // De naam van de actie die je in Project Settings -> Input Map hebt gemaakt
    private const string InteractieActie = "interactie-bacterie";

    public override void _Ready()
    {
		AddToGroup("MachineGroup");
        // 1. Koppel de lokale nodes
        _interactieZone = GetNode<Area3D>("Area3D");
        _interactieLabel = GetNode<Label3D>("Label3D");

        // 2. Verbind de signalen voor de scanner
        // We gebruiken BodyEntered omdat je samples RigidBody3D's zijn
        _interactieZone.BodyEntered += OnBodyEntered;
        _interactieZone.BodyExited += OnBodyExited;
        
        // 3. Initialiseer de starttoestand
        _interactieLabel.Visible = false;

        // PAUSEER DE MINIGAME: Zorg dat deze bij de start helemaal niets doet
        SampleNode.ProcessMode = ProcessModeEnum.Disabled;
        SampleNode.Visible = false;
        
		GD.Print("SYSTEEM: Machine klaar voor gebruik.");    
	}

    public override void _Process(double delta)
    {
        // Check of de speler op de interactie-toets drukt
        // Alleen mogelijk als de speler én de sample in de zone staan
        if (!_isBezigMetMinigame && _spelerInBuurt && _sampleInBuurt && Input.IsActionJustPressed(InteractieActie))
        {
            StartMiniGame();
        }
    }

    private void OnBodyEntered(Node3D body)
    {
		GD.Print($"Machine voelt nu: {body.Name} (Groepen: {string.Join(", ", body.GetGroups())})");
        // Check of de speler de zone binnenloopt (Zorg dat speler in groep "Player" zit)
        if (body.IsInGroup("Player"))
        {
            _spelerInBuurt = true;
            UpdateLabelVisibility();
        }
        
		// Alleen een nieuw sample accepteren als we niet bezig zijn
        if (!_isBezigMetMinigame && body.IsInGroup("Sample"))
        {
			if (body.IsInGroup("infected")) 
        	{
				GD.Print($"MACHINE: Geïnfecteerd object herkend: {body.Name}");
				_huidigSample = body;
				_sampleInBuurt = true;
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
		if (_isBezigMetMinigame && body == _huidigSample) return;
        if (body.IsInGroup("Player"))
        {
            _spelerInBuurt = false;
        }

        if (body == _huidigSample)
        {
            _huidigSample = null;
            _sampleInBuurt = false;
        }
		UpdateLabelVisibility();
    }

    private void UpdateLabelVisibility()
    {
        // Toon de "Druk op E" tekst alleen als beide aanwezig zijn
		_interactieLabel.Visible = (_spelerInBuurt && _sampleInBuurt && !_isBezigMetMinigame);    }

    private void StartMiniGame()
    {
		if (_huidigSample == null) return;

        _isBezigMetMinigame = true;
        GD.Print("MACHINE: Minigame wordt opgestart...");

        // 1. PAUSEER DE TUTORIAL
        mainNode.ProcessMode = ProcessModeEnum.Disabled;

        // 2. ACTIVEER DE MINIGAME NODE
        SampleNode.ProcessMode = ProcessModeEnum.Always;
        SampleNode.Visible = true;

        // 3. WISSEL CAMERA & MUIS
        sampleCamera.MakeCurrent();
        Input.MouseMode = Input.MouseModeEnum.Visible;
		var minigameScript = SampleNode as BacterieMinigame ?? SampleNode.GetNodeOrNull<BacterieMinigame>("BacterieMinigame");

        // 4. --- DIT IS DE MISSENDE STAP ---
        // Vertel het script op de SampleNode dat het spel moet starten (bacteriën spawnen)
        if (minigameScript != null)
        {
            minigameScript.startSpel(); 
            GD.Print("BacterieMinigame script aangeroepen!");
        }
        else
        {
            // Debug: Als je dit ziet, staat het script niet direct op de SampleNode
            // maar misschien op een kind-node?
            GD.Print("FOUT: Kon BacterieMinigame script niet vinden op SampleNode.");
            
            // Optioneel: probeer het script te vinden op een kind als het niet op de root staat
            var scriptOpKind = SampleNode.GetNodeOrNull<BacterieMinigame>("NaamVanJeKindNode");
            scriptOpKind?.startSpel();
        }
    }

    // Tip: Roep deze methode aan vanuit je minigame-script als je klaar bent!
	public void StopMiniGame()
	{
		GD.Print("--- MACHINE: START OPSCHONING ---");

		if (_huidigSample != null && IsInstanceValid(_huidigSample))
        {
            // A. META AANPASSEN (Voor scanner en dozen)
            _huidigSample.SetMeta("IsGoed", "Goed");
            GD.Print("MACHINE: Meta 'IsGoed' aangepast naar 'Goed'.");

            // B. GROEP VERWIJDEREN (Voor machine-interactie)
            if (_huidigSample.IsInGroup("infected"))
            {
                _huidigSample.RemoveFromGroup("infected");
                GD.Print("MACHINE: Groep 'infected' verwijderd.");
            }

            // C. VISUELE STIPPEN VERWIJDEREN
            foreach (Node child in _huidigSample.GetChildren())
            {
                // We verwijderen extra MeshInstances (de stippen), maar laten de hoofdmesh staan
                if (child is MeshInstance3D && child.Name != "MeshInstance3D")
                {
                    child.QueueFree();
                }
            }
            GD.Print($"MACHINE: {_huidigSample.Name} is nu volledig hersteld.");
        }
        else
        {
            GD.PrintErr("MACHINE FOUT: Geen referentie naar sample gevonden bij afsluiten!");
        }

        // RESET MACHINE STATUS
        _isBezigMetMinigame = false;
		_huidigSample = null;
		_sampleInBuurt = false; 
        SampleNode.ProcessMode = ProcessModeEnum.Disabled;
        SampleNode.Visible = false;
        
        mainNode.ProcessMode = ProcessModeEnum.Inherit;
        mainCamera.MakeCurrent();
        Input.MouseMode = Input.MouseModeEnum.Captured;

        // Forceer label update
        UpdateLabelVisibility();
        
        GD.Print("--- MACHINE: TERUG NAAR HOOFDWERELD ---");
	}
}
