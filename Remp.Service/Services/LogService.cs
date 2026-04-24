using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Remp.Service.Interfaces;
using Remp.Service.LogModels;

namespace Remp.Service.Services;

public class LogService : ILogService
{
    private readonly IMongoCollection<AuthLog> _authLogs;
    private readonly IMongoCollection<CaseHistoryLog> _caseHistoryLogs;

    public LogService(IConfiguration configuration)
    {
        MongoClient client = new MongoClient(configuration["MongoDB:ConnectionString"]);
        IMongoDatabase database = client.GetDatabase(configuration["MongoDB:DatabaseName"]);
        _authLogs = database.GetCollection<AuthLog>("AuthLogs");
        _caseHistoryLogs = database.GetCollection<CaseHistoryLog>("CaseHistory");
    }

    public async Task LogAuthAsync(AuthLog log)
    {
        log.Timestamp = DateTime.UtcNow;
        await _authLogs.InsertOneAsync(log);
    }

    public async Task LogCaseHistoryAsync(CaseHistoryLog log)
    {
        log.Timestamp = DateTime.UtcNow;
        await _caseHistoryLogs.InsertOneAsync(log);
    }
}
