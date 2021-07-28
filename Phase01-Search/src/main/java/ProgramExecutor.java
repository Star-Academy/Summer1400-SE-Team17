import lombok.SneakyThrows;

import java.io.IOException;
import java.net.URISyntaxException;
import java.util.HashSet;
import java.util.Scanner;

public class ProgramExecutor {
    public static void execute() throws IOException, URISyntaxException {
        init();
        run(getScanner());
    }

    @SneakyThrows
    private static void run(Scanner scanner) {
        while (true) {
            String command = scanner.nextLine().trim();
            if (command.equals("--exit")) {
                return;
            }
            HashSet<Integer> searchResult = InvertedIndex.search(command);
            int count = 0;
            for (int documentId : searchResult) {
                if (++count > 5) {
                    break;
                }
                Data data = new Data();
                data.setIndexDocument(documentId);
                System.out.println(data.getDocument().getContent());
                System.out.println("#######################################" +
                        "################################################################" +
                        "##################################################");
            }
        }
    }

    private static void init() {
        System.out.println("initializing database...");
        InvertedIndex.load();
        System.out.println("initializing finished...");
    }

    private static Scanner getScanner() {
        Scanner scanner = new Scanner(System.in);
        if (scanner.hasNextLine()) {
            scanner = scanner.skip(".*");
            scanner.next();
            scanner = new Scanner(System.in);
        }
        return scanner;
    }
}
