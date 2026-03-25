using Godot;
using System;

public partial class OrganSpawning : Node
{
	Random random = new Random();
	[Export] public PackedScene heart;
	[Export] public PackedScene lung;

	[Export] public PackedScene bloodSample;
	[Export] public PackedScene drugSample;
	Node3D spawn;
	Timer timer;

	bool isSick;

	int tick;
	int organ;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		timer = GetNode<Timer>("Timer");
		spawn = GetNode<Node3D>("spawn");
		SpawnSample(bloodSample);
		timer.Timeout += Ontick;
		timer.Start();
		
		

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public void OrganSelector()
	{
		if(organ == 1)
		{
			spawnOrgan(heart);
		}
		else if(organ == 2)
		{
			spawnOrgan(lung);
		}
		else if(organ == 3)
		{
			SpawnSample(bloodSample);
		}
		else if (organ == 4)
		{
			SpawnSample(drugSample);
		}
		
		GD.Print("organ has spawned!!!");
	}

	public void Ontick()
	{
		organ = random.Next(1, 5);
		isSick = Convert.ToBoolean(random.Next(0, 2));
		OrganSelector();
		
	}

	public void spawnOrgan(PackedScene organ)
	{
		var instance = organ.Instantiate<RigidBody3D>();
		instance.Position = spawn.Position;
		AddChild(instance);
		if(isSick == true)
		{
			GD.Print("organ is infected!!!");
			for(int i = 0; i <= random.Next(5, 15); i++)
			{
				CreateDot(instance);
			}
		}
	}

	public void CreateDot(Node3D parent)
	{
		MeshInstance3D dot = new MeshInstance3D();
		dot.Mesh = new SphereMesh{ Radius = 0.05f, Height = 0.1f };

		//material toevoegen
		StandardMaterial3D matte = new StandardMaterial3D();
		matte.AlbedoColor = Colors.Black;

		dot.MaterialOverride = matte;


		//locatie krijgen van de mesh
		var meshInstance = parent.GetNode<MeshInstance3D>("MeshInstance3D");

		//bounding box nemen van het orgaan
		Aabb box = meshInstance.GetAabb();
		Vector3 size = box.Size;
		Vector3 center = box.Position + (size / 2);

		//willekeurige plaats geven op het orgaan
		RandomNumberGenerator rng = new RandomNumberGenerator();
        rng.Randomize();

    	float x = rng.RandfRange(-size.X / 2, size.X /2) + center.X;
        float y = rng.RandfRange(-size.Y / 2, size.Y /2) + center.Y;
        float z = rng.RandfRange(-size.Z / 2, size.Z /2) + center.Z;

		dot.Position = new Vector3(x, y, z);
		parent.AddChild(dot);
		
	}

	public void SpawnSample(PackedScene sample)
	{
		var instance = sample.Instantiate<RigidBody3D>();
		instance.Position = spawn.Position;
		AddChild(instance);
	}

}
