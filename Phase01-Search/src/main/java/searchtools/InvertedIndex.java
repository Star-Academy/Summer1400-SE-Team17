package searchtools;

import com.google.gson.reflect.TypeToken;
import data.Data;
import data.Document;
import data.JsonSerializer;
import lombok.Getter;
import lombok.SneakyThrows;

import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.lang.reflect.Type;
import java.net.URISyntaxException;
import java.util.ArrayList;
import java.util.HashMap;


public class InvertedIndex {
    @Getter
    private HashMap<String, ArrayList<Data>> dataBase;
    private static final String INVERTED_INDEX_DIRECTORY = "src/main/resources/data.json";

    public InvertedIndex() {
        this.load();
    }

    @SneakyThrows
    public void load() {
        try {
            loadDataFromJson();
        } catch (FileNotFoundException e) {
            loadDataFromFiles();
        }
    }

    private void loadDataFromJson() throws IOException {
        FileReader reader = new FileReader(INVERTED_INDEX_DIRECTORY);
        Type mapType = new TypeToken<HashMap<String, ArrayList<Data>>>() {
        }.getType();
        dataBase = JsonSerializer.fromJson(reader, mapType);
        reader.close();

    }

    private void loadDataFromFiles() throws IOException, URISyntaxException {
        FileWriter writer = new FileWriter(INVERTED_INDEX_DIRECTORY);
        dataBase = getDocuments();
        JsonSerializer.toJson(dataBase, writer);
        writer.close();
    }

    private HashMap<String, ArrayList<Data>> getDocuments() throws IOException, URISyntaxException {
        HashMap<String, ArrayList<Data>> dataBase = new HashMap<>();
        for (Document document : Document.getDocuments()) {
            getDataFromParsedDocument(dataBase, document);
        }
        return dataBase;
    }

    private void getDataFromParsedDocument(HashMap<String, ArrayList<Data>> dataBase, Document document) {
        for (Data data : Parser.parseDocument(document)) {
            dataBase.computeIfAbsent(data.getWord(), k -> new ArrayList<>());
            dataBase.get(data.getWord()).add(data);
        }
    }

}
