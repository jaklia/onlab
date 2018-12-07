using System;

namespace Robot.Commands
{
    public interface ICommandList
    {
        event Action<CommandList> ListContextEntered;

        event Action<CommandList> ListContextExited;

    }
}
