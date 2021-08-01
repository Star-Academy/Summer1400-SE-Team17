package executor;

import data.Data;
import lombok.Getter;
import lombok.Setter;
import searchtools.InvertedIndex;
import searchtools.SearchEngine;
import searchtools.Searcher;

import java.io.IOException;
import java.net.URISyntaxException;
import java.util.ArrayList;
import java.util.Scanner;
import java.util.Set;

@Setter
public class ProgramExecutor {
    private Printer printer = System.out::println;
    private Reader reader;

    @Getter
    private final static ProgramExecutor executor = new ProgramExecutor();

    private Searcher<Integer> searcher;

    {
        reader = new Reader() {
            private final Scanner scanner = new Scanner(System.in);

            @Override
            public String read() {
                return scanner.nextLine();
            }
        };
    }


    private ProgramExecutor() {
        
    }

    public void execute() throws IOException, URISyntaxException {
        init();
        run();
    }

    private void run() throws IOException, URISyntaxException {
        while (true) {
            String command = reader.read();
            if (command.equals("--exit")) {
                return;
            }
            Set<Integer> searchResult = searcher.search(command);
            printResult(searchResult);
        }
    }

    private void printResult(Set<Integer> searchResult) throws IOException, URISyntaxException {
        int count = 0;
        for (int documentId : searchResult) {
            if (++count > 5) {
                break;
            }
            Data data = new Data();
            data.setIndexDocument(documentId);
            printer.print(data.getDocument().getContent());
            printer.print("#######################################" +
                    "################################################################" +
                    "##################################################");
        }
    }

    private void init() {
        printer.print("initializing database...");
        searcher = new SearchEngine(new InvertedIndex());
        printer.print("initializing finished...");
    }
    public static int abc() {
        return 0;
    }

}
