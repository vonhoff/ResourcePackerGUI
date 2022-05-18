namespace ResourcePacker.Common
{
    /// <summary>
    /// From: https://stackoverflow.com/a/45110025
    /// </summary>
    public class ActionDebouncer : IDisposable
    {
        private readonly Action _action;
        private readonly TimeSpan _timeSpan;
        private readonly HashSet<ManualResetEvent> _resets = new();
        private readonly object _mutex = new();

        public ActionDebouncer(Action action, TimeSpan timespan)
        {
            _timeSpan = timespan;
            _action = action;
        }

        public void Dispose()
        {
            lock (_mutex)
            {
                while (_resets.Count > 0)
                {
                    var reset = _resets.First();
                    _resets.Remove(reset);
                    reset.Set();
                }
            }

            GC.SuppressFinalize(this);
        }

        public void Invoke()
        {
            var thisReset = new ManualResetEvent(false);

            lock (_mutex)
            {
                while (_resets.Count > 0)
                {
                    var otherReset = _resets.First();
                    _resets.Remove(otherReset);
                    otherReset.Set();
                }

                _resets.Add(thisReset);
            }

            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    if (!thisReset.WaitOne(_timeSpan))
                    {
                        _action();
                    }
                }
                finally
                {
                    lock (_mutex)
                    {
                        using (thisReset)
                        {
                            _resets.Remove(thisReset);
                        }
                    }
                }
            });
        }
    }
}