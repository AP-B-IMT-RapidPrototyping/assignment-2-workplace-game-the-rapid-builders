using Godot;
using System;

public partial class TutLungs : RigidBody3D
{
	[Export] public string IsGoed; 

    public override void _Ready()
    {
        IsGoed = "Slecht"; 
    }
}
