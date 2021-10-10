using SCVE.Core.Services;

namespace SCVE.Null
{
    public class DeltaTimeZero : IDeltaTimeProvider
    {
        public float Get()
        {
            return 0f;
        }
    }
}