using System.Collections.Generic;
using System.Threading;

namespace App.Core.Services
{
    public class ThreadManagerService
    {
        private List<Thread> runningThreads = new List<Thread>();

        public void AddThread(Thread thread)
        {
            
            runningThreads.Add(thread);

        }

        public void RemoveThread(Thread thread)
        {
            runningThreads.Remove(thread);
        }

        public bool AreThreadsRunning(int threadNumber)
        {
            if (runningThreads.Count == 0 || threadNumber >= runningThreads.Count)
            {
                return false;
            }

            try
            {
                Console.WriteLine($"Thread {runningThreads[threadNumber].ThreadState}");
                return runningThreads[threadNumber].ThreadState == ThreadState.Running;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

    }
}
