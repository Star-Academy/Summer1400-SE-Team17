using System.Collections.Generic;

namespace Phase08.Interfaces
{
    public interface IDatabase<TVALUE, TKey, TDOC>
    {
        public bool ContainsKey(TKey o);
        public HashSet<TVALUE> Get(TKey o);

        public void AddWord(TKey word);

        public void AddDocumentWord(TKey word, TDOC document);
        public void SaveData();
    }
}