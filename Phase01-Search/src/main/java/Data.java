import lombok.Getter;
import lombok.Setter;

import java.util.ArrayList;

@Getter
@Setter
public class Data {
    private String word;
    private int indexDocument;
    private ArrayList<Integer> positions;
    public Document getDocument() {
        return null;
    }
}