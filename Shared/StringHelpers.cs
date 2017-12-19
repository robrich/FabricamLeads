namespace Fabricam.Shared
{
    public static class StringHelpers
    {

        public static int? ToIntOrNull(this string Source)
        {
            int result = 0;
            if (int.TryParse(Source, out result)) {
                return result;
            }
            return null;
        }

    }
}
