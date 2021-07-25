
import lombok.SneakyThrows;

import java.io.File;
import java.io.IOException;
import java.net.URISyntaxException;
import java.net.URL;
import java.nio.file.Files;
import java.util.ArrayList;

public class DatabaseReader {
    @SneakyThrows
    public static ArrayList<Document> getDocsInDirectory(String directoryLocation) {
        ArrayList<Document> result = new ArrayList<>();
        File[] filesList = getFiles(directoryLocation);
        addDocumentsToResult(result, filesList);
        return result;
    }

    private static void addDocumentsToResult(ArrayList<Document> result, File[] filesList) throws IOException {
        for(File file : filesList) {
            String content = new String(Files.readAllBytes(file.toPath()));
            int id = Integer.parseInt(file.getName());
            Document document = new Document(content , id);
            result.add(document);
        }
    }

    private static File[] getFiles(String directoryLocation) throws URISyntaxException {
        URL url = DatabaseReader.class.getResource(directoryLocation);
        assert url != null;
        File directory = new File(url.toURI().getPath());
        File[] filesList = directory.listFiles();
        assert filesList != null;
        return filesList;
    }
}
