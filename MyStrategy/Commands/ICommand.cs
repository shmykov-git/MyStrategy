using System;

namespace MyStrategy.Commands
{
    public class Command<T>
    {
        private readonly Action<T> action;

        public Command(Action<T> action)
        {
            this.action = action;
        }

        public void Execute(T arg)
        {
            action(arg);
        }
    }
}
