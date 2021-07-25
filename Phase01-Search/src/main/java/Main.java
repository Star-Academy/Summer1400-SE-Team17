import java.io.IOException;
import java.io.InputStream;
import java.net.URISyntaxException;
import java.util.*;

public class Main {
    public static void main(String[] args) throws IOException, URISyntaxException {
        closeInputStream();
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
                System.out.println("######################################################################################################################################################################################");
            }
        }
    }

    private static void closeInputStream() {
       InputStream stream = System.in;
       System.setIn(new InputStream() {
           @Override
           public int read() throws IOException {
               return 0;
           }
       });
       InvertedIndex invertedIndex = new InvertedIndex();
        TimerTask task = new TimerTask() {
            @Override
            public void run() {
                System.setIn(stream);
            }
        };
        Timer timer = new Timer();
        timer.schedule(task,3000);
    }
}
