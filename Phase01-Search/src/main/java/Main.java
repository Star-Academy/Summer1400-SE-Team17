import java.io.IOException;
import java.net.URISyntaxException;
import java.util.ArrayList;
import java.util.Scanner;

public class Main {
    public static void main(String[] args) throws IOException, URISyntaxException {
        Scanner scanner = new Scanner(System.in);
        while (true) {
            String command = scanner.nextLine().trim();
            ArrayList<Data> searchResult = InvertedIndex.search(command);
            for (int i = 0; i < searchResult.size(); i++) {
                if (i > 5) break;
                System.out.println(searchResult.get(i).getDocument().getContent());
                System.out.println("####################################################################");
            }
        }
    }
}
