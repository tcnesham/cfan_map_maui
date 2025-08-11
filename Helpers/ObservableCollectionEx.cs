using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CFAN.SchoolMap.Helpers
{
    public static class ObservableCollectionEx
    {
        public static void AddRange<T>(this ObservableCollection<T> co, IEnumerable<T> items)
        {
            foreach (var i in items)
            {
                co.Add(i);
            }
        }
    }
}
