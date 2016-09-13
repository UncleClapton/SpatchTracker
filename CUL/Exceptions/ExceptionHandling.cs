﻿using Clapton.IO;
using System;
using System.Diagnostics;

namespace Clapton.Exceptions
{
    public static class ExceptionHandling
    {
        public static void ReportException(object sender, Exception exception)
        {
            #region const
            const string messageFormat = @"
------
ERROR, date = {0}, sender = {1},
{2}
";
            const string path = "error.log";
            #endregion

            try
            {
                var message = string.Format(messageFormat, DateTimeOffset.Now, sender, exception);

                Debug.WriteLine(message);
                File.TryWriteToFile(path, message, true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
