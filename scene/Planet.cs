using System;
using Godot;

public partial class Planet : Sprite2D
{
    [Export] Area2D gravityArea;
    [Export] float pullStrength;

    Vector2 pullDirection;
    Ship ship;


    public override void _Ready()
    {
        ship = GetNode<Ship>("/root/main/ship");
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionPressed("scale-up") && ship.planetPower < 2000) ship.planetPower += 1;
        if (Input.IsActionPressed("scale-down") && ship.planetPower > 0) ship.planetPower -= 1;
    }


    public float PullShip()
    {
        return pullStrength;
    }
}
