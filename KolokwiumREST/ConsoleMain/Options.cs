using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMain
{
    class Options
    {

        [Option('u', "url")]
        public string Url { get; set; }

        [Option('t', "filetype")]
        public string FileType { get; set; }

        [Option('p', "path")]
        public string Path { get; set; }

        [Option('o', "outpath")]
        public string OutPath { get; set; }
    }
}
