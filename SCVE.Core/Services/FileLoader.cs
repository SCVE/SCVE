namespace SCVE.Core.Services
{
    public abstract class FileLoader<T>
    {
        public abstract T Load(string fileName);
    }
}