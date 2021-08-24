using System.Collections.Generic;
using System.IO;
using Phase08.Model;
using Phase08.Interfaces;


namespace Phase08
{
    public class DictionaryLoader : IFileLoader<HashSet<Document>>
    {
        public HashSet<Document> Load(string path)
        {
            HashSet<Document> result = new HashSet<Document>();
            foreach (var fileRelativePath in Directory.EnumerateFiles(path))
            {
                string fileName = fileRelativePath.Substring(fileRelativePath.LastIndexOf('/') + 1);
                string fileContent = File.ReadAllText(fileRelativePath);
                result.Add(new Document(int.Parse(fileName), fileContent));
            }

            return result;
        }
    }
}