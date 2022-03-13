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
        private readonly Thread _thread;

        public bool IsRunning { get; set; }

        public BackgroundJobThread(ConcurrentQueue<BackgroundJobBase> jobsQueue, ConcurrentQueue<BackgroundJobBase> finishedQueue, string name)
        {
            _jobsQueue = jobsQueue;
            _finishedQueue = finishedQueue;
            _name = name;
            _thread = new Thread(Run);
        }

        public void Start()
        {
            _thread.Start();
            Console.WriteLine($"{_name} started");
        }

        public void Stop()
        {
            _thread.Interrupt();
        }

        private void Run()
        {
            try
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

                    Thread.Sleep(1);
                }
            }
            catch (ThreadInterruptedException e)
            {
                Console.WriteLine($"{_name} exited");
                throw;
            }
        }
    }
}