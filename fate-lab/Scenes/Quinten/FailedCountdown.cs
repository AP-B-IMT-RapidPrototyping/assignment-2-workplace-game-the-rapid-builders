using Godot;
using System;

public partial class FailedCountdown : Node3D
{
	private Timer timer;
	[Export] public int time_left;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		timer = GetNode<Timer>("Timer");
		timer.Timeout += countdown;
		timer.Start();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void countdown()
	{
		if(time_left > 0)
		{
			time_left--;
		}
		else
		{
			GetTree().ChangeSceneToFile("res://Scenes/Tutku/Lab.tscn");
		}
	}
}
