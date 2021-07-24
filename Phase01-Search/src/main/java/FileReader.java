
import lombok.SneakyThrows;

import java.io.File;
import java.io.IOException;
import java.net.URISyntaxException;
import java.nio.file.Files;
import java.util.ArrayList;

public class FileReader {
    @SneakyThrows
    public static ArrayList<Document> getDocsInDirectory(String directoryLocation) {
        ArrayList<Document> result = new ArrayList<>();
        File directory = new File(FileReader.class.getResource(directoryLocation).toURI().getPath());
        File[] filesList = directory.listFiles();
        assert filesList != null;
        for(File file : filesList) {
            String content = new String(Files.readAllBytes(file.toPath()));
            int id = Integer.parseInt(file.getName());
            Document document = new Document(content , id);
            result.add(document);
        }
        return result;
    }
}
