using Godot;

public partial class EscapePod : CharacterBody2D
{
    [Export] Sprite2D bodyPointer;
    [Export] public Vector2 velocity = Vector2.Zero;
    [Export] Sprite2D sprite;
    [Export] Area2D gravityCheckArea;
    [Export] Area2D sunCheckArea;
    [Export] float speed = 100;
    Vector2 direction;
    public bool launched = false;

    Planet planetNear = null;
    Comet cometNear = null;


    public override void _Ready()
    {
        gravityCheckArea.AreaEntered += OnGravityAreaEntered;
        gravityCheckArea.AreaExited += OnGravityAreaExited;
        sunCheckArea.AreaEntered += OnSunEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        UpdateDirection();
        CalculateVelocity();
        UpdateHelpers();
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
            speed = Mathf.Lerp(speed, 100f, 0.01f);
        }
    }

    private void CalculateVelocity()
    {
        velocity = velocity.Slerp(direction, 0.007f);
        velocity = velocity.Normalized() * speed;

        if (!launched) return;

        Velocity = velocity;
        MoveAndSlide();
        Rotation = velocity.Angle();
    }

    private void OnGravityAreaEntered(Area2D area)
    {
        if (area.GetParent().Name == "planet-earth") return;
        if (area.GetParent() is Planet) planetNear = (Planet)area.GetParent();
        if (area.GetParent() is Comet) cometNear = (Comet)area.GetParent();
    }

    private void OnGravityAreaExited(Area2D area)
    {
        planetNear = null;
        cometNear = null;
    }

    private void OnSunEntered(object _)
    {
        QueueFree();
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
