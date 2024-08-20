using Godot;

public partial class Main : Node
{
	AudioStreamPlayer2D spaceBounce;
	AudioStreamPlayer2D spaceTerror;
	HSlider volSlider;
	Session session;


	public override void _Ready()
	{
		session = GetNode<Session>("/root/Session");

		Color bgColor = new Color(0.1f, 0.1f, 0.1f);
		RenderingServer.SetDefaultClearColor(bgColor);

		volSlider = GetTree().Root.GetNode<HSlider>("main/camera/volume-title/volume");
		spaceBounce = GetTree().Root.GetNode<AudioStreamPlayer2D>("main/ship/space-bounce");
		spaceTerror = GetTree().Root.GetNode<AudioStreamPlayer2D>("main/ship/space-terror");

		volSlider.Value = session.vol;

		int songToPlay = GD.RandRange(1, 2);
		if (songToPlay == 1) spaceBounce.Play();
		if (songToPlay == 2) spaceTerror.Play();
	}

	public override void _Process(double delta)
	{
		if (volSlider != null)
		{
			spaceBounce.VolumeDb = (float)volSlider.Value;
			spaceTerror.VolumeDb = (float)volSlider.Value;
		}
		session.vol = (float)volSlider.Value;
	}
}
