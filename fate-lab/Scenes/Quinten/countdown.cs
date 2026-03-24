using Godot;
using System;

public partial class countdown : Camera3D
{
	private Timer timer;
	private Label label;
	[Export] public int time_left;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		timer = GetNode<Timer>("Timer");
		label = GetNode<Label>("Label");

		// Toon de starttijd
		label.Text = time_left.ToString() + "s";

		// Verbind het timeout signaal via code (of via de editor)
		timer.Timeout += OnTimerTimeout;
		timer.Start(); // Start de timer
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public void OnTimerTimeout()
	{
		if(time_left > 0)
		{
			time_left--;
			label.Text = time_left.ToString() + "s";

		}
		else
		{
			GetTree().ChangeSceneToFile("res://Scenes/Quinten/failed.tscn");
		}
	}
}
