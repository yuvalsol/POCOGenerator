namespace System
{
    public static partial class EventExtensions
    {
        public static void Raise(this EventHandler handler, object sender, EventArgs args)
        {
            if (handler != null)
            {
                foreach (EventHandler listener in handler.GetInvocationList())
                    listener.Invoke(sender, args);
            }
        }

        public static EventArgs Raise(this EventHandler handler, object sender, Func<EventArgs> argsHandler)
        {
            if (handler != null)
            {
                EventArgs args = argsHandler();
                foreach (EventHandler listener in handler.GetInvocationList())
                    listener.Invoke(sender, args);
                return args;
            }

            return null;
        }

        public static void RaiseAsync(this EventHandler handler, object sender, EventArgs args)
        {
            if (handler != null)
            {
                foreach (EventHandler listener in handler.GetInvocationList())
                    listener.BeginInvoke(sender, args, (ar) => { try { listener.EndInvoke(ar); } catch { } }, null);
            }
        }

        public static EventArgs RaiseAsync(this EventHandler handler, object sender, Func<EventArgs> argsHandler)
        {
            if (handler != null)
            {
                EventArgs args = argsHandler();
                foreach (EventHandler listener in handler.GetInvocationList())
                    listener.BeginInvoke(sender, args, (ar) => { try { listener.EndInvoke(ar); } catch { } }, null);
                return args;
            }

            return null;
        }

        public static void Raise<TEventArgs>(this EventHandler<TEventArgs> handler, object sender, TEventArgs args) where TEventArgs : EventArgs
        {
            if (handler != null)
            {
                foreach (EventHandler<TEventArgs> listener in handler.GetInvocationList())
                    listener.Invoke(sender, args);
            }
        }

        public static TEventArgs Raise<TEventArgs>(this EventHandler<TEventArgs> handler, object sender, Func<TEventArgs> argsHandler) where TEventArgs : EventArgs
        {
            if (handler != null)
            {
                TEventArgs args = argsHandler();
                foreach (EventHandler<TEventArgs> listener in handler.GetInvocationList())
                    listener.Invoke(sender, args);
                return args;
            }

            return null;
        }

        public static void RaiseAsync<TEventArgs>(this EventHandler<TEventArgs> handler, object sender, TEventArgs args) where TEventArgs : EventArgs
        {
            if (handler != null)
            {
                foreach (EventHandler<TEventArgs> listener in handler.GetInvocationList())
                    listener.BeginInvoke(sender, args, (ar) => { try { listener.EndInvoke(ar); } catch { } }, null);
            }
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
