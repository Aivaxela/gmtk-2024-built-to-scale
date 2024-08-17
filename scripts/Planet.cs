using System;
using System.Transactions;
using Godot;

public partial class Planet : Node2D
{
    [Export] Area2D gravityArea;
    [Export] Label planetPowerLabel;
    [Export] Sprite2D sprite;
    [Export] CollisionShape2D gravityAreaShape;

    [Export] float pullStrength;

    Vector2 pullDirection;
    Ship ship;


    public override void _Ready()
    {
        ship = GetNode<Ship>("/root/main/ship");
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionPressed("scale-up") && ship.planetPower < 5) ship.planetPower += 0.001f;
        if (Input.IsActionPressed("scale-down") && ship.planetPower > 1) ship.planetPower -= 0.001f;

        planetPowerLabel.Text = ship.planetPower.ToString();

        sprite.Scale = new Vector2(ship.planetPower, ship.planetPower);
        gravityArea.Scale = new Vector2(ship.planetPower / 3f, ship.planetPower / 3f);
    }


    public float PullShip()
    {
        return pullStrength;
    }
}
