using Godot;
using System;

public partial class ProgressBarAftikken : ProgressBar
{
	[Export] public double MaxTijd = 120.0; //Tijd in seconde
	[Export] public HartScript mijnHart;
    [Export] public Label GameOverTekst;
	private bool _HartSnel = false;
    private bool _isDood = false;
	private double _Tijdnu;

	public override void _Ready()
	{
		MaxValue = MaxTijd;
		Value = MaxTijd;
		_Tijdnu = MaxTijd;

        GameOverTekst.Visible = false;
	}

	public override void _Process(double delta)
	{
        if (_Tijdnu > 0)
        {
            _Tijdnu -= delta;
            Value = _Tijdnu;

            // PANIEK: Onder de 20%
            //Hart klopt harder
            if (Value < MaxValue * 0.2 && !_HartSnel)
            {
                GD.Print("NU MOET HET HART SNEL GAAN!");
                _HartSnel = true;
                if (mijnHart != null) 
                {
                    mijnHart._speed = 6.0;
                    mijnHart.Hart(); 
                }
            }
        }
        // DOOD: Op of onder 0%
        else 
        {
            if (!_isDood)
            {
                _isDood = true; 
                _Tijdnu = 0;
                Value = 0;
                mijnHart.StopHart();
                
                if (mijnHart != null) 
                {
                    GameOverTekst.Visible = true;
                    GD.Print("Het hart is gestopt.");
                }
            }
        }
    }
}
