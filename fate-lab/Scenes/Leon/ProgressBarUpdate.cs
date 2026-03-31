using Godot;
using System;

public partial class ProgressBarUpdate : ProgressBar
{
    [Export] public double SchadePerFout = 1.0; // Hoeveel er af gaat per fout
    [Export] public double StartGezondheid = 15.0; 
    [Export] public HartScript mijnHart;
    [Export] public Label GameOverTekst;

    private bool _HartSnel = false;
    private bool _isDood = false;
    private double _Tijdnu;

    public override void _Ready()
    {
        MaxValue = StartGezondheid;
        Value = StartGezondheid;
        _Tijdnu = StartGezondheid;

        if (GameOverTekst != null) GameOverTekst.Visible = false;
    }

    // De _Process is nu bijna leeg omdat we niet meer automatisch aftellen
    public override void _Process(double delta)
    {
        // We laten deze leeg of halen hem weg, 
        // tenzij je nog andere visuele updates wilt doen.
    }

    // DEZE FUNCTIE ROEP JE AAN BIJ EEN FOUT
    public void DoeSchade()
    {
        if (_isDood) return;

        _Tijdnu -= SchadePerFout;
        Value = _Tijdnu;

        GD.Print($"Fout gemaakt! Gezondheid nu: {_Tijdnu}");

        // PANIEK: Onder de 20%
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

        // DOOD: Op of onder 0
        if (_Tijdnu <= 0)
        {
            GaDood();
        }
    }

    public void GaDood()
    {
        _isDood = true; 
        _Tijdnu = 0;
        Value = 0;
        
        if (mijnHart != null) mijnHart.StopHart();
        if (GameOverTekst != null) GameOverTekst.Visible = true;
        
        GD.Print("Het hart is gestopt.");
    }
}