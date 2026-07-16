public interface ICommand
{
    void Execute();
    bool IsFinished();
}