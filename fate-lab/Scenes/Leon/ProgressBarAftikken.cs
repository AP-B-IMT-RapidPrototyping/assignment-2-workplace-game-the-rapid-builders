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
    private bool _isGestart = false;

	public override void _Ready()
	{
        ResetTimer();
	}

    public void StartTimer()
    {
        GD.Print("UI: Timer en Hartslag worden nu gestart!");
        _isGestart = true;
        SetProcess(true); 
        
        mijnHart?.Hart(); 
    }

    public void ResetTimer()
    {
        _isGestart = false;
        SetProcess(false);
        
        _Tijdnu = MaxTijd;
        MaxValue = MaxTijd;
        Value = MaxTijd;
        
        _HartSnel = false;
        _isDood = false;

        if (GameOverTekst != null) 
            GameOverTekst.Visible = false;

        if (mijnHart != null)
        {
            mijnHart._speed = 1.0; 
            mijnHart.Scale = new Vector2(1f, 1f);
            mijnHart.HartslagGewoon?.Stop();
            mijnHart.HartslagSnel?.Stop(); 
        }

        GD.Print("UI: Timer volledig gereset naar beginstand.");
    }

    public void StopTimer()
    {
        _isGestart = false;
        SetProcess(false); 
        GD.Print("UI: Gefeliciteerd! Alle organen ingeleverd. Tijd gestopt.");
    }

	public override void _Process(double delta)
	{
        if (!_isGestart) return;
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
                GameOverTekst.Visible = true;
                GD.Print("Het hart is gestopt.");
            }
        }
    }
}
