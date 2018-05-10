using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;

namespace Commix.Diagnostics
{
    public abstract class ThreadedPipelineTrace : IDisposable
    {
        public int ManagedThreadId { get; }

        private readonly IDisposable _onRun;
        private readonly IDisposable _onComplete;
        private readonly IDisposable _onError;
        private readonly IDisposable _onProcessorRun;
        private readonly IDisposable _onProcessorComplete;
        private readonly IDisposable _onProcessorError;

        protected ThreadedPipelineTrace(int managedThreadId, PipelineMonitor monitor)
        {
            ManagedThreadId = managedThreadId;

            var onRun = Observable.FromEventPattern<PipelineEventArgs>(
                h => monitor.RunEvent += h,
                h => monitor.RunEvent -= h);

            _onRun = onRun
                .Where(x => Thread.CurrentThread.ManagedThreadId == ManagedThreadId)
                .Subscribe(OnRun);

            var onComplete = Observable.FromEventPattern<PipelineEventArgs>(
                h => monitor.CompleteEvent += h,
                h => monitor.CompleteEvent -= h);

            _onComplete = onComplete
                .Where(x => Thread.CurrentThread.ManagedThreadId == ManagedThreadId)
                .Subscribe(OnComplete);

            var onError = Observable.FromEventPattern<PipelineErrorEventArgs>(
                h => monitor.ErrorEvent += h,
                h => monitor.ErrorEvent -= h);

            _onError = onError
                .Where(x => Thread.CurrentThread.ManagedThreadId == ManagedThreadId)
                .Subscribe(OnError);

            var onProcessorRun = Observable.FromEventPattern<PipelineProcessorEventArgs>(
                h => monitor.ProcessorRunEvent += h,
                h => monitor.ProcessorRunEvent -= h);

            _onProcessorRun = onProcessorRun
                .Where(x => Thread.CurrentThread.ManagedThreadId == ManagedThreadId)
                .Subscribe(OnProcessorRun);

            var onProcessorComplete = Observable.FromEventPattern<PipelineProcessorEventArgs>(
                h => monitor.ProcessorCompleteEvent += h,
                h => monitor.ProcessorCompleteEvent -= h);

            _onProcessorComplete = onProcessorComplete
                .Where(x => Thread.CurrentThread.ManagedThreadId == ManagedThreadId)
                .Subscribe(OnProcessorComplete);

            var onProcessorError = Observable.FromEventPattern<PipelineProcessorExceptionEventArgs>(
                h => monitor.ProcessorExceptionEvent += h,
                h => monitor.ProcessorExceptionEvent -= h);

            _onProcessorError = onProcessorError
                .Where(x => Thread.CurrentThread.ManagedThreadId == ManagedThreadId)
                .Subscribe(OnProcessorError);
        }

        protected virtual void OnRun(EventPattern<PipelineEventArgs> args)
        {}
        
        protected virtual void OnComplete(EventPattern<PipelineEventArgs> args)
        {}

        protected virtual void OnError(EventPattern<PipelineErrorEventArgs> args)
        {}

        protected virtual void OnProcessorRun(EventPattern<PipelineProcessorEventArgs> args)
        {}
        
        protected virtual void OnProcessorComplete(EventPattern<PipelineProcessorEventArgs> args)
        {}

        protected virtual void OnProcessorError(EventPattern<PipelineProcessorExceptionEventArgs> args)
        {}

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
                _onError?.Dispose();
                _onProcessorRun?.Dispose();
                _onProcessorComplete?.Dispose();
                _onProcessorError?.Dispose();
            }
        }
    }
}