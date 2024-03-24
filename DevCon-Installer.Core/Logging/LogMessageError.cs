namespace DevConInstaller.Core.Logging
{
    public class LogMessageError : LogMessageBase
    {
        public LogMessageError(string message)
        {
            Message = message;
            Color = "Red";
        }
    }
}