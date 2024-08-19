using Godot;

public partial class MercuryToken : CharacterBody2D
{
    [Export] public Vector2 velocity = Vector2.Zero;
    [Export] Area2D gravityCheckArea;
    [Export] float speed = 100;
    Vector2 direction;

    Planet planetNear = null;

    public override void _Ready()
    {
        velocity = new Vector2(20, 0);

        gravityCheckArea.AreaEntered += OnGravityAreaEntered;
        gravityCheckArea.AreaExited += OnGravityAreaExited;
    }

    public override void _PhysicsProcess(double delta)
    {
        UpdateDirection();
        CalculateVelocity();
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
            speed = Mathf.Lerp(speed, 100f, 0.01f);
        }
    }

    private void CalculateVelocity()
    {
        velocity = velocity.Slerp(direction, 0.008f);
        velocity = velocity.Normalized() * speed;

        Velocity = velocity;
        MoveAndSlide();
        Rotation = velocity.Angle();
    }

    private void OnGravityAreaEntered(Area2D area)
    {
        if (area.GetParent() is Planet) planetNear = (Planet)area.GetParent();
    }

    private void OnGravityAreaExited(Area2D area)
    {
        planetNear = null;
    }
}
