namespace HumToon.Editor
{
    public static class HumToonExtensionMethods
    {
        public static bool ToBool(this float value)
        {
            return value >= 0.5;
        }

        public static float ToFloat(this bool value)
        {
            return value ? 1.0f : 0.0f;
        }
    }
}
