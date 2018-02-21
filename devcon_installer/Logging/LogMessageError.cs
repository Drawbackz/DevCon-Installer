namespace devcon_installer.Logging
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