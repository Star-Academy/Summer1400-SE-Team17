import lombok.Getter;

import java.util.ArrayList;

@Getter
public class Document {

    private final String content;
    private final int id;
    public Document(String content , int id) {
        this.content = content;
        this.id = id;
    }

    public static ArrayList<Document> getDocuments() {
      return DatabaseReader.getDocsInDirectory("EnglishData");
    }
}
