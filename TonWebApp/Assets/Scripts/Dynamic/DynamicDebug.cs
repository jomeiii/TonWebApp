namespace Dynamic
{
    public static class DynamicDebug
    {
        public static void Debug(string className, string methodName, string message)
        {
            UnityEngine.Debug.Log(
                $"<color=green>[{className}]</color> " +
                $"<color=yellow>{methodName}()</color>: " +
                $"<color=white> {message}. </color>");
        }

        public static void DebugError(string className, string methodName, string message)
        {
            UnityEngine.Debug.LogError(
                $"<color=green>[{className}]</color> " +
                $"<color=yellow>{methodName}()</color>: " +
                $"<color=red> {message}. </color>");
        }

        public static void DebugWarning(string className, string methodName, string message)
        {
            UnityEngine.Debug.LogWarning(
                $"<color=green>[{className}]</color> " +
                $"<color=yellow>{methodName}()</color>: " +
                $"<color=red> {message}! </color>");
        }
    }
}