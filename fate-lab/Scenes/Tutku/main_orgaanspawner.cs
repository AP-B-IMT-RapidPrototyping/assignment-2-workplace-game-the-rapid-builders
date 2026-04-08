using Godot;
using System;

public partial class main_orgaanspawner : Node3D
{
    private Random _random = new Random();

    [ExportGroup("Object Scènes")]
    [Export] public PackedScene HeartScene;
    [Export] public PackedScene LungScene;
    [Export] public PackedScene BloodSampleScene;
    [Export] public PackedScene DrugSampleScene;

    [ExportGroup("Grootte Instellingen Per Type")]
    [Export] public float HeartScale = 0.18f;
    [Export] public float LungScale = 0.2f;
    [Export] public float BloodScale = 0.2f;
    [Export] public float DrugScale = 0.01f;

    private Node3D _spawnPoint;

    public override void _Ready()
    {
        GD.Print("SPAWNER DEBUG: _Ready is gestart");

        _spawnPoint = GetNodeOrNull<Node3D>("spawn");

        if (_spawnPoint == null)
        {
            GD.PrintErr("SPAWNER DEBUG FOUT: Kind-node 'spawn' niet gevonden!");
            return;
        }

        GD.Print("SPAWNER DEBUG: Spawnpoint gevonden, start SpawnLogic...");
        SpawnLogic();
    }

    public void SpawnLogic()
    {
        GD.Print("SPAWNER DEBUG: SpawnLogic wordt uitgevoerd!");

        int objectChoice = _random.Next(1, 5);
        PackedScene selectedScene = null;
        float chosenScale = 0.2f;

        switch (objectChoice)
        {
            case 1: selectedScene = HeartScene; chosenScale = HeartScale; break;
            case 2: selectedScene = LungScene; chosenScale = LungScale; break;
            case 3: selectedScene = BloodSampleScene; chosenScale = BloodScale; break;
            case 4: selectedScene = DrugSampleScene; chosenScale = DrugScale; break;
        }

        if (selectedScene == null)
        {
            GD.PrintErr("SPAWNER DEBUG FOUT: Scène niet toegewezen!");
            return;
        }

        int statusChoice = _random.Next(0, 3);
        string finalStatus = "Goed";
        if (statusChoice == 1) finalStatus = "Slecht";
        else if (statusChoice == 2) finalStatus = "Geinfecteerd";

        var instance = selectedScene.Instantiate<RigidBody3D>();
        AddChild(instance);

        Transform3D t = instance.GlobalTransform;
        t.Origin = _spawnPoint.GlobalPosition;
        t.Basis = Basis.FromScale(new Vector3(chosenScale, chosenScale, chosenScale));
        instance.GlobalTransform = t;

        // GEBRUIK METADATA IN PLAATS VAN SET
        // Dit "plakt" het label direct op de node op een manier die de scanner ALTIJD kan lezen
        instance.SetMeta("IsGoed", finalStatus);

        GD.Print($"SPAWNER SUCCES: {instance.Name} gespawned met status: {finalStatus}");

        if (finalStatus == "Geinfecteerd")
        {
            ApplyVisualInfection(instance, chosenScale);
        }
    }

    private void ApplyVisualInfection(Node3D parent, float currentScale)
    {
        var meshInstance = parent.GetNodeOrNull<MeshInstance3D>("MeshInstance3D");
        if (meshInstance == null) return;

        int dots = _random.Next(12, 18);
        Aabb box = meshInstance.GetAabb();
        StandardMaterial3D dotMat = new StandardMaterial3D { AlbedoColor = Colors.Black };

        for (int i = 0; i < dots; i++)
        {
            MeshInstance3D dot = new MeshInstance3D();
            float s = 0.04f * currentScale; 
            dot.Mesh = new SphereMesh { Radius = s, Height = s * 2 };
            dot.MaterialOverride = dotMat;
            dot.Position = new Vector3(
                (float)(_random.NextDouble() - 0.5) * box.Size.X + (box.Position.X + box.Size.X / 2),
                (float)(_random.NextDouble() - 0.5) * box.Size.Y + (box.Position.Y + box.Size.Y / 2),
                (float)(_random.NextDouble() - 0.5) * box.Size.Z + (box.Position.Z + box.Size.Z / 2)
            );
            parent.AddChild(dot);
        }
    }
}