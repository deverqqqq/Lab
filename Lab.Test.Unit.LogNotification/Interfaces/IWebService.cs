using System.Threading;

namespace Lab.Test.Unit.LogNotification.Interfaces;

public interface IWebService
{
    void LogError(string message);
}