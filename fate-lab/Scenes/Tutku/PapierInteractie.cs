using Godot;
using System;
using System.Collections.Generic;

// Dit moet BOVENAAN staan, buiten de andere class
public class PatientData
{
    public string Naam, ID, Info, WelkeTest;
    public Texture2D Foto;
    public bool IsGetest = false;
    public bool TestGelukt = false;

    public PatientData(string n, string id, string inf, Texture2D f, string t)
    {
        Naam = n; ID = id; Info = inf; Foto = f; WelkeTest = t;
    }
}

public partial class PapierInteractie : Area3D
{
    [Export] public DocumentScherm MijnDocumentUI;
    [Export] public Texture2D Foto1, Foto2;
    [Export] public string InteractieActie = "interactie-bacterie"; 

    private List<PatientData> _lijst = new List<PatientData>();
    private int _index = 0;
    private bool _testGeaccepteerd = false;
    private bool _spelerInBuurt = false;

    public override void _Ready()
    {
        _lijst.Add(new PatientData("Elias Thorne", "PX-9921", "Bacteriële infectie.", Foto1, "Microscoop"));
        _lijst.Add(new PatientData("Silas Vane", "PX-4005", "DNA onderzoek.", Foto2, "DNA-Test"));

        this.BodyEntered += (body) => { if (body.IsInGroup("Player")) _spelerInBuurt = true; };
        this.BodyExited += (body) => { if (body.IsInGroup("Player")) _spelerInBuurt = false; };

        // Zoek de HUD zodra je terugkomt in het Lab
    var hud = GetTree().Root.FindChild("MeldingUI", true, false) as HUD;
    if (hud != null)
    {
        // De melding die je nu wilt zien:
        hud.ToonMelding("Test verwerkt! Open dossier voor resultaat.");
    }
    }

    public override void _Process(double delta)
    {
        if (_spelerInBuurt && Input.IsActionJustPressed(InteractieActie))
        {
            LaatVolgendePatientZien();
        }
    }

    public void LaatVolgendePatientZien()
    {
        if (MijnDocumentUI == null) return;
        var p = _lijst[_index];
        _testGeaccepteerd = false; 
        MijnDocumentUI.OpenDocument(p.Naam, p.ID, p.Info, p.Foto, p.IsGetest, p.TestGelukt, p.WelkeTest, this);
        _index = (_index + 1) % _lijst.Count;
    }

    public void UpdateStatus(bool succes)
    {
        int i = _index - 1; 
        if (i < 0) i = _lijst.Count - 1;
        _lijst[i].IsGetest = true;
        _lijst[i].TestGelukt = succes;
    }

    public void ZetToestemming(bool status) => _testGeaccepteerd = status;
    public bool MagMinigameStarten() => _testGeaccepteerd;
}