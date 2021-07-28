import com.google.gson.reflect.TypeToken;
import lombok.SneakyThrows;

import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.net.URISyntaxException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;


public class InvertedIndex {
    private static HashMap<String, ArrayList<Data>> dataBase;
    private static final String INVERTED_INDEX_DIRECTORY = "src/main/resources/data.json";


    @SneakyThrows
    public static void load() {
        try {
            loadDataFromJson();
        } catch (FileNotFoundException e) {
            loadDataFromFiles();
        }
    }

    private static void loadDataFromJson() throws IOException {
        FileReader reader = new FileReader(INVERTED_INDEX_DIRECTORY);
        dataBase = (HashMap<String, ArrayList<Data>>) JsonSerializer.fromJson(reader, new TypeToken<HashMap<String, ArrayList<Data>>>() {
        }.getType());
        reader.close();
    }

    private static void loadDataFromFiles() throws IOException, URISyntaxException {
        FileWriter writer = new FileWriter(INVERTED_INDEX_DIRECTORY);
        dataBase = getDocuments();
        JsonSerializer.toJson(dataBase, writer);
        writer.close();
    }

    private static HashMap<String, ArrayList<Data>> getDocuments() throws IOException, URISyntaxException {
        HashMap<String, ArrayList<Data>> dataBase = new HashMap<>();
        for (Document document : Document.getDocuments()) {
            getDataFromParsedDocument(dataBase, document);
        }
        return dataBase;
    }

    private static void getDataFromParsedDocument(HashMap<String, ArrayList<Data>> dataBase, Document document) {
        for (Data data : Parser.parseDocument(document)) {
            dataBase.computeIfAbsent(data.getWord(), k -> new ArrayList<>());
            dataBase.get(data.getWord()).add(data);
        }
    }

    public static HashSet<Integer> search(String command) {
        String[] words = getWords(command);
        HashSet<Integer> result = new HashSet<>();
        stemWords(words);
        addEssentialWordsToResult(words, result);
        addWillingWordsToResult(words, result);
        removeBannedWordsFromResult(words, result);
        return result;
    }

    private static void removeBannedWordsFromResult(String[] words, HashSet<Integer> result) {
        for (String word : words) {
            if (word.charAt(0) == '-') {
                removeWordFromResult(result, word.substring(1));
            }
        }
    }

    private static void removeWordFromResult(HashSet<Integer> result, String word) {
        for (Data data : dataBase.getOrDefault(word , new ArrayList<>())) {
            result.remove(data.getIndexDocument());
        }
    }

    private static void addWillingWordsToResult(String[] words, HashSet<Integer> result) {
        for (String word : words) {
            if (word.charAt(0) == '+') {
                addWordToResult(result, word.substring(1));
            }
        }
    }

    private static void addWordToResult(HashSet<Integer> result, String word) {
        for (Data data : dataBase.getOrDefault(word, new ArrayList<>())) {
            result.add(data.getIndexDocument());
        }
    }

    private static void addEssentialWordsToResult(String[] words, HashSet<Integer> result) {
        boolean isFirstEssential = true;
        for (String word : words) {
            if (word.charAt(0) == '+' || word.charAt(0) == '-') {
                continue;
            }
            HashSet<Integer> documentsWithWord = new HashSet<>();
            addWordToResult(documentsWithWord, word);
            if (isFirstEssential) {
                result.addAll(documentsWithWord);
                isFirstEssential = false;
            } else {
                result.retainAll(documentsWithWord);
            }
        }
    }

    private static void stemWords(String[] words) {
        for (int i = 0; i < words.length; ++i) {
            if (words[i].charAt(0) == '+' || words[i].charAt(0) == '-') {
                words[i] = words[i].charAt(0) + Parser.stemWord(words[i].substring(1));
            } else {
                words[i] = Parser.stemWord(words[i]);
            }
        }
    }

    private static String[] getWords(String command) {
        return command.trim().toLowerCase().split(" ");
    }

}
