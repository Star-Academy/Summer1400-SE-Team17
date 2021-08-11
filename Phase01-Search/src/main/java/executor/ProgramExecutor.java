package executor;

import data.Data;
import lombok.Getter;
import lombok.Setter;
import searchtools.InvertedIndex;
import searchtools.SearchEngine;
import searchtools.Searcher;

import java.io.IOException;
import java.net.URISyntaxException;
import java.util.Scanner;
import java.util.Set;

@Setter
public class ProgramExecutor {
    private Printer printer = System.out::println;
    private Reader reader;
    private ContentGetter<String, Integer> contentGetter;

    @Getter
    private final static ProgramExecutor executor = new ProgramExecutor();

    private Searcher<Integer> searcher;

    {
        reader = getDefaultReader();
        contentGetter = getDefaultContentGetter();
        searcher = getDefaultSearcher();
    }


    private ProgramExecutor() {
    }


    public void execute() {
        run();
    }


    private void run(){
        while (true) {
            String command;
            command = reader.read();

            if (command.equals("--exit")) {
                return;
            }

            Set<Integer> searchResult = searcher.search(command);

            printResult(searchResult);

        }
    }

    private void printResult(Set<Integer> searchResult) {
        int count = 0;
        for (int documentId : searchResult) {
            if (++count > 5) return;
            printContent(documentId);
        }
    }

    private void printContent(int docIndex) {
        printer.print(contentGetter.getContent(docIndex));
        printer.print("#######################################" +
                "################################################################" +
                "##################################################");
    }


    private Reader getDefaultReader() {
        return new Reader() {
            private final Scanner scanner = new Scanner(System.in);

            @Override
            public String read() {
                return scanner.nextLine();
            }
        };
    }

    private ContentGetter<String, Integer> getDefaultContentGetter() {
        return (integer) -> {
            try {
                return Data.getDocument(integer).getContent();
            } catch (IOException | URISyntaxException e) {
                return null;
            }
        };
    }
    

    private Searcher<Integer> getDefaultSearcher() {
        printer.print("initializing database...");
        Searcher<Integer> searcher = new SearchEngine(new InvertedIndex());
        printer.print("initializing finished...");
        return searcher;
    }

}
