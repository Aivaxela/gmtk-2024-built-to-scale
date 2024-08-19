using Godot;

public partial class PlanetDebris : GpuParticles2D
{
    [Export] Timer destroyTimer;

    public override void _Ready()
    {
        destroyTimer.Timeout += OnTimeout;
    }

    private void OnTimeout()
    {
        QueueFree();
    }
}
