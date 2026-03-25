using Godot;
using System;

public partial class VloerBeheer : MeshInstance3D
{
    public override void _Ready()
    {
        // 1. Pak het materiaal van de mesh vast
        // We maken een unieke kopie zodat niet alle muren ook veranderen
        StandardMaterial3D vloerMaterial = (StandardMaterial3D)GetActiveMaterial(0).Duplicate();
        SetSurfaceOverrideMaterial(0, vloerMaterial);

        // 2. Kleur aanpassen via script (bijv. donkergrijs)
        vloerMaterial.AlbedoColor = new Color(0.2f, 0.2f, 0.2f);

        // 3. Tegel herhaling aanpassen (UV1 Scale)
        // Hoe groter de Vector3, hoe meer tegels je ziet
        vloerMaterial.Uv1Scale = new Vector3(20, 20, 20);
        
        // 4. Triplanar aanzetten zodat het niet uitrekt
        vloerMaterial.Uv1Triplanar = true;
    }
}