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

    // --- Geëxporteerde Variabelen (In te vullen in de Inspector) ---
    [Export] public Node3D mainNode;      // Je Tutorial/Hoofdwereld
    [Export] public Node3D SampleNode;    // Je Minigame wereld
    [Export] public Camera3D mainCamera;  // De camera van de speler
    [Export] public Camera3D sampleCamera;// De camera van de minigame

    // De naam van de actie die je in Project Settings -> Input Map hebt gemaakt
    private const string InteractieActie = "interactie-bacterie";

    public override void _Ready()
    {
        // 1. Koppel de lokale nodes
        _interactieZone = GetNode<Area3D>("Area3D");
        _interactieLabel = GetNode<Label3D>("Label3D");

        // 2. Verbind de signalen voor de scanner
        // We gebruiken BodyEntered omdat je samples RigidBody3D's zijn
        _interactieZone.BodyEntered += OnBodyEntered;
        _interactieZone.BodyExited += OnBodyExited;
        
        // 3. Initialiseer de starttoestand
        _interactieLabel.Visible = false;

        // VERBERG DE MUIS: Voor de tutorial (First Person look)
        Input.MouseMode = Input.MouseModeEnum.Captured;

        // PAUSEER DE MINIGAME: Zorg dat deze bij de start helemaal niets doet
        SampleNode.ProcessMode = ProcessModeEnum.Disabled;
        SampleNode.Visible = false;
        
        GD.Print("Systeem gereed. Scanner wacht op speler en sample.");
    }

    public override void _Process(double delta)
    {
        // Check of de speler op de interactie-toets drukt
        // Alleen mogelijk als de speler én de sample in de zone staan
        if (_spelerInBuurt && _sampleInBuurt && Input.IsActionJustPressed(InteractieActie))
        {
            StartMiniGame();
        }
    }

    private void OnBodyEntered(Node3D body)
    {
        // Check of de speler de zone binnenloopt (Zorg dat speler in groep "Player" zit)
        if (body.IsInGroup("Player"))
        {
            _spelerInBuurt = true;
            UpdateLabelVisibility();
        }
        
        // Check of een sample (RigidBody) de zone binnenkomt (Zorg dat sample in groep "Sample" zit)
        if (body.IsInGroup("Sample"))
        {
            GD.Print($"Sample gedetecteerd: {body.Name}");
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

    private void UpdateLabelVisibility()
    {
        // Toon de "Druk op E" tekst alleen als beide aanwezig zijn
        _interactieLabel.Visible = (_spelerInBuurt && _sampleInBuurt);
    }

    private void StartMiniGame()
    {
        GD.Print("Interactie geslaagd! Tutorial pauzeren en minigame starten...");

        // 1. PAUSEER DE TUTORIAL
        mainNode.ProcessMode = ProcessModeEnum.Disabled;

        // 2. ACTIVEER DE MINIGAME NODE
        SampleNode.ProcessMode = ProcessModeEnum.Always;
        SampleNode.Visible = true;

        // 3. WISSEL CAMERA & MUIS
        sampleCamera.MakeCurrent();
        Input.MouseMode = Input.MouseModeEnum.Visible;

        // 4. --- DIT IS DE MISSENDE STAP ---
        // Vertel het script op de SampleNode dat het spel moet starten (bacteriën spawnen)
        if (SampleNode is BacterieMinigame minigameScript)
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
        GD.Print("Minigame voltooid! Terug naar de tutorial...");

        // 1. DEACTIVEER DE MINIGAME
        SampleNode.ProcessMode = ProcessModeEnum.Disabled;
        SampleNode.Visible = false;

        // 2. ACTIVEER DE TUTORIAL: Alles gaat weer verder waar het bleef
        mainNode.ProcessMode = ProcessModeEnum.Inherit;

        // 3. WISSEL CAMERA TERUG
        mainCamera.MakeCurrent();

        // 4. VERBERG DE MUIS WEER
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }
}
