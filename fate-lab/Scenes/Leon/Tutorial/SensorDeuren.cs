using Godot;
using System;

public partial class SensorDeuren : Node3D
{
    // We maken variabelen voor beide AnimationPlayers
    private AnimationPlayer _animLinks;
    private AnimationPlayer _animRechts;
	private int _actie1 = 0;
	private int _actie2 = 0;

    public override void _Ready()
    {
        // We zoeken de AnimationPlayers in de juiste child nodes
        _animLinks = GetNode<AnimationPlayer>("Deurlinks/AnimationPlayer");
        _animRechts = GetNode<AnimationPlayer>("DeurRechts/AnimationPlayer");
    }

    // Verbind dit signaal met de Area3D (body_entered)
    private void OnArea3DBodyEntered(Node3D body)
    {
        // Controleer of de speler de area binnenloopt
		if(_actie1 == 0)
		{
			if (body.IsInGroup("Player"))
			{
				_animLinks.Play("animatie-links");
				_animRechts.Play("animatie-rechts");
				_actie1++;
			}
		}
		else
		{
			GD.Print("actie moet niet meer gebeuren");
		}
        
    }

    // Verbind dit signaal met de Area3D (body_exited)
    private void OnArea3DBodyExited(Node3D body)
    {
		if(_actie2 == 0)
		{
			if (body.IsInGroup("Player"))
			{
				// Speelt de animaties weer terug naar dicht
				_animLinks.PlayBackwards("animatie-links");
				_animRechts.PlayBackwards("animatie-rechts");
				_actie2++;
			}
		}
		else
		{
			GD.Print("actie moet niet meer gebeuren");
		}
    }
}