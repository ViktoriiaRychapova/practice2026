using System.Collections.Generic;

public class RoundRobinScheduler : IScheduler
{
    private readonly Queue<ICommand> _commands = new Queue<ICommand>();
    private readonly object _lock = new object();

    public bool HasCommand()
    {
        lock (_lock)
        {
            return _commands.Count > 0;
        }
    }

    public ICommand Select()
    {
        lock (_lock)
        {
            if (_commands.Count == 0)
                return null;
            var cmd = _commands.Dequeue();
            _commands.Enqueue(cmd);
            return cmd;
        }
    }

    public void Add(ICommand cmd)
    {
        lock (_lock)
        {
            _commands.Enqueue(cmd);
        }
    }
}