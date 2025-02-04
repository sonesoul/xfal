global using System.Collections;
using System;
using System.Collections.Generic;

namespace PixelBox
{
    public class StepTask
    {
        public static class Manager
        {
            private readonly static List<StepTask> tasks = new();
            private readonly static Stack<IEnumerator> updateBuffer = new();

            public static void Update()
            {
                for (int i = tasks.Count - 1; i >= 0; i--)
                {
                    updateBuffer.Clear();

                    StepTask task = tasks[i];

                    try
                    {
                        if (!MoveNext(task.Iterator))
                        {
                            task.Complete();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new AggregateException($"StepTask inner exception: {ex.Message}");
                    }
                }
            }

            private static bool MoveNext(IEnumerator mainTask)
            {
                Stack<IEnumerator> nestedTasks = updateBuffer;

                //finding nested tasks
                IEnumerator task = mainTask;
                while (task.Current is IEnumerator subTask)
                {
                    task = subTask;

                    if (subTask != null)
                        nestedTasks.Push(task);
                }

                //updating nested tasks
                while (nestedTasks.Count > 0)
                {
                    if (nestedTasks.Peek().MoveNext())
                    {
                        return true;
                    }
                    
                    nestedTasks.Pop();
                }

                //all nested tasks are completed
                return mainTask.MoveNext();
            }

            public static void Register(StepTask task) => tasks.Add(task);
            public static void Unregister(StepTask task) => tasks.Remove(task);
        }

        public static YieldInstruction Yields { get; } = new();

        public event Action<StepTask> Completed;

        public IEnumerator Iterator { get; private set; }
        public bool IsRunning { get; private set; }

        public StepTask(IEnumerator iterator, bool start = true)
        {
            SetIterator(iterator);

            if (start)
            {
                Start();
            }
        }

        public void Start()
        {
            if (IsRunning)
                return;

            Manager.Register(this);
            IsRunning = true;
        }
        public void StartNew(IEnumerator iterator)
        {
            Stop();
            SetIterator(iterator);
            Start();
        }

        public void Stop()
        {
            if (!IsRunning)
                return;
            
            Manager.Unregister(this);
            IsRunning = false;
        }
        public void Complete()
        {
            Stop();
            Completed?.Invoke(this);
        }

        private void SetIterator(IEnumerator iterator)
        {
            Iterator = iterator ?? throw new ArgumentNullException(nameof(iterator));
        }
    }
}