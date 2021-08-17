using System.Collections.Generic;

namespace Phase05
{
    public interface IReader
    {
        string Read();
    }

    public interface ISearcher<T>
    {
        HashSet<T> Search(string text);
    }

    public interface IPrinter
    {
        void Print(string content);
    }

    public interface IParser<T>
    {
        T Parse(string text);
    }

    public interface IFileLoader<T>
    {
        T Load(string path);
    }

    public interface IDataBase<T,K,S>
    {
        public bool ContainsKey(K o);
        public HashSet<T> Get(K o);

        public void AddWord(K word);

        public void AddDocumentWord(K word, S document);
        public void SaveData();
    }
    
}