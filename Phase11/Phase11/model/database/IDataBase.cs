using System.Collections.Generic;

namespace Phase11.model.database
{
    public interface IDataBase<TKey,Tvalue,Tdocument>
    {
        public bool ContainsKey(Tvalue o);
        public HashSet<TKey> Get(Tvalue o);

        public void AddWord(Tvalue word);

        public void AddDocumentWord(Tvalue word, Tdocument document);
        public void SaveData();
    }
}