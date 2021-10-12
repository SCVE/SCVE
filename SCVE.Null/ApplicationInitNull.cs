using SCVE.Core.App;
using SCVE.Core.Entities;
using SCVE.Core.Services;

namespace SCVE.Null
{
    public class ApplicationInitNull : ApplicationInit
    {
        public ApplicationInitNull() : base(
            new RendererNull(),
            new FileLoaderNull(),
            new DeltaTimeZero(),
            null
        )
        {
        }
    }
}