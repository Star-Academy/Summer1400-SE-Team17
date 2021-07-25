import lombok.SneakyThrows;

import java.io.IOException;
import java.io.InputStream;
import java.net.URISyntaxException;
import java.util.*;

public class Main {
    private static Scanner scanner;

    public static void main(String[] args) throws IOException, URISyntaxException {
        init();
        run();
    }

    private static void run() {
        while (true) {
            String command = scanner.nextLine().trim();
            HashSet<Integer> searchResult = InvertedIndex.search(command);
            int count = 0;
            for (int documentId : searchResult) {
                if (++count > 5) break;
                Data data = new Data();
                data.setIndexDocument(documentId);
                System.out.println(data.getDocument().getContent());
                System.out.println("#########################################################################################################################################################");
            }
        }
    }

    @SneakyThrows
    private static void init() {
        System.out.println("initializing database...");
        InvertedIndex.load();
        System.out.println("initializing finished...");
        scanner = new Scanner(System.in);
        scanner = scanner.skip(".*");
        scanner.next();
        scanner = new Scanner(System.in);
    }
}
