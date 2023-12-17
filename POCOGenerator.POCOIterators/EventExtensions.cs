namespace System
{
    internal static partial class EventExtensions
    {
        public static TEventArgs Raise<TEventArgs>(this EventHandler<TEventArgs> handler, object sender, Func<TEventArgs> argsHandler) where TEventArgs : EventArgs
        {
            if (handler != null)
            {
                TEventArgs args = argsHandler();
                foreach (EventHandler<TEventArgs> listener in handler.GetInvocationList())
                {
                    listener.Invoke(sender, args);
                    if (args != null && args is POCOGenerator.POCOIterators.IStopGenerating stopGenerating && stopGenerating.Stop)
                        return args;
                }
                return args;
            }

            return null;
        }

        public static TEventArgs RaiseAsync<TEventArgs>(this EventHandler<TEventArgs> handler, object sender, Func<TEventArgs> argsHandler) where TEventArgs : EventArgs
        {
            if (handler != null)
            {
                TEventArgs args = argsHandler();
                foreach (EventHandler<TEventArgs> listener in handler.GetInvocationList())
                    listener.BeginInvoke(sender, args, (ar) => { try { listener.EndInvoke(ar); } catch { } }, null);
                return args;
            }

            return null;
        }
    }
}