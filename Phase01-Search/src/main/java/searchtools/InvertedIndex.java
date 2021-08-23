package searchtools;

import com.google.gson.reflect.TypeToken;
import data.Data;
import data.Document;
import data.JsonSerializer;
import lombok.Getter;
import lombok.SneakyThrows;

import java.io.*;
import java.lang.reflect.Type;
import java.net.URISyntaxException;
import java.util.ArrayList;
import java.util.HashMap;


public class InvertedIndex {
    @Getter
    private HashMap<String, ArrayList<Data>> dataBase;
    public static final String DATA_JSON = "src/main/resources/data.json";

    public InvertedIndex() {
        this.load(DATA_JSON);
    }

    @SneakyThrows
    public void load(String dataAddr) {
        if(doesFileExist(dataAddr)) {
            loadDataFromJson(dataAddr);
        } else {
            loadDataFromFiles(dataAddr);
        }
    }

    private void loadDataFromJson(String dataAddr) throws IOException {
        FileReader reader = new FileReader(dataAddr);
        try {
            Type mapType = new TypeToken<HashMap<String, ArrayList<Data>>>() {
            }.getType();
            dataBase = JsonSerializer.fromJson(reader, mapType);
        } catch (Exception e) {

        } finally {
            reader.close();
        }
    }

    private boolean doesFileExist(String dataAddr) {
        File f = new File(dataAddr);
        return f.exists() && f.isFile();
    }

    private void loadDataFromFiles(String dataAddr) throws IOException, URISyntaxException {
        FileWriter writer = new FileWriter(dataAddr);
        dataBase = getDocuments();
        JsonSerializer.toJson(dataBase, writer);
        writer.close();
    }

    private HashMap<String, ArrayList<Data>> getDocuments() throws IOException, URISyntaxException {
        HashMap<String, ArrayList<Data>> dataBase = new HashMap<>();
        for (Document document : Document.getDocuments(Document.getDATA_DIRECTORY())) {
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
