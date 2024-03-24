namespace DevConInstaller.Core.Logging
{
    public class LogMessage : LogMessageBase
    {
        public LogMessage(string message)
        {
            Message = message;
            Color = "Black";
        }
    }
}