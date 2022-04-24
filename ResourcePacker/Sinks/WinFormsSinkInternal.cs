using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;

namespace ResourcePacker.Sinks
{
    internal sealed class WinFormsSinkInternal : ILogEventSink
    {
        private readonly ITextFormatter _textFormatter;

        public WinFormsSinkInternal(ITextFormatter textFormatter)
        {
            _textFormatter = textFormatter;
        }

        public delegate void LogHandler(string? sourceContext, string str);

        public event LogHandler? OnLogReceived;

        public void Emit(LogEvent logEvent)
        {
            if (logEvent == null)
            {
                throw new ArgumentNullException(nameof(logEvent));
            }

            if (_textFormatter == null)
            {
                throw new NullReferenceException(nameof(_textFormatter));
            }

            var renderSpace = new StringWriter();
            _textFormatter.Format(logEvent, renderSpace);

            logEvent.Properties.TryGetValue("SourceContext", out var contextProperty);
            FireEvent(contextProperty?.ToString().Trim('"'), renderSpace.ToString());
        }

        private void FireEvent(string? context, in string str)
        {
            OnLogReceived?.Invoke(context, str);
        }
    }
}