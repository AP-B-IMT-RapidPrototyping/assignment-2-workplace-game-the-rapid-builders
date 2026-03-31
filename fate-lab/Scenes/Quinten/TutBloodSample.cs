using Godot;
using System;

public partial class TutBloodSample : RigidBody3D
{
	[Export] public string IsGoed; 

    public override void _Ready()
    {
        IsGoed = "Goed"; 
    }
}
