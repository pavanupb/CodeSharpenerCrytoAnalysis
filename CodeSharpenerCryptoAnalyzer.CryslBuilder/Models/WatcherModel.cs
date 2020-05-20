using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CodeSharpenerCryptoAnalyzer.CryslBuilder.Models
{
    public class WatcherModel
    {
        public string FilePath { get; set; }
        public WatcherChangeTypes  ChangeType { get; set; }
        public bool IsFileModified { get; set; }
    }
}
