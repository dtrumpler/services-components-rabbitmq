using System;

namespace OpenTable.Services.Components.Common
{
    public static class SystemTime
    {
        private static DateTime _setTime = DateTime.MinValue;

        public static void Clear()
        {
            _setTime = DateTime.MinValue;
        }

        public static void Set(DateTime toSet)
        {
            _setTime = toSet;
        }

        public static void SetUtc(DateTime toSet)
        {
            _setTime = toSet.ToLocalTime();
        }

        public static DateTime Now()
        {
            if (_setTime == DateTime.MinValue)
                return DateTime.Now;
            return _setTime;
        }

        public static DateTime UtcNow()
        {
            if (_setTime == DateTime.MinValue)
                return DateTime.UtcNow;
            return _setTime.ToUniversalTime();
        }
    }
}
