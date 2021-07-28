import lombok.Getter;

import java.io.IOException;
import java.net.URISyntaxException;
import java.util.ArrayList;

@Getter
public class Document {
    @Getter
    private static final String DATA_DIRECTORY = "EnglishData";
    private final String content;
    private final int id;

    public Document(String content, int id) {
        this.content = content;
        this.id = id;
    }

    public static ArrayList<Document> getDocuments() throws IOException, URISyntaxException {
        return DatabaseReader.getDocsInDirectory(DATA_DIRECTORY);
    }
}
