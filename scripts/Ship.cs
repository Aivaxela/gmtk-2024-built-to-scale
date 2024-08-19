using Godot;

public partial class Ship : CharacterBody2D
{
    [Export] Area2D gravityCheckArea;
    [Export] Area2D boundCheckArea;
    [Export] Sprite2D dirPointer;
    [Export] Sprite2D bodyPointer;
    [Export] GpuParticles2D boostParticles;
    [Export] Label fuelLevelReadout;
    [Export] Label infoLabel;
    [Export] public TextureProgressBar fuel;

    float speed = 200;
    float boostCoefficient = 0.01f;
    Vector2 velocity = new Vector2(100, 0);
    Vector2 direction;
    Sun sun;
    public bool boosting = false;
    public int podCounter = 1;

    public Planet planetNear = null;
    Comet cometNear = null;


    public override void _Ready()
    {
        sun = GetNode<Sun>("/root/main/sun");

        gravityCheckArea.AreaEntered += OnGravityAreaEntered;
        gravityCheckArea.AreaExited += OnGravityAreaExited;
        boundCheckArea.AreaEntered += OnBoundaryEntered;

    }

    public override void _PhysicsProcess(double delta)
    {
        UpdateDirection();
        CalculateVelocity();
        UpdateHelpers();

        if (Input.IsActionJustPressed("reset")) PrepReset();

        CheckForBoosting();
    }

    public override void _Process(double delta)
    {
        fuelLevelReadout.Text = fuel.Value.ToString();
        UpdateInfoLabel();
    }


    private void UpdateDirection()
    {
        boostCoefficient = 0f;

        if ((cometNear != null && boosting) || (planetNear != null && boosting))
        {
            Vector2 boostDir = GetGlobalMousePosition() - GlobalPosition;
            direction = boostDir.Normalized();
            boostCoefficient = 0.001f;
            speed = Mathf.Lerp(speed, 100f, 0.01f);
        }
        else if (planetNear != null)
        {
            Vector2 dirToPlanet = planetNear.GlobalPosition - GlobalPosition;
            direction = dirToPlanet.Normalized();
            fuel.Value += 0.5f;
        }
        else if (cometNear != null)
        {
            Vector2 dirToComet = cometNear.GlobalPosition - GlobalPosition;
            direction = dirToComet.Normalized();
        }
        else if (boosting)
        {
            Vector2 boostDir = GetGlobalMousePosition() - GlobalPosition;
            direction = boostDir.Normalized();
            boostCoefficient = 0.01f;
            speed = Mathf.Lerp(speed, 200f, 0.01f);
        }
        else
        {
            direction = velocity;
            speed = Mathf.Lerp(speed, 200f, 0.01f);
        }
    }

    private void CalculateVelocity()
    {
        velocity = velocity.Slerp(direction, 0.017f + boostCoefficient);
        velocity = velocity.Normalized() * speed;

        Velocity = velocity;
        MoveAndSlide();
        Rotation = velocity.Angle();
        dirPointer.RotationDegrees = velocity.Angle();
    }

    private void CheckForBoosting()
    {
        if (fuel.Value <= 0 && false)  //TODO: remove infinite fuel
        {
            boosting = false;
            boostParticles.Emitting = false;
            boostCoefficient = 0f;
            return;
        }
        boosting = Input.IsActionPressed("boost");
        boostParticles.Emitting = Input.IsActionPressed("boost");
        boostCoefficient = Input.IsActionPressed("boost") ? 0.01f : 0f;
        if (boosting) fuel.Value -= 2;
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

    private void OnBoundaryEntered(object _)
    {
        PrepReset();
    }

    public void PrepReset()
    {
        CallDeferred("Reset");
    }

    private void Reset()
    {
        GetTree().ReloadCurrentScene();

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

    private void UpdateInfoLabel()
    {
        infoLabel.Text = "";

        if (planetNear == null) return;
        if (planetNear.Name == "planet-earth" && podCounter <= 3)
        {
            infoLabel.Text = $"Left-click to launch Escape Pod #{podCounter}!";
        }
        else
        {
            infoLabel.Text = "";
        }
    }
}
