using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Technical
{
    /// <summary>
    /// Log helper.
    /// </summary>
    public static class TraceHelper
    {

        private static bool Initialized { get; set; }

        public static void Comment(string text)
        {
            if (!Initialized) Initialize();
            Trace.WriteLine(text);
        }

        public static void TraceFrame(List<byte> frame, bool sent)
        {
            if (!Initialized) Initialize();

            var comment = string.Join(" ", frame.Select(x => x.ToString("X")).ToArray());
            if (sent) comment = "Envoyé : " + comment;
            else comment = "Reçu : " + comment;
            Trace.WriteLine(comment);
            Trace.Flush();
        }

        private static void Initialize()
        {
            Trace.Listeners.Add(new TextWriterTraceListener("D:\\framew.txt"));
            Initialized = true;
            Trace.AutoFlush = true;
        }

    }
}
