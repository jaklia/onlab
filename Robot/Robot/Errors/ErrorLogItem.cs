using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Errors
{
    public class ErrorLogItem
    {
        public string Text { get; private set; }

        public string Type { get; private set; }

        public int Line { get; private set; }

        public ErrorLogItem(string text, string type, int line)
        {
            Text = text;
            Type = type;
            Line = line;
        }
    }
}
