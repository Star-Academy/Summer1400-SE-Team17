import lombok.Getter;
import lombok.Setter;
import lombok.SneakyThrows;

import java.io.File;
import java.io.FileReader;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.ArrayList;

@Getter
@Setter
public class Data {
    private transient String word;
    private int indexDocument;
    private ArrayList<Integer> positions = new ArrayList<>();
    @SneakyThrows
    public Document getDocument() {
        String content = new String(Files.readAllBytes(Paths.get(getClass().getResource("EnglishData/" + indexDocument).toURI())));
        return new Document(content,indexDocument);
    }
    public Data(String word,int indexDocument) {
        this.word = word;
        this.indexDocument = indexDocument;
        positions = new ArrayList<>();
    }
    public Data() {

    }

    public Data(String word) {
        this.word = word;
        positions = new ArrayList<>();
    }

    public void addPosition(int position) {
        positions.add(position);
    }
}
