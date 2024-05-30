using Godot;

public partial class SystemLine : Label {
    private Timer timer;

    public override void _Ready() {
        timer = GetNode<Timer>("LineTimer");
    }

    public void SetTextStartTimer(string text) {
        Text = text;
        timer.Start();
    }

    public void OnLineTimeout() {
        QueueFree();
    }
}
