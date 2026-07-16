using System;
using System.Collections.Concurrent;
using System.Threading;

public class ServerThread
{
    private Thread _thread;
    private BlockingCollection<ICommand> _commands = new BlockingCollection<ICommand>();
    private volatile bool _isRunning = true;
    private volatile bool _softStopRequested = false;

    public ServerThread()
    {
        _thread = new Thread(ProcessCommands);
        _thread.Start();
    }

    public void EnqueueCommand(ICommand command)
    {
        _commands.Add(command);
    }

    private void ProcessCommands()
    {
        try
        {
            while (_isRunning)
            {
                ICommand command = _commands.Take();
                command.Execute();
                if (_softStopRequested && _commands.IsEmpty)
                {
                    _isRunning = false;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void HardStop()
    {
        _isRunning = false;
        _commands.CompleteAdding();
        _thread.Join();
    }

    public void SoftStop()
    {
        _softStopRequested = true;
    }

    public void Wait()
    {
        _thread.Join();
    }
}