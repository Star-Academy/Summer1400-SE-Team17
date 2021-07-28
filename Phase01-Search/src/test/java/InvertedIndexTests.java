import data.Data;
import lombok.SneakyThrows;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;
import searchtools.InvertedIndex;
import searchtools.Parser;
import searchtools.Searcher;

import java.lang.reflect.Field;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;

public class InvertedIndexTests {
    private final Searcher searcher = new Searcher(new InvertedIndex());
    @Test
    public void wholeProcessTest() {
        String command = "male friend old bone -sponge +humidity";
        int expectedResultSize = 2;
        int expectedResult = 57110;
        HashSet<Integer> searchResults = searcher.search(command);
        Assertions.assertEquals(expectedResultSize, searchResults.size());
        Assertions.assertTrue(searchResults.contains(expectedResult));
        System.out.println(Parser.stemWord("hi"));
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
        Field field = Searcher.class.getDeclaredField("dataBase");
        field.setAccessible(true);
        field.set(searcher , dataBase);
    }

}
