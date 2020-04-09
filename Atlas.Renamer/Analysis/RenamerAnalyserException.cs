using System;

namespace Atlas.Renamer.Analysis
{
    public class RenamerAnalyserException : Exception
    {
        public RenamerAnalyserException(string message)
            : base(message) { }
    }
}