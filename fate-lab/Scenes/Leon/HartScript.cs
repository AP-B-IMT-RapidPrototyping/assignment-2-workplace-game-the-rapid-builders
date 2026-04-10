using Godot;
using System;

public partial class HartScript : TextureRect
{
	public double _speed = 1.0;
	private Tween mijnTween;

	[Export] public AudioStreamPlayer HartslagGewoon;
	[Export] public AudioStreamPlayer HartslagSnel;
	[Export] public AudioStreamPlayer HartslagDood;
	public override void _Ready()
	{
		//Hart();
	}

	public void Hart()
	{
		if(mijnTween != null && mijnTween.IsValid())
		{
			mijnTween.Kill();
		}

		HartslagGewoon?.Stop();
		HartslagSnel?.Stop();

		if(_speed > 1.0)
		{
			HartslagGewoon?.Stop();
            if (HartslagSnel != null && !HartslagSnel.Playing) 
            {
                HartslagSnel.Play();
            }
		}
		else
		{
			HartslagSnel?.Stop();
            if (HartslagGewoon != null && !HartslagGewoon.Playing) 
            {
                HartslagGewoon.Play();
            }
		}

		mijnTween = GetTree().CreateTween();
		mijnTween.SetLoops();

		mijnTween.TweenProperty(this, "scale", new Vector2(1.2f, 1.2f), (float)(0.15 / _speed))
			.SetTrans(Tween.TransitionType.Back)
			.SetEase(Tween.EaseType.Out);

		mijnTween.TweenProperty(this, "scale", new Vector2(1f, 1f), (float)(0.3 / _speed))
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.InOut);

		mijnTween.TweenInterval((float)(0.5 / _speed));
	}

	public void StopHart()
    {
		GD.Print("StopHart functie wordt nu uitgevoerd in het script.");
        if (mijnTween != null && mijnTween.IsValid())
        {
            mijnTween.Kill();
			mijnTween = null;
        }

		HartslagGewoon?.Stop();
		HartslagSnel?.Stop();
		HartslagDood?.Play();

        Scale = new Vector2(1f, 1f);
    }

	
}
