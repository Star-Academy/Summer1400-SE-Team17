import lombok.Getter;
import lombok.Setter;

import java.util.ArrayList;

@Getter
@Setter
public class Data {
    private String word;
    private int indexDocument;
    private ArrayList<Integer> positions = new ArrayList<>();
    public Document getDocument() {
        return null;
    }
    public Data(String word,int indexDocument) {
        this.word = word;
        this.indexDocument = indexDocument;
        positions = new ArrayList<>();
    }

    public Data(String word) {
        this.word = word;
        positions = new ArrayList<>();
    }

    public void addPosition(int position) {
        positions.add(position);
    }
}
