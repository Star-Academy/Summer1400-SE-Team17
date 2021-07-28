package data;

import lombok.Getter;
import lombok.Setter;

import java.io.IOException;
import java.net.URISyntaxException;
import java.net.URL;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.Arrays;

@Getter
@Setter
public class Data {

    private ArrayList<Integer> positions = new ArrayList<>();
    private transient String word;
    private int indexDocument;

    public Data(String word, ArrayList<Integer> positions, int indexDocument) {
        this.word = word;
        this.positions = positions;
        this.indexDocument = indexDocument;
    }

    public Data(String word, int indexDocument) {
        this.word = word;
        this.indexDocument = indexDocument;
    }

    public Data(String word) {
        this.word = word;
    }

    public Data() {

    }


    public Document getDocument() throws IOException, URISyntaxException {
        String content = getContent();
        return new Document(content, indexDocument);
    }


    private String getContent() throws URISyntaxException, IOException {
        URL url = getClass().getResource(Document.getDATA_DIRECTORY() + "/" + indexDocument);
        assert url != null;
        Path path = Paths.get(url.toURI());
        return new String(Files.readAllBytes(path), StandardCharsets.UTF_8);
    }


    public void addPosition(int position) {
        positions.add(position);
    }

    @Override
    public String toString() {
        return "Word: " + word + "| @data.Document: " + indexDocument + "| Had accord in: " +
                Arrays.toString(positions.toArray());
    }
}
