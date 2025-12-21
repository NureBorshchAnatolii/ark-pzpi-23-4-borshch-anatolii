using System.Text.RegularExpressions;
using CareLink.Application.Dtos.Admin;
using Microsoft.Extensions.Configuration;

namespace CareLink.Application.Implementations
{
    public class FileLogReaderService
    {
        private readonly string _logFilePath;

        public FileLogReaderService(IConfiguration config)
        {
            _logFilePath = config["Logging:FilePath"] ?? "logs/log.txt";
        }

        public async Task<IEnumerable<SystemLogDto>> ReadLogsAsync(DateTime? from = null, DateTime? to = null, string? level = null)
        {
            if (!File.Exists(_logFilePath))
                return Enumerable.Empty<SystemLogDto>();

            var lines = await File.ReadAllLinesAsync(_logFilePath);

            var logs = lines
                .Select(line => ParseLine(line))
                .Where(log => log != null)
                .Cast<SystemLogDto>();

            if (from.HasValue) logs = logs.Where(l => l.Timestamp >= from.Value);
            if (to.HasValue) logs = logs.Where(l => l.Timestamp <= to.Value);
            if (!string.IsNullOrEmpty(level)) logs = logs.Where(l => l.Level.Equals(level, StringComparison.OrdinalIgnoreCase));

            return logs;
        }

        private SystemLogDto? ParseLine(string line)
        {
            var match = Regex.Match(line, 
                @"^(?<date>\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d+)\s\[(?<level>\w+)\]\s(?<source>.+?)\s-\s(?<message>.+)$");
            if (!match.Success) return null;

            return new SystemLogDto
            {
                Timestamp = DateTime.Parse(match.Groups["date"].Value),
                Level = match.Groups["level"].Value,
                Source = match.Groups["source"].Value,
                Message = match.Groups["message"].Value
            };
        }
    }
}