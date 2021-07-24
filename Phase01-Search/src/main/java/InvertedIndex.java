import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;
import lombok.SneakyThrows;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.FileWriter;
import java.net.URL;
import java.util.ArrayList;
import java.util.HashMap;

public class InvertedIndex {
    public static HashMap<String, ArrayList<Data>> dataBase;
    private final static URL FILE = InvertedIndex.class.getResource("data.json");
    static {
        load();
    }

    @SneakyThrows
    private static void load() {
        Gson gson = new Gson();
        try {
            FileReader reader = new FileReader("main/resources/data.json");
            dataBase = gson.fromJson(reader,new TypeToken<HashMap<String, ArrayList<Data>>>(){}.getType());
            reader.close();
        } catch (FileNotFoundException e) {
            FileWriter writer = new FileWriter("src/main/resources/data.json");
            dataBase = getDocuments();
            gson.toJson(dataBase,writer);
            writer.close();
        }

    }

    private static HashMap<String,ArrayList<Data>> getDocuments() {
        HashMap<String,ArrayList<Data>> dataBase = new HashMap<>();
        for (Document document : Document.getDocuments()) {
            for (Data data : Parser.parseDocument(document)) {
                if (dataBase.get(data.getWord()) == null)
                    dataBase.put(data.getWord(), new ArrayList<>());
                dataBase.get(data.getWord()).add(data);
            }
        }
        return dataBase;
    }

    public static ArrayList<Data> search(String word) {
        System.out.println(dataBase.size());
        return dataBase.getOrDefault(word, new ArrayList<>());
    }

}
