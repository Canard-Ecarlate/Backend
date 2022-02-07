namespace DuckCity.GameApi
{
    public static class DotEnv
    {
        private const int MaxElementsInARow = 2;

        public static void Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            foreach (string line in File.ReadAllLines(filePath))
            {
                string[] parts = line.Split('=', StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != MaxElementsInARow)
                {
                    continue;
                }
                Environment.SetEnvironmentVariable(parts[0], parts[1]);
            }
        }
    }
}
