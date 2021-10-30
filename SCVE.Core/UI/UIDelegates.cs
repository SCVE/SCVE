namespace SCVE.Core.UI
{
    public class UIDelegates
    {
        public delegate void ContentSizeChangedDelegate();

        public delegate void ChildAddedDelegate(Component child);
        public delegate void ChildRemovedDelegate(Component child);
    }
}