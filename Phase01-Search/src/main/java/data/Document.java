package data;

import lombok.Getter;

import java.io.IOException;
import java.net.URISyntaxException;
import java.util.ArrayList;

@Getter
public class Document {
    @Getter
    private static final String DATA_DIRECTORY = "src/main/resources/searchtools/EnglishData";
    private final String content;
    private final int id;

    public Document(String content, int id) {
        this.content = content;
        this.id = id;
    }

    public static ArrayList<Document> getDocuments(String directory) throws IOException, URISyntaxException {
        return DatabaseReader.getDocsInDirectory(directory);
    }
}
