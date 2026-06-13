using System;
using System.Collections.Generic;
using System.Text;

namespace ZZZ_PS_Launcher
{
    public class CommitData
    {
        public CommitData(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }

        public string Value { get; }
    }
}
