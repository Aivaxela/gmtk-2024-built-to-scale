using Godot;

public partial class Ship : CharacterBody2D
{
    public enum State { PILOTING, DOCKED }
    public State currentShipState;

    [Export] Area2D gravityCheckArea;
    [Export] Area2D boundCheckArea;
    [Export] Area2D warpShipCaptureArea;
    [Export] Sprite2D dirPointer;
    [Export] Sprite2D bodyPointer;
    [Export] GpuParticles2D boostParticles;
    [Export] Label fuelLevelReadout;
    [Export] Label infoLabel;
    [Export] Marker2D dockPoint;
    [Export] TextureProgressBar boardingBar;
    [Export] public TextureProgressBar fuel;
    [Export] public Timer podBoardingTimer;

    float speed = 200;
    float boostCoefficient = 0.01f;
    Vector2 velocity = new Vector2(100, 0);
    Vector2 direction;
    Sun sun;
    WarpShip warpShip;
    Session session;
    public bool boosting = false;
    public bool podReady = true;
    public bool warpShipReady = true;
    public int podCounter = 1;
    public int podsArrived = 0;
    bool nearWarpShip = false;

    public Planet planetNear = null;
    Comet cometNear = null;
    GpuParticles2D warpShipBeam;
    Label boardingTitle;


    public override void _Ready()
    {
        currentShipState = State.PILOTING;

        sun = GetNode<Sun>("/root/main/sun");
        session = GetNode<Session>("/root/Session");
        warpShip = GetNode<WarpShip>("/root/main/warp-ship");
        warpShipBeam = warpShip.GetNode<GpuParticles2D>("beam-particles");
        boardingTitle = boardingBar.GetNode<Label>("title");

        gravityCheckArea.AreaEntered += OnGravityAreaEntered;
        gravityCheckArea.AreaExited += OnGravityAreaExited;
        boundCheckArea.AreaEntered += OnBoundaryEntered;
        warpShipCaptureArea.AreaEntered += OnWarpShipAreaEntered;
        warpShipCaptureArea.AreaExited += OnWarpShipAreaExited;
        podBoardingTimer.Timeout += OnBoardingTimerTimeout;
    }

    public override void _PhysicsProcess(double delta)
    {
        switch (currentShipState)
        {
            case State.PILOTING:
                _Process_Piloting(delta);
                break;
            case State.DOCKED:
                _Process_Docked(delta);
                break;
        }
    }

    public void _Process_Piloting(double delta)
    {
        UpdateDirection();
        CalculateVelocity();
        UpdateHelpers();
        CheckForBoosting();
    }

    public void _Process_Docked(double delta)
    {
        GlobalPosition = dockPoint.GlobalPosition;
        Rotation = 0;
        boostParticles.Emitting = false;
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("reset")) PrepReset();
        fuelLevelReadout.Text = fuel.Value.ToString();
        UpdateInfoLabel();
        DockWarpShip();
        dirPointer.Visible = currentShipState == State.DOCKED ? false : true;
        boardingBar.Value = podBoardingTimer.TimeLeft;

        if (planetNear != null)
        {
            if (planetNear.Name == "planet-pluto") session.plutoVisited = true;
            if (planetNear.Name == "planet-x") session.xFound = true;
        }
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

    private void DockWarpShip()
    {
        if (Input.IsActionJustPressed("interact"))
        {
            if (nearWarpShip)
            {
                currentShipState = State.DOCKED;
                nearWarpShip = false;
                warpShipBeam.Emitting = false;
            }
            else if (currentShipState == State.DOCKED)
            {
                currentShipState = State.PILOTING;
                warpShipBeam.Emitting = true;
                return;
            }
        }
    }

    public void LaunchWarpShip()
    {
        warpShip.velocity = new Vector2(25, 0);
        Area2D warpShipCaptureArea = warpShip.GetNode<Area2D>("ship-capture-area");
        warpShipCaptureArea.Monitorable = true;
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

    private void OnWarpShipAreaEntered(object _)
    {
        nearWarpShip = true;
    }

    private void OnWarpShipAreaExited(object _)
    {
        nearWarpShip = false;
    }

    private void OnBoundaryEntered(object _)
    {
        PrepReset();
    }

    private void OnBoardingTimerTimeout()
    {
        podReady = true;
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

        if (planetNear != null)
        {
            if (planetNear.Name == "planet-earth" && podCounter <= 3)
                infoLabel.Text = $"Left-click to launch Escape Pod #{podCounter}!";

            if (planetNear.Name == "planet-mars" && warpShipReady)
                infoLabel.Text = $"{podsArrived}/3 pod(s) have arrived. Left-click to launch Warp Ship!";
        }
        else if (nearWarpShip)
            infoLabel.Text = "Left-click to dock!";

        else if (currentShipState == State.DOCKED)
            infoLabel.Text = "Left-click to depart.";

        boardingTitle.Text = podBoardingTimer.IsStopped()
         ? "Escape Pod Ready!" : "Escape Pod Boarding...";
    }
}
