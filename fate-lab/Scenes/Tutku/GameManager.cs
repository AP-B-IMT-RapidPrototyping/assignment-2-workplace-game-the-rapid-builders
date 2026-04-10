using Godot;
using System;

public partial class GameManager : Node
{
	[Export] public ProgressBarAftikken DeTimerBalk;
	private int _verwerkteObjecten = 0;
	[Export] public int DoelAantal = 5;

	public void ObjectVerwerkt()
	{
		_verwerkteObjecten++;
		GD.Print($"GAME MANAGER: {_verwerkteObjecten} van de 5 verwerkt."); // Dit MOET je nu in de console zien

		if (_verwerkteObjecten >= DoelAantal)
		{
			StopHetSpel();
		}
	}

	private void StopHetSpel()
	{
		if (DeTimerBalk != null)
		{
			DeTimerBalk.ResetTimer();
		}
		else
		{
			var backupTimer = GetTree().Root.FindChild("ProgressBar", true, false);
			backupTimer?.Call("ResetTimer");
		}

		var knopNode = GetTree().Root.FindChild("PushButton", true, false) as PushButton;
		if (knopNode != null)
		{
			knopNode.IsPressed = false;
			GD.Print("MANAGER: Knop is weer ontgrendeld voor de volgende batch.");
		}

		_verwerkteObjecten = 0;
	}
}
