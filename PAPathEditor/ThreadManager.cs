using System;
using System.Collections.Generic;

namespace PAPathEditor
{
    public static class ThreadManager
    {
        private static Queue<Action> executionQueue = new Queue<Action>();

        public static void ExecuteOnMainThread(Action action)
            => executionQueue.Enqueue(action);

        public static void ExecuteAll()
        {
            while (executionQueue.Count > 0)
                executionQueue.Dequeue().Invoke();
        }
    }
}
