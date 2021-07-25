import lombok.Getter;
import lombok.Setter;
import lombok.SneakyThrows;

import java.net.URL;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.ArrayList;

@Getter
@Setter
public class Data {
    private ArrayList<Integer> positions = new ArrayList<>();
    private transient String word;
    private int indexDocument;

    public Data(String word, int indexDocument) {
        this.word = word;
        this.indexDocument = indexDocument;
        positions = new ArrayList<>();
    }

    public Data(String word) {
        this.word = word;
        positions = new ArrayList<>();
    }

    public Data() {

    }


    @SneakyThrows
    public Document getDocument() {
        String content = getContent();
        return new Document(content, indexDocument);
    }

    @SneakyThrows
    private String getContent() {
        URL url = getClass().getResource("EnglishData/" + indexDocument);
        assert url != null;
        return new String(Files.readAllBytes(Paths.get(url.toURI())));
    }


    public void addPosition(int position) {
        positions.add(position);
    }
}
