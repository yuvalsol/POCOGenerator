using System;
using System.Text;

namespace POCOGenerator.POCOWriters
{
    internal abstract class POCOWriter
    {
        private StringBuilder snapshot;

        public void StartSnapshot()
        {
            if (snapshot == null)
                snapshot = new StringBuilder();
        }

        public void SnapshotClear()
        {
            snapshot?.Clear();
        }

        public void SnapshotWrite(string text)
        {
            snapshot?.Append(text);
        }

        public void SnapshotWriteLine(string text = null)
        {
            snapshot?.AppendLine(text);
        }

        public StringBuilder EndSnapshot()
        {
            if (snapshot != null)
            {
                StringBuilder sb = snapshot;
                snapshot = null;
                return sb;
            }

            return null;
        }
    }
}
