using System.Collections.Generic;

namespace Phase08.Interfaces
{
    public interface ISearcher<T>
    {
        HashSet<T> Search(string text);
    }

}