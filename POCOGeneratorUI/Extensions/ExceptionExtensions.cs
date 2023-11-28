using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace System
{
    public static partial class ExceptionExtensions
    {
        public static string GetUnhandledExceptionErrorMessage(this Exception ex)
        {
            return GetExceptionErrorMessage(ex,
                string.Format("An unhandled error occurred.{0}The application will terminate now.{0}", "\n")
            );
        }

        public static string GetExceptionErrorMessage(this Exception ex, string mainMessage = null)
        {
            if (ex == null)
                return null;

            string errorMessage = mainMessage;

            errorMessage += string.Format("{0}ERROR: {1}{0}STACK TRACE:{0}{2}",
                "\n",
                ex.Message,
                ex.GetFormattedStackTrace()
            );

            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
                errorMessage += string.Format("{0}ERROR: {1}{0}STACK TRACE:{0}{2}",
                    "\n",
                    ex.Message,
                    ex.GetFormattedStackTrace()
                );
            }

            errorMessage = errorMessage.Trim();

            return errorMessage;
        }

        public static string GetFormattedStackTrace(this Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            bool isFoundNamespaceNotSystem = false;

            StackTrace st = new StackTrace(ex, true);
            for (int i = 0; i < st.FrameCount; i++)
            {
                StackFrame sf = st.GetFrame(i);
                if (sf != null)
                {
                    MethodBase method = sf.GetMethod();
                    if (method != null)
                    {
                        Type reflectedType = method.ReflectedType;
                        if (reflectedType != null)
                        {
                            if (isFoundNamespaceNotSystem == false || reflectedType.Namespace.StartsWith("System") == false)
                            {
                                isFoundNamespaceNotSystem = reflectedType.Namespace.StartsWith("System") == false;

                                MethodInfo mi = method as MethodInfo;
                                if (mi != null)
                                {
                                    sb.Append((i + 1) + ". ");
                                    sb.Append(mi.GetSignature());

                                    int lineNumber = sf.GetFileLineNumber();
                                    if (lineNumber > 0)
                                    {
                                        sb.Append(" at line ");
                                        sb.Append(lineNumber);
                                    }

                                    sb.Append("\n");
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }

            return sb.ToString();
        }
    }
}
