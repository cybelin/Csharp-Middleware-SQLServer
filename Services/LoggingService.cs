using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public interface ILoggingService
{
    Task SaveLogsAsync(RequestLog requestLog, ResponseLog responseLog);
}

public class LoggingService : ILoggingService
{
    private readonly ApiLoggingDbContext _context;

    public LoggingService(ApiLoggingDbContext context)
    {
        _context = context;
    }

    public async Task SaveLogsAsync(RequestLog requestLog, ResponseLog responseLog)
    {
        // Store request log
        await _context.RequestLogs.AddAsync(requestLog);
        // Store response log
        await _context.ResponseLogs.AddAsync(responseLog);
        
        // Save changes to the database asynchronously
        await _context.SaveChangesAsync();
    }
}
