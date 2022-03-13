using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using SCVE.Editor.Abstractions;
using SCVE.Editor.Background;

namespace SCVE.Editor.Services
{
    public class BackgroundJobRunner : IService, IUpdateReceiver, IExitReceiver
    {
        private ConcurrentQueue<BackgroundJobBase> _jobs;
        private ConcurrentQueue<BackgroundJobBase> _finished;

        private List<BackgroundJobThread> _threads;

        private int _threadCount = Environment.ProcessorCount - 1;

        public BackgroundJobRunner()
        {
            _jobs = new ConcurrentQueue<BackgroundJobBase>();
            _finished = new ConcurrentQueue<BackgroundJobBase>();
            _threads = new List<BackgroundJobThread>(_threadCount);
            for (var i = 0; i < _threadCount; i++)
            {
                var thread = new BackgroundJobThread(_jobs, _finished, $"{(i+1)} Background Thread");
                thread.Start();
                _threads.Add(thread);
            }
        }

        public void PushJob(BackgroundJobBase job)
        {
            _jobs.Enqueue(job);
        }

        public void OnUpdate(float delta)
        {
            int i = 0;
            while (/*i++ < 3 && */_finished.TryDequeue(out var job))
            {
                job.OnFinished?.Invoke();
            }
        }

        public void OnExit()
        {
            foreach (var thread in _threads)
            {
                thread.Stop();
            }
        }
    }
}