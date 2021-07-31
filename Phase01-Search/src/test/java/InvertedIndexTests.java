import data.Data;
import lombok.SneakyThrows;
import org.junit.jupiter.api.*;
import searchtools.InvertedIndex;
import searchtools.SearchEngine;


import java.io.File;
import java.lang.reflect.Field;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;

import static org.mockito.Mockito.doAnswer;
import static org.mockito.Mockito.spy;

public class InvertedIndexTests {
    private static SearchEngine searcher;
    public static final String MOCK_DATA_JSON = "src/main/resources/mock-data.json";

    @BeforeEach
    public void init() {
        InvertedIndex mockIndex = spy(InvertedIndex.class);
        doAnswer(invocationOnMock -> {
            mockIndex.load(MOCK_DATA_JSON);
            return null;
        }).when(mockIndex).load(InvertedIndex.DATA_JSON);
        mockIndex.load(InvertedIndex.DATA_JSON);
        searcher = new SearchEngine(mockIndex);
    }

    @Test
    public void wholeProcessTest() {
        String command = "male friend old bone -sponge +humidity";
        int expectedResultSize = 2;
        int expectedResult = 57110;
        HashSet<Integer> searchResults = searcher.search(command);
        Assertions.assertEquals(expectedResultSize, searchResults.size());
        Assertions.assertTrue(searchResults.contains(expectedResult));
    }

    @Test
    @SneakyThrows
    public void whenDataBaseIsolatedTest() {
        String command = "+car -dog doctor";
        int expectedResultSize = 2;
        HashMap<String, ArrayList<Data>> dataBase = getFirstSample();
        mockDataBase(dataBase);
        HashSet<Integer> results = searcher.search(command);
        Assertions.assertEquals(expectedResultSize , results.size());
        Assertions.assertTrue(results.contains(2));
        Assertions.assertTrue(results.contains(3));
    }

    @AfterAll
    static void deleteMockData() {
        new File(MOCK_DATA_JSON).delete();
    }


    private HashMap<String, ArrayList<Data>> getFirstSample() {
        String firstContent = "car dog doctor!";
        String secondContent = "car car doctor?";
        String thirdContent = "doctor !!! ff !";
        ArrayList<Data> carData = new ArrayList<>();
        carData.add(new Data("car" , 1));
        carData.add(new Data("car" , 2));
        ArrayList<Data> dogData = new ArrayList<>();
        dogData.add(new Data("dog" , 1));
        ArrayList<Data> doctorData = new ArrayList<>();
        doctorData.add(new Data("doctor" , 1));
        doctorData.add(new Data("doctor" , 2));
        doctorData.add(new Data("doctor" , 3));
        HashMap<String, ArrayList<Data>> dataBase = new HashMap<>();
        dataBase.put("car" , carData);
        dataBase.put("dog" , dogData);
        dataBase.put("doctor" , doctorData);
        return dataBase;
    }

    private void mockDataBase(HashMap<String, ArrayList<Data>> dataBase) throws NoSuchFieldException, IllegalAccessException {
        Field field = SearchEngine.class.getDeclaredField("dataBase");
        field.setAccessible(true);
        field.set(searcher , dataBase);
    }

}
