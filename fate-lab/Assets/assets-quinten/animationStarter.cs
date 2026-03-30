using Godot;
using System;

public partial class animationStarter : RigidBody3D
{
	// Called when the node enters the scene tree for the first time.
	[Export] AnimationPlayer animatie;
	public override void _Ready()
	{
		animatie.Play();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
