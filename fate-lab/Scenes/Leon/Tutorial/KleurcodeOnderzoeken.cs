using Godot;
using System;

public partial class KleurcodeOnderzoeken : Area3D
{
    [Export] public MeshInstance3D BolMesh;

    private Color _kleurStandaard = new Color(0.8f, 0.8f, 0.8f); // Grijs
    private Color _kleurGoed = new Color(0, 1, 0);               // Groen
    private Color _kleurSlecht = new Color(1, 0, 0);             // Rood

    private StandardMaterial3D _materiaal;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;

        if (BolMesh != null)
        {
            // We maken een gloednieuw materiaal aan. 
            // Dit lost problemen op met objecten die vanuit Blender komen.
            _materiaal = new StandardMaterial3D();
            _materiaal.AlbedoColor = _kleurStandaard;
            
            // We dwingen de mesh om dit nieuwe materiaal te gebruiken op alle lagen
            int surfaceCount = BolMesh.GetSurfaceOverrideMaterialCount();
            if (surfaceCount > 0)
            {
                for (int i = 0; i < surfaceCount; i++)
                {
                    BolMesh.SetSurfaceOverrideMaterial(i, _materiaal);
                }
            }
            else
            {
                // Als er geen overrides zijn, zetten we hem op de basis
                BolMesh.MaterialOverride = _materiaal;
            }
        }
        else
        {
            GD.PrintErr("SCANNER FOUT: BolMesh is niet toegewezen in de Inspector!");
        }
    }

    private async void OnBodyEntered(Node3D body)
    {
        // Zoek naar de waarde IsGoed
        Variant statusVariant = body.Get("IsGoed");

        if (statusVariant.VariantType != Variant.Type.Nil)
        {
            string status = statusVariant.AsString();
            GD.Print("SCANNER: Status gevonden -> " + status);

            // Kleur aanpassen
            if (status == "Goed")
            {
                _materiaal.AlbedoColor = _kleurGoed;
            }
            else
            {
                _materiaal.AlbedoColor = _kleurSlecht;
            }

            // Wacht 2 seconden
            await ToSignal(GetTree().CreateTimer(2.0f), SceneTreeTimer.SignalName.Timeout);

            // Reset
            _materiaal.AlbedoColor = _kleurStandaard;
            GD.Print("SCANNER: Kleur gereset naar standaard.");
        }
    }
}