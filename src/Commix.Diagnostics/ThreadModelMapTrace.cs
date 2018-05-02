using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;

using Commix.Pipeline.Model.Processors;

namespace Commix.Diagnostics
{
    public class ThreadModelMapTrace : IDisposable
    {
        public int ManagedThreadId { get; }

        private readonly IDisposable _onRun;
        private readonly IDisposable _onComplete;

        private readonly Stack<Guid> _instanceStack = new Stack<Guid>();

        public ThreadModelMapTrace(int managedThreadId, IObservableModelMapperProcessor observableProcessor)
        {
            ManagedThreadId = managedThreadId;

            var onRun = Observable.FromEventPattern<ModelMapperMonitor.ModelMapperMonitorArgs>(
                h => observableProcessor.Monitor.RunEvent += h,
                h => observableProcessor.Monitor.RunEvent -= h);

            _onRun = onRun
                .Where(x => Thread.CurrentThread.ManagedThreadId == ManagedThreadId)
                .Subscribe(OnNext);

            var onComplete = Observable.FromEventPattern<ModelMapperMonitor.ModelMapperMonitorArgs>(
                h => observableProcessor.Monitor.CompleteEvent += h,
                h => observableProcessor.Monitor.CompleteEvent -= h);

            _onComplete = onComplete
                .Where(x => Thread.CurrentThread.ManagedThreadId == ManagedThreadId)
                .Subscribe(OnComplete);
        }

        private void OnNext(EventPattern<ModelMapperMonitor.ModelMapperMonitorArgs> eventPattern)
        {
            Console.WriteLine($"{string.Concat(Enumerable.Repeat('>', _instanceStack.Count))} {ManagedThreadId} {eventPattern.EventArgs.ModelType} (Run)");

            _instanceStack.Push(Guid.NewGuid());
        }

        private void OnComplete(EventPattern<ModelMapperMonitor.ModelMapperMonitorArgs> eventPattern)
        {
            _instanceStack.Pop();

            Console.WriteLine($"{string.Concat(Enumerable.Repeat('>', _instanceStack.Count))} {ManagedThreadId} {eventPattern.EventArgs.ModelType} (Complete)");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _onRun?.Dispose();
                _onComplete?.Dispose();
            }
        }
    }
}