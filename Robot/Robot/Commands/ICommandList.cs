using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Commands
{
    public interface ICommandList
    {
        CommandBase nextCmd();

        CommandBase prevCmd();
    }
}
