﻿using MaxLib.Collections;
using System;
using System.Collections.Generic;
using System.Text;
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
                stage.Execute(this);
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

        public abstract string TaskInfo { get; }

        public abstract bool Finished { get; }

        public abstract void Execute(UpdateManager manager);

        public abstract bool CanStart(UpdateManager manager);

        public event Action<UpdateTask> NewTaskAdded;

        protected void OnNewTaskAdded(UpdateTask task)
        {
            NewTaskAdded?.Invoke(task);
        }
    }

    abstract class UpdateStage<T> : UpdateStage where T: UpdateTask
    {
        public List<T> Tasks { get; private set; }

        public T CurrentTask { get; private set; }

        bool finished = false;
        public override bool Finished => finished;

        public UpdateStage()
        {
            Tasks = new List<T>();
        }

        public override void Execute(UpdateManager manager)
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
                OnNewTaskAdded(t);
                (CurrentTask = t).Execute(manager);
                progress += step;
            }
            progress = 1;
            step = 0;
            CurrentTask = null;
            finished = true;
        }

        public override string TaskName => CurrentTask?.Name;

        public override string TaskInfo => CurrentTask?.Info;

        float progress = 0;
        float step = 0;
        public override float TaskProgress => progress + step * (CurrentTask?.Progress ?? 0);
    }

    abstract class UpdateTask
    {
        public event EventHandler ValueChanged;
        protected void OnValueChanged(EventArgs e = null)
        {
            ValueChanged?.Invoke(this, e ?? EventArgs.Empty);
        }


        string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnValueChanged();
            }
        }

        float progress;
        public float Progress
        {
            get => progress;
            set
            {
                progress = value;
                OnValueChanged();
            }
        }

        string info;
        public string Info
        {
            get => info;
            set
            {
                info = value;
                OnValueChanged();
            }
        }

        public abstract void Execute(UpdateManager manager);

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(Name ?? "unbenannte Aufgabe");
            sb.AppendFormat(" ({0:#0.00}%)", Progress * 100);
            if (Info != null) sb.AppendFormat(" ({0})", Info);
            return sb.ToString();
        }
    }
}
