import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;

import java.util.HashSet;

public class InvertedIndexTests {
    @Test
    public void searchTest() {
        String command = "male friend old bone -sponge +humidity";
        int expectedResultSize = 2;
        int expectedResult = 57110;
        InvertedIndex.load();
        HashSet<Integer> searchResults = InvertedIndex.search(command);
        Assertions.assertEquals(expectedResultSize , searchResults.size());
        Assertions.assertTrue(searchResults.contains(expectedResult));
    }

}
