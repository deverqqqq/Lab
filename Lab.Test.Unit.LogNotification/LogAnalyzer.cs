using System;
using Lab.Test.Unit.LogNotification.Interfaces;

namespace Lab.Test.Unit.LogNotification;

public class LogAnalyzer
{
    private readonly IWebService _webService;

    public LogAnalyzer(IWebService webService)
    {
        this._webService = webService;
    }

    public bool WasLastFileNameValid { get; set; }

    public bool IsValidLogFileName(string fileName)
    {
        this.WasLastFileNameValid = false;
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException("fileName has to be provided");
        }

        var isValid = fileName.EndsWith(".SLF", StringComparison.CurrentCultureIgnoreCase);
        this.WasLastFileNameValid = isValid;

        return isValid;
    }

    public void Analyze(string fileName)
    {
        if (fileName.Length < 8)
        {
            this._webService.LogError($"FIleName too short: {fileName}");
        }
    }
}