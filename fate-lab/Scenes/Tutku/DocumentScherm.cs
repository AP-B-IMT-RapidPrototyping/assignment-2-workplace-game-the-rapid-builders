
using Godot;
using System;
public partial class DocumentScherm : Control
{
    [Export] public Label NaamLabel, IdLabel, InfoLabel, ResultaatLabel;
    [Export] public RichTextLabel TaakLabel; // Sleep hier je InfoLabel2 in
    [Export] public TextureRect FotoRect;
    [Export] public Button AccepteerBtn, AfwijsBtn, SluitBtn;
    
    public PapierInteractie OuderScript;
    
    public override void _Ready() => Hide();

    public void OpenDocument(string n, string id, string i, Texture2D f, bool getest, bool gelukt, string taak, PapierInteractie bron)
    {
        OuderScript = bron;
        NaamLabel.Text = "Naam: " + n;
        IdLabel.Text = "ID: " + id;
        InfoLabel.Text = i;
        FotoRect.Texture = f;
        
        if (!getest)
        {
            ResultaatLabel.Text = "Status: nog niet getest.";
            TaakLabel.Text = "Test: " + taak;
            AccepteerBtn.Show();
            AfwijsBtn.Show();
        }
        else
        {
            ResultaatLabel.Text = gelukt ? "Status: VOLTOOID" : "Status: MISLUKT";
            TaakLabel.Text = "AFGEROND";
            AccepteerBtn.Hide();
            AfwijsBtn.Hide();
        }
        Show();
        // MAAK DE MUIS ZICHTBAAR
    Input.MouseMode = Input.MouseModeEnum.Visible;
    
    }
// Voeg dit toe aan AL je sluit/knop functies:
private void SluitScherm()
{
    Hide();
    // VERBERG DE MUIS WEER VOOR HET LOPEN
    Input.MouseMode = Input.MouseModeEnum.Captured; 
}
// Gebruik exact deze namen omdat je signalen hiernaar zoeken
public void _on_sluit_knop_pressed() { SluitScherm(); }
public void _on_accepteer_button_pressed() 
{ 
    OuderScript.ZetToestemming(true); 
    UpdateMonitorTekst("GEACCEPTEERD"); // VOEG DIT TOE
    SluitScherm(); 
}

public void _on_afwijs_button_pressed() 
{ 
    OuderScript.ZetToestemming(false); 
    UpdateMonitorTekst("GEWEIGERD"); // VOEG DIT TOE
    SluitScherm(); 
}

// Deze nieuwe functie zoekt de monitor en plakt de tekst erop
private void UpdateMonitorTekst(string status)
{
    // Zorg dat de naam "document_scherm" EXACT zo in je scene tree staat (met kleine letters!)
var monitorDossier = GetTree().Root.FindChild("document_scherm", true, false) as DocumentScherm;
    // We zoeken nu de Sprite3D in plaats van het document_scherm
    var sticker = GetTree().Root.FindChild("Sprite3D", true, false) as Sprite3D;

    if (sticker != null)
    {
        sticker.Visible = true;
        // Modulate op een Sprite3D kleurt de hele afbeelding
        sticker.Modulate = (status == "GEACCEPTEERD") ? Colors.Green : Colors.Red;
        GD.Print("Monitor Sticker gekleurd!");
    }
}
}
