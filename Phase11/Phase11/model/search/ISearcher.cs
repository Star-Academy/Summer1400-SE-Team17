using System.Collections.Generic;

namespace Phase11.model
{
    public interface ISearcher<T>
    {
        HashSet<T> Search(string text);
    }
}