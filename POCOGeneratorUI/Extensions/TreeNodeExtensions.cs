using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
    public static partial class TreeNodeExtensions
    {
        #region Hide CheckBox & Show Plus

        public const int TVIF_CHILDREN = 0x0040;
        public const int TVIF_STATE = 0x0008;
        public const int TVIS_STATEIMAGEMASK = 0xF000;
        public const int TV_FIRST = 0x1100;
        public const int TVM_SETITEM = TV_FIRST + 63;

        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        public struct TVITEM
        {
            public int mask;
            public IntPtr hItem;
            public int state;
            public int stateMask;
            [MarshalAs(UnmanagedType.LPTStr)]
            public String lpszText;
            public int cchTextMax;
            public int iImage;
            public int iSelectedImage;
            public int cChildren;
            public IntPtr lParam;
        }

        public static void HideCheckBox(this TreeNode node)
        {
            TVITEM tvi = new TVITEM()
            {
                hItem = node.Handle,
                mask = TVIF_STATE,
                stateMask = TVIS_STATEIMAGEMASK,
                state = 0
            };
            IntPtr lparam = Marshal.AllocHGlobal(Marshal.SizeOf(tvi));
            Marshal.StructureToPtr(tvi, lparam, false);
            SendMessage(node.TreeView.Handle, TVM_SETITEM, IntPtr.Zero, lparam);
        }

        public static void ShowPlus(this TreeNode node)
        {
            TVITEM tvi = new TVITEM()
            {
                hItem = node.Handle,
                mask = TVIF_CHILDREN,
                cChildren = 1
            };
            IntPtr lparam = Marshal.AllocHGlobal(Marshal.SizeOf(tvi));
            Marshal.StructureToPtr(tvi, lparam, false);
            SendMessage(node.TreeView.Handle, TVM_SETITEM, IntPtr.Zero, lparam);
        }

        #endregion

        #region Hidden Nodes

        private static readonly Dictionary<TreeNode, TreeNode> hiddenNodes = new Dictionary<TreeNode, TreeNode>();

        public static bool IsHidden(this TreeNode node)
        {
            return hiddenNodes.ContainsKey(node);
        }

        public static IEnumerable<TreeNode> HiddenNodes(this TreeNode parent)
        {
            return hiddenNodes.Where(x => x.Value == parent).Select(x => x.Key);
        }

        public static void ShowAll(this TreeNode parent)
        {
            foreach (TreeNode node in parent.HiddenNodes().ToArray())
                node.Show();
        }

        public static void HideAll(this TreeNode parent)
        {
            foreach (TreeNode node in parent.Nodes.Cast<TreeNode>().ToArray())
                node.Hide();
        }

        public static void Show(this TreeNode node)
        {
            if (hiddenNodes.ContainsKey(node) == false)
                return;
            TreeNode parent = hiddenNodes[node];
            parent.Nodes.AddSorted(node);
            node.ShowPlus();
            hiddenNodes.Remove(node);
        }

        public static void Hide(this TreeNode node)
        {
            if (hiddenNodes.ContainsKey(node))
                return;
            TreeNode parent = node.Parent;
            node.Remove();
            hiddenNodes.Add(node, parent);
        }

        public static void AddSorted(this TreeNodeCollection nodes, TreeNode node)
        {
            if (nodes.Count == 0)
            {
                nodes.Add(node);
                return;
            }

            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                TreeNode node2 = nodes[i];
                int result = node.CompareTo(node2);
                if (result >= 0)
                {
                    nodes.Insert(i + 1, node);
                    return;
                }
            }

            nodes.Insert(0, node);
        }

        public static int CompareTo(this TreeNode node1, TreeNode node2)
        {
            if (node1 == null && node2 == null)
                return 0;

            if (node1 == null && node2 != null)
                return -1;

            if (node1 != null && node2 == null)
                return 1;

            return node1.ToString().CompareTo(node2.ToString());
        }

        #endregion
    }
}
