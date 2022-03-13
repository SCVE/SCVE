using System;

namespace SCVE.Editor.Background
{
    public abstract class BackgroundJobBase
    {
        protected BackgroundJobBase(Action onFinished)
        {
            OnFinished = onFinished;
        }

        protected BackgroundJobBase()
        {
        }

        public Action OnFinished { get; set; }

        public int Progress { get; set; }

        /// <summary>
        /// the method, to be ran in another thread
        /// </summary>
        public abstract void Run();
    }
}