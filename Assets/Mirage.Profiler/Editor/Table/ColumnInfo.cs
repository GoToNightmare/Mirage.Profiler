using System;
using Mirage.NetworkProfiler.ModuleGUI.Messages;

namespace Mirage.NetworkProfiler.ModuleGUI.UITable
{
    internal class ColumnInfo
    {
        private readonly Func<MessageInfo, string> _textGetter;

        public string Header { get; private set; }
        public int Width { get; private set; }
        public bool AllowSort { get; private set; }
        public Func<Group, Group, int> SortGroup { get; private set; }
        public Func<MessageInfo, MessageInfo, int> SortMessages { get; private set; }

        public ColumnInfo(string header, int width, Func<MessageInfo, string> textGetter)
        {
            Header = header;
            Width = width;
            _textGetter = textGetter;
        }

        /// <summary>
        /// Enables sorting for column. If sort functions are null they will use default sort from <see cref="GroupSorter"/>
        /// </summary>
        /// <param name="sortGroup"></param>
        /// <param name="sortMessages"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddSort(Func<Group, Group, int> sortGroup, Func<MessageInfo, MessageInfo, int> sortMessages)
        {
            AllowSort = true;
            SortGroup = sortGroup ?? GroupSorter.DefaultGroupSort;
            SortMessages = sortMessages ?? GroupSorter.DefaultMessageSort;
        }

        /// <summary>
        /// Enables sorting for column. If sort functions are null they will use default sort from <see cref="GroupSorter"/>
        /// <para>Uses member getting to sort via that member</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="groupGetter"></param>
        /// <param name="messageGetter"></param>
        public void AddSort<T>(Func<Group, T> groupGetter, Func<MessageInfo, T> messageGetter) where T : IComparable<T>
        {
            Func<Group, Group, int> sortGroup = groupGetter != null
                ? (x, y) => GroupSorter.Compare(x, y, groupGetter)
                : null;

            Func<MessageInfo, MessageInfo, int> sortMessages = messageGetter != null
                ? (x, y) => GroupSorter.Compare(x, y, messageGetter)
                : null;

            AddSort(sortGroup, sortMessages);
        }

        internal string GetText(MessageInfo info)
        {
            return _textGetter.Invoke(info);
        }
    }
}
