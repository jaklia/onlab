using Robot.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Utility
{
    public class FunctionData
    {
        public List<CommandBase> Commands { get; set; } = new List<CommandBase>();

        public HashSet<Parameter> Parameters { get; set; } =
            new HashSet<Parameter>();




        public void AddCommand(CommandBase command)
        {
            Commands.Add(command);
        }

        public bool AddParameter(Parameter param)
        {
            return Parameters.Add(param);
        }
    }
}
