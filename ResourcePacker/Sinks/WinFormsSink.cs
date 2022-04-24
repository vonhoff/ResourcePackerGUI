using Serilog;
using Serilog.Formatting;
using Serilog.Formatting.Display;

namespace ResourcePacker.Sinks
{
    internal static class WinFormsSink
    {
        private static readonly ITextFormatter DefaultTextFormatter =
            new MessageTemplateTextFormatter("[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}");

        public static WinFormsSinkInternal TextBoxSink { get; private set; } = new(DefaultTextFormatter);

        public static WinFormsSinkInternal MakeTextBoxSink(ITextFormatter? formatter = null)
        {
            formatter ??= DefaultTextFormatter;
            TextBoxSink = new WinFormsSinkInternal(formatter);
            return TextBoxSink;
        }

        public static LoggerConfiguration WriteToTextBox(this LoggerConfiguration configuration, ITextFormatter? formatter = null)
        {
            return configuration.WriteTo.Sink(MakeTextBoxSink(formatter));
        }
    }
}