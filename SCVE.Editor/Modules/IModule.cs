namespace SCVE.Editor.Modules
{
    public interface IModule
    {
        void OnUpdate();
        void Init();

        /// <summary>
        /// The only place where modules should reference each other
        /// </summary>
        void CrossReference(Modules modules);
    }
}