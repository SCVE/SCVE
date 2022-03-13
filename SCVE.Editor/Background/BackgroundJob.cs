using System;

namespace SCVE.Editor.Background
{
    public abstract class BackgroundJob<T> : BackgroundJobBase
    {
        private Action<T> _onFinished;

        protected T Result;

        public BackgroundJob(Action<T> onFinished)
        {
            _onFinished = onFinished;
            OnFinished = () => _onFinished?.Invoke(Result);
        }
    }
}