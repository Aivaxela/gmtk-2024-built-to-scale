using Godot;

public partial class Main : Node
{
	public override void _Ready()
	{
		Color bgColor = new Color(0.1f, 0.1f, 0.1f);
		RenderingServer.SetDefaultClearColor(bgColor);
	}

	public override void _Process(double delta)
	{
	}
}
