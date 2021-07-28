package data;

import lombok.SneakyThrows;

import java.io.File;
import java.io.IOException;
import java.net.MalformedURLException;
import java.net.URISyntaxException;
import java.net.URL;
import java.nio.file.Files;
import java.util.ArrayList;

public class DatabaseReader {

    public static ArrayList<Document> getDocsInDirectory(String directoryLocation) throws URISyntaxException, IOException {
        ArrayList<Document> result = new ArrayList<>();
        File[] filesList = getFiles(directoryLocation);
        addDocumentsToResult(result, filesList);
        return result;
    }

    private static void addDocumentsToResult(ArrayList<Document> result, File[] filesList) throws IOException {
        for (File file : filesList) {
            Document document = createDocument(file);
            result.add(document);
        }
    }

    private static Document createDocument(File file) throws IOException {
        String content = new String(Files.readAllBytes(file.toPath()));
        int id = Integer.parseInt(file.getName());
        Document document = new Document(content, id);
        return document;
    }

    private static File[] getFiles(String directoryLocation) throws URISyntaxException, MalformedURLException {
        URL url = new File(directoryLocation).toURI().toURL();
        File directory = new File(url.toURI().getPath());
        File[] filesList = directory.listFiles();
        assert filesList != null;
        return filesList;
    }
}
