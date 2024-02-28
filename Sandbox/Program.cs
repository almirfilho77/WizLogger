using WizLog;

Console.WriteLine("Hello, World!");
WizLogger.Initialize("log.txt", WizLogger.LogLevel.DEBUG);
WizLogger.Info("Writing this info to the file!!");
WizLogger.Debug("Writing this debug to the file!!");
WizLogger.Error("Writing this error to the file!!");