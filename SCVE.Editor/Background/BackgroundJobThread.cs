using System;
using System.Collections.Concurrent;
using System.Threading;

namespace SCVE.Editor.Background
{
    public class BackgroundJobThread
    {
        private readonly ConcurrentQueue<BackgroundJobBase> _jobsQueue;
        private readonly ConcurrentQueue<BackgroundJobBase> _finishedQueue;
        private readonly string _name;
        private Thread _thread;

        private SemaphoreSlim _isRunningSemaphore;
        public bool IsRunning { get; set; }

        public BackgroundJobThread(ConcurrentQueue<BackgroundJobBase> jobsQueue, ConcurrentQueue<BackgroundJobBase> finishedQueue, string name)
        {
            _jobsQueue = jobsQueue;
            _finishedQueue = finishedQueue;
            _name = name;
            _thread = new Thread(Run);
            _isRunningSemaphore = new SemaphoreSlim(1);
        }

        public void Start()
        {
            _thread.Start();
            Console.WriteLine($"{_name} started");
        }

        private void Run()
        {
            while (true)
            {
                if (_jobsQueue.TryDequeue(out var job))
                {
                    IsRunning = true;
                    job.Run();
                    _finishedQueue.Enqueue(job);
                    IsRunning = false;
                }

                if (_thread.ThreadState == ThreadState.AbortRequested)
                {
                    return;
                }
                else
                {
                    Thread.Sleep(1);
                }
            }
        }
    }
}