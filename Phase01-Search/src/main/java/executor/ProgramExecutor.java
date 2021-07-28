package executor;

import data.Data;
import lombok.Getter;
import lombok.SneakyThrows;
import searchtools.InvertedIndex;
import searchtools.Searcher;

import java.io.IOException;
import java.net.URISyntaxException;
import java.util.HashSet;
import java.util.Scanner;

public class ProgramExecutor {
    @Getter
    private final static ProgramExecutor executor = new ProgramExecutor();

    private Searcher searcher;


    private ProgramExecutor() {
    }

    public void execute() throws IOException, URISyntaxException {
        init();
        run(getScanner());
    }

    @SneakyThrows
    private void run(Scanner scanner) {
        while (true) {
            String command = scanner.nextLine().trim();
            if (command.equals("--exit")) {
                return;
            }
            HashSet<Integer> searchResult = searcher.search(command);
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

    private void init() {
        System.out.println("initializing database...");
        searcher = new Searcher(new InvertedIndex());
        System.out.println("initializing finished...");
    }

    private Scanner getScanner() {
        return new Scanner(System.in);
    }
}
