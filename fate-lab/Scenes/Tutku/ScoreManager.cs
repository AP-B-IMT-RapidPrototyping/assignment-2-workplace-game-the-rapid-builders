using Godot;
using System;

public partial class ScoreManager : Label
{
	private int _huidigeScore = 0;

    public override void _Ready()
    {
        UpdateScoreTekst();
    }

    public void VoegPuntenToe(int punten)
    {
        _huidigeScore += punten;
        
        // Optioneel: Voorkom dat de score onder 0 gaat
        _huidigeScore = Math.Max(0, _huidigeScore);
        
        UpdateScoreTekst();
    }

    private void UpdateScoreTekst()
    {
        Text = $"Score: {_huidigeScore}";
    }
}
