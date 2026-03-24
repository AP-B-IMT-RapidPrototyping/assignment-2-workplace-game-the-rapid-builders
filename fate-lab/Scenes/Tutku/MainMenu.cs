using Godot;
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
}