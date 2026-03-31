using Godot;
using System;
using System.Threading.Tasks;

public partial class KleurcodeOnderzoeken : Area3D
{
	[Export] public MeshInstance3D BolMesh;

	// Kleuren definiëren
    private Color _kleurStandaard = new Color(0.8f, 0.8f, 0.8f); // Grijs/Wit (zoals op je foto)
    private Color _kleurGoed = new Color(0, 1, 0);               // Groen
    private Color _kleurSlecht = new Color(1, 0, 0);             // Rood

    private StandardMaterial3D _materiaal;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;

        // DIT IS NIEUW: Maak het materiaal hier één keer uniek
        if (BolMesh != null && BolMesh.GetActiveMaterial(0) is StandardMaterial3D mat)
        {
            _materiaal = (StandardMaterial3D)mat.Duplicate();
            BolMesh.SetSurfaceOverrideMaterial(0, _materiaal);
        }
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	private async void OnBodyEntered(Node3D body)
    {
        // Check of het een orgaan is
        if (body is TutLungs orgaan)
        {
            GD.Print("Scanner detecteert: " + orgaan.IsGoed);

            // Verander de kleur van het opgeslagen materiaal
            if (orgaan.IsGoed == "Goed")
            {
                _materiaal.AlbedoColor = _kleurGoed;
            }
            else
            {
                _materiaal.AlbedoColor = _kleurSlecht;
            }

            // Wacht 2 seconden
            await ToSignal(GetTree().CreateTimer(2.0f), SceneTreeTimer.SignalName.Timeout);

            // Reset naar standaard kleur
            _materiaal.AlbedoColor = _kleurStandaard;
            
            GD.Print("Scanner gereset naar standaard.");
        }

        if (body is TutBloodSample Bloed)
        {
            GD.Print("Scanner detecteert: " + Bloed.IsGoed);

            // Verander de kleur van het opgeslagen materiaal
            if (Bloed.IsGoed == "Goed")
            {
                _materiaal.AlbedoColor = _kleurGoed;
            }
            else
            {
                _materiaal.AlbedoColor = _kleurSlecht;
            }

            // Wacht 2 seconden
            await ToSignal(GetTree().CreateTimer(2.0f), SceneTreeTimer.SignalName.Timeout);

            // Reset naar standaard kleur
            _materiaal.AlbedoColor = _kleurStandaard;
            
            GD.Print("Scanner gereset naar standaard.");
        }
    
        if (body is TutDrugSample Drug)
        {
            GD.Print("Scanner detecteert: " + Drug.IsGoed);

            // Verander de kleur van het opgeslagen materiaal
            if (Drug.IsGoed == "Goed")
            {
                _materiaal.AlbedoColor = _kleurGoed;
            }
            else
            {
                _materiaal.AlbedoColor = _kleurSlecht;
            }

            // Wacht 2 seconden
            await ToSignal(GetTree().CreateTimer(2.0f), SceneTreeTimer.SignalName.Timeout);

            // Reset naar standaard kleur
            _materiaal.AlbedoColor = _kleurStandaard;
            
            GD.Print("Scanner gereset naar standaard.");
        }
    }
}
