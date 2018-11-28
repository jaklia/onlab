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

        public string Type { get; private set; } //   antlr | custom

        public string Source { get; private set; } //   map | robot

        public int Line { get; private set; }

        public ErrorLogItem(string text, string type, string source, int line)
        {
            Text = text;
            Type = type;
            Source = source;
            Line = line;
        }
    }
}
