using System;

public class HardStopCommand : ICommand
{
    private ServerThread _server;

    public HardStopCommand(ServerThread server)
    {
        _server = server;
    }

    public void Execute()
    {
        
        _server.HardStop();
    }
}