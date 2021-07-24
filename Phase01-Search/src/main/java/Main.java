import java.io.IOException;
import java.net.URISyntaxException;
import java.util.ArrayList;

public class Main {
    public static void main(String[] args) throws IOException, URISyntaxException {
        ArrayList<Document> documents = FileReader.getDocsInDirectory("EnglishData");
        System.out.println(documents.get(0).getContent());
    }
}
