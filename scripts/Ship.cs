using Godot;

public partial class Ship : CharacterBody2D
{
    [Export] Area2D gravityCheckArea;
    [Export] Label speedLabel;
    [Export] Label totalVelLabel;
    [Export] Label dirLabel;
    [Export] Label planetPowerLabel;
    [Export] Sprite2D dirPointer;
    [Export] Sprite2D planetPointer;

    public float planetPower = 1;
    float speed = 200;

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
        UpdateDirection();

        velocity = velocity.Slerp(direction, 0.009f);
        velocity = velocity.Normalized() * speed;

        Velocity = velocity;
        MoveAndSlide();
        Rotation = velocity.Angle();
        dirPointer.RotationDegrees = velocity.Angle();

        UpdateHelpers();
        CheckForReset();
    }

    private void UpdateDirection()
    {

        if (planetNear != null)
        {
            Vector2 dirToPlanet = planetNear.GlobalPosition - GlobalPosition;
            direction = dirToPlanet.Normalized();
        }
        else
        {
            direction = velocity;
        }
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

    private void CheckForReset()
    {
        if (Input.IsActionJustPressed("reset"))
        {
            GlobalPosition = new Vector2(-243, 89);
            velocity = new Vector2(150, 0);
            RotationDegrees = 0;
        }
    }

    private void UpdateHelpers()
    {
        if (planetNear != null)
        {
            planetPointer.Visible = true;
            planetPointer.Rotation = GetAngleTo(planetNear.GlobalPosition);
        }
        else
        {
            planetPointer.Visible = false;
        }

        speedLabel.Text = Velocity.ToString();
        totalVelLabel.Text = (Mathf.Abs(Velocity.X) + Mathf.Abs(Velocity.Y)).ToString();
        dirLabel.Text = direction.ToString();
        planetPowerLabel.Text = planetPower.ToString();
    }
}
