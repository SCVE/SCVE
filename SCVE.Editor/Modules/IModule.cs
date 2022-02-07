namespace SCVE.Editor.Modules
{
    public interface IModule
    {
        void OnUpdate();
        void OnInit();

        /// <summary>
        /// The only place where modules should reference each other
        /// </summary>
        void CrossReference(ModulesContainer modulesContainer);
    }
}