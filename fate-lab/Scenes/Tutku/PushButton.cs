using Godot;
using System;

public partial class PushButton : StaticBody3D
{
    [Signal] public delegate void ButtonPressedEventHandler();

    [Export] public RayCast3D PlayerRay; // Hier sleep je de RayCast van de player in
    [Export] public string InteractAction = "interact"; // De actie die je al in je script hebt

    private AnimationPlayer _anim;
    public bool IsPressed = false;

    public override void _Ready()
    {
        _anim = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public override void _Process(double delta)
    {
        // De knop checkt zelf of de speler naar hem kijkt
        if (PlayerRay != null && PlayerRay.IsColliding())
        {
            if (PlayerRay.GetCollider() == this)
            {
                // Als de speler naar de knop kijkt en op 'interact' drukt
                if (Input.IsActionJustPressed("interactie-bacterie")) 
				{
					Press();
				}
            }
        }
    }

    public void Press()
    {
        if (IsPressed) 
		{
			GD.Print("KNOP: Je hebt al gedrukt, wacht tot de ronde klaar is!");
			return;
		}

		IsPressed = true;
        GD.Print("KNOP: Ingedrukt");

        if (_anim != null && _anim.HasAnimation("PushButton"))
        {
            _anim.Play("PushButton");
        }

		var spawner = GetTree().Root.FindChild("OrgaanSpawner", true, false);
		if (spawner != null)
		{
			spawner.Call("SpawnRij");
		}

		var timerUI = GetTree().Root.FindChild("ProgressBar", true, false);
		if (timerUI != null)
		{
			// We roepen een functie aan in je timer script, bijvoorbeeld "StartTimer"
			timerUI.Call("StartTimer"); 
		}

        EmitSignal(SignalName.ButtonPressed);

        // Reset de druk-beveiliging na 0.5 sec
        //GetTree().CreateTimer(0.5f).Timeout += () => IsPressed = false;
    }
}