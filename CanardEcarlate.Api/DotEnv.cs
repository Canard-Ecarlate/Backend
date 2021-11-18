namespace CanardEcarlate.Api
{
    using System;
    using System.IO;

    public static class DotEnv
    {
        private const int MAX_ELEMENTS_IN_A_ROW = 2;

        public static void Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            foreach (var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split('=', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != MAX_ELEMENTS_IN_A_ROW)
                {
                    continue;
                }
                Environment.SetEnvironmentVariable(parts[0], parts[1]);
            }
        }
    }
}
