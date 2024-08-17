using System;
using Godot;

public partial class Ship : CharacterBody2D
{
    [Export] Area2D gravityCheckArea;
    [Export] Label speedLabel;
    [Export] Label totalVelLabel;
    [Export] Label dirLabel;
    [Export] Label planetPowerLabel;

    public float planetPower = 0;

    Vector2 velocity = new Vector2(150, 0);
    Vector2 direction;

    Planet planetNear = null;


    public override void _Ready()
    {
        gravityCheckArea.AreaEntered += OnGravityAreaEntered;
        gravityCheckArea.AreaExited += OnGravityAreaExited;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (planetNear != null)
        {
            Vector2 dirToPlanet = planetNear.GlobalPosition - GlobalPosition;
            direction = dirToPlanet.Normalized() * planetPower;
        }
        else
        {
            direction = velocity;
        }

        velocity = velocity.Lerp(direction, 0.001f);
        Velocity = velocity;
        MoveAndSlide();
        Rotation = velocity.Angle();

        speedLabel.Text = Velocity.ToString();
        totalVelLabel.Text = (Velocity.X + Velocity.Y).ToString();
        dirLabel.Text = direction.ToString();
        planetPowerLabel.Text = planetPower.ToString();
    }

    private void OnGravityAreaEntered(Area2D area)
    {
        if (area.GetParent() is not Planet) return;
        planetNear = (Planet)area.GetParent();
    }

    private void OnGravityAreaExited(Area2D area)
    {
        if (area.GetParent() is not Planet) return;
        planetNear = null;
    }
}
