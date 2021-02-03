using System;

namespace CreateNCSql.Code
{
    /// <summary>
    /// 
    /// </summary>
    public static class CommonHelper
    {

        public static string GetDateTimeString()
        {
            return $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
        }
    }
}
