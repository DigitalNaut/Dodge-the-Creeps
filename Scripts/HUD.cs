using Godot;

namespace Game;

public partial class HUD : CanvasLayer
{
	Label _messageLabel;
	Label _scoreLabel;
	Timer _messageTimer;
	Button _startButton;

	[Signal]
	public delegate void StartGameEventHandler();

	public override void _Ready()
	{
		_messageLabel = GetNode<Label>("MessageLabel");
		_scoreLabel = GetNode<Label>("ScoreLabel");
		_messageTimer = GetNode<Timer>("MessageTimer");
		_startButton = GetNode<Button>("StartButton");
	}

	public void ShowMessage(string text)
	{
		_messageLabel.Text = text;
		_messageLabel.Show();

		_messageTimer.Start();
	}

	async public void ShowGameOver()
	{
		ShowMessage("Game Over");

		await ToSignal(_messageTimer, Timer.SignalName.Timeout);
		_messageLabel.Text = "Dodge the Creeps!";
		_messageLabel.Show();

		await ToSignal(GetTree().CreateTimer(1f), SceneTreeTimer.SignalName.Timeout);
		_startButton.Show();
	}

	public void UpdateScore(int score) => _scoreLabel.Text = score.ToString();

	void OnStartButtonPressed()
	{
		_startButton.Hide();
		EmitSignal(SignalName.StartGame);
	}

	void OnMessageTimerTimeout() => _messageLabel.Hide();
}
