using Godot;
using System;

public partial class MainMenu : Control
{
    [Export]
    private string _labScenePath = "res://Scenes/Tutku/LAB.tscn"; 
    
    // 1. Voeg het pad naar je tutorial toe (pas aan indien nodig)
    [Export]
    private string _tutorialScenePath = "res://Scenes/Tutorial.tscn"; 

    private AnimationPlayer _animPlayer;
    private string _targetScene; // Om bij te houden naar welke scene we gaan na de fade-out

    public override void _Ready()
    {
        GD.Print("Menu is geladen!");

        _animPlayer = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");

        if (_animPlayer == null)
        {
            GD.PrintErr("FOUT: AnimationPlayer niet gevonden!");
        }
        else if (_animPlayer.HasAnimation("fade_in"))
        {
            _animPlayer.Play("fade_in");
        }

        // Knoppen ophalen
        Button startKnop = GetNodeOrNull<Button>("VBoxContainer/Button2");
        Button tutorialKnop = GetNodeOrNull<Button>("VBoxContainer/Button3"); // De tutorial knop
        Button afsluitKnop = GetNodeOrNull<Button>("VBoxContainer/Button5");

        // Koppelen aan functies
        if (startKnop != null) startKnop.Pressed += () => StartFadeTransition(_labScenePath);
        if (tutorialKnop != null) tutorialKnop.Pressed += () => StartFadeTransition(_tutorialScenePath);
        if (afsluitKnop != null) afsluitKnop.Pressed += OnQuitPressed;
    }

    // Een universele functie voor de transitie
    private void StartFadeTransition(string scenePath)
    {
        _targetScene = scenePath;
        GD.Print($"Transitie naar {scenePath} start...");

        if (_animPlayer != null && _animPlayer.HasAnimation("fade_out"))
        {
            _animPlayer.Play("fade_out");

            // Eenmalige event handler (lambda) om te switchen na de animatie
            _animPlayer.AnimationFinished += OnFadeOutFinished;
        }
        else
        {
            ChangeScene(_targetScene);
        }
    }

    private void OnFadeOutFinished(StringName animName)
    {
        if (animName == "fade_out")
        {
            // Ontkoppel de event handler om dubbele calls in de toekomst te voorkomen
            _animPlayer.AnimationFinished -= OnFadeOutFinished;
            ChangeScene(_targetScene);
        }
    }

    private void ChangeScene(string path)
    {
        Error err = GetTree().ChangeSceneToFile(path);
        if (err != Error.Ok)
        {
            GD.PrintErr("FOUT: Kan scene niet laden: " + path);
        }
    }

    private void OnQuitPressed()
    {
        GetTree().Quit();
    }
}


/*using Godot;
using System;

public partial class MainMenu : Control
{
    [Export]
    private string _labScenePath = "res://Scenes/Tutku/LAB.tscn"; 

    private AnimationPlayer _animPlayer;

    public override void _Ready()
    {
        GD.Print("Menu is geladen!");

        _animPlayer = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");

        if (_animPlayer == null)
        {
            GD.PrintErr("FOUT: AnimationPlayer niet gevonden onder CanvasLayer! Check de naam.");
        }
        else
        {
            // Start direct de fade_in zodat het menu mooi verschijnt
            if (_animPlayer.HasAnimation("fade_in"))
            {
                _animPlayer.Play("fade_in");
            }
        }

        // Knoppen koppelen op basis van jouw VBoxContainer structuur
        Button nieuwSpelKnop = GetNodeOrNull<Button>("VBoxContainer/Button");
        Button startKnop = GetNodeOrNull<Button>("VBoxContainer/Button2");
        Button afsluitKnop = GetNodeOrNull<Button>("VBoxContainer/Button5");

        if (nieuwSpelKnop != null) nieuwSpelKnop.Pressed += OnStartGamePressed;
        if (startKnop != null) startKnop.Pressed += OnStartGamePressed;
        if (afsluitKnop != null) afsluitKnop.Pressed += OnQuitPressed;
    }

    private void OnStartGamePressed()
    {
        GD.Print("Start-knop ingedrukt, fade-out start...");

        if (_animPlayer != null && _animPlayer.HasAnimation("fade_out"))
        {
            _animPlayer.Play("fade_out");

            // Wachten tot de animatie klaar is voor we van scene wisselen
            _animPlayer.AnimationFinished += (animName) => {
                if (animName == "fade_out")
                {
                    ChangeToLabScene();
                }
            };
        }
        else
        {
            // Als er geen animatie is, switch dan direct
            ChangeToLabScene();
        }
    }

    private void ChangeToLabScene()
    {
        Error err = GetTree().ChangeSceneToFile(_labScenePath);
        if (err != Error.Ok)
        {
            GD.PrintErr("FOUT: Kan scene niet laden: " + _labScenePath);
        }
    }

    private void OnQuitPressed()
    {
        GetTree().Quit();
    }
}*/