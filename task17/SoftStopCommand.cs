public class SoftStopCommand : ICommand
{
    private ServerThread _server;

    public SoftStopCommand(ServerThread server)
    {
        _server = server;
    }

    public void Execute()
    {
        _server.SoftStop();
    }
}