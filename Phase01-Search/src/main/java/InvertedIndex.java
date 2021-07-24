import java.util.ArrayList;
import java.util.HashMap;

public class InvertedIndex {
    public static HashMap<String, ArrayList<Data>> dataBase;

    static {
        addDocument();
    }

    private static void addDocument() {
        for (Document document : Document.getDocuments()) {
            for (Data data : Parser.parseDocument(document)) {
                if (dataBase.get(data.getWord()) == null)
                    dataBase.put(data.getWord(), new ArrayList<>());
                dataBase.get(data.getWord()).add(data);
            }
        }
    }

    public static ArrayList<Data> search(String word) {
        return dataBase.getOrDefault(word, new ArrayList<>());
    }

}
