import java.io.IOException;
import java.net.URISyntaxException;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.Scanner;

public class Main {
    public static void main(String[] args) throws IOException, URISyntaxException {
        Scanner scanner = new Scanner(System.in);
        while (true) {
            String command = scanner.nextLine().trim();
            HashSet<Integer> searchResult = InvertedIndex.search(command);
            int count = 0;
            for(int documentId : searchResult){
                if (++ count > 5) break;
                Data data = new Data();
                data.setIndexDocument(documentId);
                System.out.println(data.getDocument().getContent());
                System.out.println("####################################################################");
            }
        }
    }
}
