public class LongRunningCommand : ICommand
{
    private int _progress = 0;
    private int _totalSteps;

    public LongRunningCommand(int totalSteps)
    {
        _totalSteps = totalSteps;
    }

    public void Execute()
    {
        if (_progress < _totalSteps)
        {
            Thread.Sleep(100); // задержка
            _progress++;
        }
    }

    public bool IsFinished()
    {
        return _progress >= _totalSteps;
    }
}