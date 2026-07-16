
using System;
using System.Threading;

public class SampleCommand : ICommand
{
    public void Execute()
    {
        Thread.Sleep(1000);
    }
}