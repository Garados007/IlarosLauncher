using MaxLib.Collections;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IlarosLauncher.UpdateClient.Update
{
    class UpdateManager : IDisposable
    {
        public List<UpdateStage> Stages { get; private set; }

        public int MaxParallelExecution { get; private set; }

        public SyncedList<UpdateStage> ExecutingStages { get; private set; }

        Semaphore ExecutionLimiter;

        public event Action<UpdateManager, UpdateStage> StartExecution, EndExecution;

        public UpdateManager(int maxParallelExecution)
        {
            Stages = new List<UpdateStage>();
            MaxParallelExecution = maxParallelExecution;
            ExecutionLimiter = new Semaphore(maxParallelExecution, maxParallelExecution);
            ExecutingStages = new SyncedList<UpdateStage>();
        }

        public T GetStage<T>() where T: UpdateStage
        {
            return Stages.Find((s) => s is T).As<T>();
        }

        public async Task Execute()
        {
            await Task.WhenAll(Stages.ConvertAll((stage) => Execute(stage)).ToArray());
        }

        private async Task Execute(UpdateStage stage)
        {
            await Task.Run(() =>
            {
                SpinWait.SpinUntil(() => stage.CanStart(this));
                ExecutionLimiter.WaitOne();
                ExecutingStages.Add(stage);
                StartExecution?.Invoke(this, stage);
                stage.Execute();
                ExecutingStages.Remove(stage);
                EndExecution?.Invoke(this, stage);
                ExecutionLimiter.Release();
            });
        }

        public void Dispose()
        {
            ExecutionLimiter.Dispose();
        }
    }

    abstract class UpdateStage
    {
        public T As<T>() where T : UpdateStage
        {
            return this as T;
        }

        public abstract string GlobalTaskVerb { get; }

        public abstract string TaskName { get; }

        public abstract float TaskProgress { get; }

        public abstract bool Finishes { get; }

        public abstract void Execute();

        public abstract bool CanStart(UpdateManager manager);
    }

    abstract class UpdateStage<T> : UpdateStage where T: UpdateTask
    {
        public List<T> Tasks { get; private set; }

        public T CurrentTask { get; private set; }

        public UpdateStage()
        {
            Tasks = new List<T>();
        }

        public override void Execute()
        {
            if (Tasks.Count == 0)
            {
                progress = 1;
                step = 0;
                CurrentTask = null;
                return;
            }
            progress = 0;
            step = 1f / Tasks.Count;
            foreach (var t in Tasks)
            {
                (CurrentTask = t).Execute();
                progress += step;
            }
            progress = 1;
            step = 0;
            CurrentTask = null;
        }

        public override string TaskName => CurrentTask?.Name;

        float progress = 0;
        float step = 0;
        public override float TaskProgress => progress + step * (CurrentTask?.Progress ?? 0);
    }

    abstract class UpdateTask
    {
        public string Name { get; protected set; }

        public float Progress { get; protected set; }

        public abstract void Execute();
    }
}
