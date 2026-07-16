using System.Collections.Generic;
using System.Threading;

namespace task19
{
    public class Scheduler
    {
        private Queue<ICommand> _commands = new Queue<ICommand>();
        private object _lock = new object();

        public void Add(ICommand cmd)
        {
            lock(_lock)
            {
                _commands.Enqueue(cmd);
            }
        }

        public ICommand GetNext()
        {
            lock(_lock)
            {
                if(_commands.Count == 0) return null;
                var cmd = _commands.Dequeue();
                _commands.Enqueue(cmd);
                return cmd;
            }
        }

        public bool HasCommands()
        {
            lock(_lock)
            {
                return _commands.Count > 0;
            }
        }
    }
}