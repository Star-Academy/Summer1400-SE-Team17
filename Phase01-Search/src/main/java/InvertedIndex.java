import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;
import lombok.SneakyThrows;

import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.FileWriter;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;

public class InvertedIndex {
    public static HashMap<String, ArrayList<Data>> dataBase;


    @SneakyThrows
    public static void load() {
        Gson gson = new Gson();
        try {
            FileReader reader = new FileReader("main/resources/data.json");
            dataBase = gson.fromJson(reader, new TypeToken<HashMap<String, ArrayList<Data>>>() {
            }.getType());
            reader.close();
        } catch (FileNotFoundException e) {
            FileWriter writer = new FileWriter("src/main/resources/data.json");
            dataBase = getDocuments();
            gson.toJson(dataBase, writer);
            writer.close();
        }

    }

    private static HashMap<String, ArrayList<Data>> getDocuments() {
        HashMap<String, ArrayList<Data>> dataBase = new HashMap<>();
        for (Document document : Document.getDocuments()) {
            for (Data data : Parser.parseDocument(document)) {
                dataBase.computeIfAbsent(data.getWord(), k -> new ArrayList<>());
                dataBase.get(data.getWord()).add(data);
            }
        }
        return dataBase;
    }

    public static HashSet<Integer> search(String command) {
        String[] words = command.trim().toLowerCase().split(" ");
        HashSet<Integer> result = new HashSet<>();
        for (int i = 0; i < words.length; ++i) {
            if(words[i].charAt(0) == '+' || words[i].charAt(0) == '-')
                words[i] = words[i].charAt(0) + Parser.stemWord(words[i].substring(1));
            else
                words[i] = Parser.stemWord(words[i]);
        }
        for (int i = 0; i < words.length; ++i) {
            System.out.println(words[i]);
            if(words[i].charAt(0) == '+' || words[i].charAt(0) == '-')
                continue ;
            String word = words[i];
            HashSet<Integer> documentsWithWord = new HashSet<>();
            for (Data data : dataBase.getOrDefault(word, new ArrayList<>()))
                documentsWithWord.add(data.getIndexDocument());
            if (i == 0)
                result = documentsWithWord;
            else
                result.retainAll(documentsWithWord);
        }
        for (String word : words)
            if (word.charAt(0) == '+')
                for (Data data : dataBase.getOrDefault(word.substring(1), new ArrayList<>()))
                    result.add(data.getIndexDocument());
        for (String word : words)
            if (word.charAt(0) == '-')
                for (Data data : dataBase.getOrDefault(word.substring(1), new ArrayList<>()))
                    result.remove(data.getIndexDocument());
        return result;
    }
}
