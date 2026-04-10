using Godot;
using System;

public partial class TutBacterie : Node3D // Of welk type je minigame ook is
{
    // Hier slaan we de referentie naar de microscoop op
    public InteractionMicroscope DeMicroscoop;

    private int _aantalBacterien = 0;

    public void startSpel()
    {
        _aantalBacterien = 5; 
        GD.Print("Minigame: We starten met 5 bacteriën.");
    }

    public void BacterieGeraakt()
    {
        _aantalBacterien--;
        GD.Print($"Bacterie dood! Nog {_aantalBacterien} over.");

        if (_aantalBacterien <= 0)
        {
            AllesSchoon();
        }
    }

    private void AllesSchoon()
    {
        GD.Print("Minigame: Alles is schoon! We gaan terug...");
        
        if (DeMicroscoop != null)
        {
            DeMicroscoop.StopMiniGame();
        }
        else
        {
            GD.Print("FOUT: DeMicroscoop referentie is leeg!");
        }
    }
}
