#region GNU General Public License

/* Copyright 2022 Vonhoff, MaxtorCoder
 *
 * This file is part of ResourcePackerGUI.
 *
 * ResourcePackerGUI is free software: you can redistribute it and/or modify it under the terms of the
 * GNU General Public License as published by the Free Software Foundation, either version 3 of the License,
 * or (at your option) any later version.
 *
 * ResourcePackerGUI is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along with ResourcePackerGUI.
 * If not, see <https://www.gnu.org/licenses/>.
 */

#endregion

namespace WinFormsUI.Common
{
    /// <summary>
    /// Based on: https://stackoverflow.com/a/45110025
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