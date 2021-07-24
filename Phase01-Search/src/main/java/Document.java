import lombok.Getter;

import java.util.ArrayList;

@Getter
public class Document {
    @Getter
    private static ArrayList<Document> documents = FileReader.getDocsInDirectory("EnglishData");
    private final String content;
    private final int id;
    public Document(String content , int id) {
        this.content = content;
        this.id = id;
    }
}
