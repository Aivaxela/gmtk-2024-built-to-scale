using Godot;

public partial class Ship : CharacterBody2D
{
    [Export] Area2D gravityCheckArea;
    [Export] Sprite2D dirPointer;
    [Export] Sprite2D bodyPointer;

    float speed = 200;
    Vector2 velocity = new Vector2(100, 0);
    Vector2 direction;

    Planet planetNear = null;
    Comet cometNear = null;


    public override void _Ready()
    {
        gravityCheckArea.AreaEntered += OnGravityAreaEntered;
        gravityCheckArea.AreaExited += OnGravityAreaExited;
    }

    public override void _PhysicsProcess(double delta)
    {
        UpdateDirection();
        CalculateVelocity();
        UpdateHelpers();

        if (Input.IsActionJustPressed("reset")) Reset();
    }

    private void UpdateDirection()
    {

        if (planetNear != null)
        {
            Vector2 dirToPlanet = planetNear.GlobalPosition - GlobalPosition;
            direction = dirToPlanet.Normalized();
        }
        else if (cometNear != null)
        {
            Vector2 dirToComet = cometNear.GlobalPosition - GlobalPosition;
            direction = dirToComet.Normalized();
        }
        else
        {
            direction = velocity;
        }
    }

    private void CalculateVelocity()
    {
        velocity = velocity.Slerp(direction, 0.009f);
        velocity = velocity.Normalized() * speed;

        Velocity = velocity;
        MoveAndSlide();
        Rotation = velocity.Angle();
        dirPointer.RotationDegrees = velocity.Angle();
    }

    private void OnGravityAreaEntered(Area2D area)
    {
        if (area.GetParent() is Planet) planetNear = (Planet)area.GetParent();
        if (area.GetParent() is Comet) cometNear = (Comet)area.GetParent();
    }

    private void OnGravityAreaExited(Area2D area)
    {
        planetNear = null;
        cometNear = null;
    }

    public void Reset()
    {
        GlobalPosition = new Vector2(-243, 89);
        velocity = new Vector2(150, 0);
        RotationDegrees = 0;
    }

    private void UpdateHelpers()
    {
        if (planetNear != null)
        {
            bodyPointer.Visible = true;
            bodyPointer.Rotation = GetAngleTo(planetNear.GlobalPosition);
        }
        else if (cometNear != null)
        {
            bodyPointer.Visible = true;
            bodyPointer.Rotation = GetAngleTo(cometNear.GlobalPosition);
        }
        else
        {
            bodyPointer.Visible = false;
        }
    }
}
