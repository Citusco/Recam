using System;
using Remp.Service.LogModels;

namespace Remp.Service.Interfaces;

public interface ILogService
{
    Task LogAuthAsync(AuthLog log);
    Task LogCaseHistoryAsync(CaseHistoryLog log);
}
