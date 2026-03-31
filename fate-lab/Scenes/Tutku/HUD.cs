using Godot;
using System;

public partial class HUD : CanvasLayer
{
    [Export] public PanelContainer MeldingBox;
    [Export] public Label MeldingTekst;

 public override void _Ready()
{
    if (MeldingBox != null) MeldingBox.Hide();
    // VERWIJDER de regel ToonMelding("Systeem Online");
}
    public async void ToonMelding(string bericht, float duur = 3.0f)
    {
        if (MeldingTekst != null) MeldingTekst.Text = bericht;
        if (MeldingBox != null) MeldingBox.Show();
        
        // Nu staat hier de juiste code: GetTree()
        await ToSignal(GetTree().CreateTimer(duur), "timeout");
        
        if (MeldingBox != null) MeldingBox.Hide();
    }
}