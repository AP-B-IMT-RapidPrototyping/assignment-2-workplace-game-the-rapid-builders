using Godot;
using System;

public partial class main_kleurencode : Area3D
{
    [Export] public MeshInstance3D BolMesh;

    private Color _kleurStandaard = new Color(0.5f, 0.5f, 0.5f); 
    private Color _kleurGoed = new Color(0, 1, 0);               
    private Color _kleurSlecht = new Color(1, 0, 0);             
    private Color _kleurInfectie = new Color(1, 1, 0); // GEEL

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        UpdateVisuals(_kleurStandaard);
    }

    private async void OnBodyEntered(Node3D body)
    {
        // We checken nu op METADATA
        if (body.HasMeta("IsGoed"))
        {
            string status = body.GetMeta("IsGoed").AsString();
            GD.Print("SCANNER: Status herkend via Meta -> " + status);

            Color gekozenKleur = _kleurSlecht;
            if (status == "Goed") gekozenKleur = _kleurGoed;
            else if (status == "Geinfecteerd") gekozenKleur = _kleurInfectie;

            UpdateVisuals(gekozenKleur);

            await ToSignal(GetTree().CreateTimer(2.0f), SceneTreeTimer.SignalName.Timeout);
            UpdateVisuals(_kleurStandaard);
        }
        else
        {
            GD.Print("SCANNER: Geen status gevonden op " + body.Name);
        }
    }

    private void UpdateVisuals(Color kleur)
    {
        if (BolMesh == null) return;
        StandardMaterial3D tempMat = new StandardMaterial3D();
        tempMat.AlbedoColor = kleur;
        tempMat.EmissionEnabled = true;
        tempMat.Emission = kleur;
        tempMat.EmissionEnergyMultiplier = 3.0f;
        BolMesh.MaterialOverride = tempMat;
    }
}