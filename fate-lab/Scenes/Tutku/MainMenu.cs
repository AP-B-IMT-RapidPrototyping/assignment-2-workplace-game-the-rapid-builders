using Godot;
using System;

public partial class MainMenu : Control
{
    // Het pad naar je lab scene
    [Export]
    private string _labScenePath = "res://Scenes/Tutku/LAB.tscn"; 

    public override void _Ready()
    {
        // We zoeken de knoppen op basis van de namen in jouw screenshot
        // Button = Nieuw Spel, Button2 = Start, Button5 = Afsluiten
        Button nieuwSpelKnop = GetNode<Button>("VBoxContainer/Button");
        Button startKnop = GetNode<Button>("VBoxContainer/Button2");
        Button afsluitKnop = GetNode<Button>("VBoxContainer/Button5");

        // Acties koppelen aan de knoppen
        nieuwSpelKnop.Pressed += OnStartGamePressed;
        startKnop.Pressed += OnStartGamePressed;
        afsluitKnop.Pressed += OnQuitPressed;
    }

    private void OnStartGamePressed()
    {
        GD.Print("Spel wordt gestart...");
        // Wissel naar de lab scene
        Error err = GetTree().ChangeSceneToFile(_labScenePath);
        
        if (err != Error.Ok)
        {
            GD.PrintErr("FOUT: Kan scene niet laden op pad: " + _labScenePath);
        }
    }

    private void OnQuitPressed()
    {
        GD.Print("Game afsluiten...");
        GetTree().Quit();
    }
}