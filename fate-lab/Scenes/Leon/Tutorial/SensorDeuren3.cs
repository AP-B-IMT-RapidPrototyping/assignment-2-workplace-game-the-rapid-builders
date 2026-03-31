using Godot;
using System;

public partial class SensorDeuren3 : Node3D
{
	private AnimationPlayer _animLinks;
    private AnimationPlayer _animRechts;
	private int _actie1 = 0;
	private int _actie2 = 0; 
	public override void _Ready()
	{
		_animLinks = GetNode<AnimationPlayer>("Deurlinks/AnimationPlayer");
        _animRechts = GetNode<AnimationPlayer>("DeurRechts/AnimationPlayer");
	}

	private void OnArea3DBodyEntered(Node3D body)
    {
        // Controleer of de speler de area binnenloopt
		if(_actie1 == 0)
		{
			if (body.IsInGroup("Player"))
			{
				_animLinks.Play("animatielinks3");
				_animRechts.Play("animatierechts3");
				_actie1++;
			}
		}
		else
		{
			GD.Print("actie moet niet meer gebeuren");
		}
        
    }

	private void OnArea3DBodyExited(Node3D body)
    {
		if(_actie2 == 0)
		{
			if (body.IsInGroup("Player"))
			{
				// Speelt de animaties weer terug naar dicht
				_animLinks.PlayBackwards("animatielinks3");
				_animRechts.PlayBackwards("animatierechts3");
				_actie2++;
			}
		}
		else
		{
			GD.Print("actie moet niet meer gebeuren");
		}
    }
}
