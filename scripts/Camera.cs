using Godot;

public partial class Camera : Camera2D
{
	[Export] CharacterBody2D ship;


	public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{
		GlobalPosition = ship.GlobalPosition;
	}
}
