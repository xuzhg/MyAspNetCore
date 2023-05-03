//-----------------------------------------------------------------------------
// <copyright file="IMessageWriter.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//------------------------------------------------------------------------------


namespace ODataCborExample.Extensions
{
    // copied from ASP.NET Core doc
    public interface IMessageWriter
    {
        void Write(string message);
    }

    public class LoggingMessageWriter : IMessageWriter
    {

        private readonly ILogger<LoggingMessageWriter> _logger;

        public LoggingMessageWriter(ILogger<LoggingMessageWriter> logger) =>
            _logger = logger;

        public void Write(string message) =>
            _logger.LogInformation(message);
    }
}
