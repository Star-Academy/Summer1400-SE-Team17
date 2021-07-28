import data.Data;
import data.Document;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;
import searchtools.Parser;

import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.util.Arrays;
import java.util.Collection;
import java.util.HashSet;

public class ParserTest {
    @Test
    public void modifySentenceShouldWork() {
        final String test1 = "    hElLo AlI ";
        final String test2 = "HelLo, mmd?";
        final String test3 = "HelLo, mmd?Are you Okb??!";
        final String test4 = "HelLo, mmd?Are y$$%\u2003ou \u2403Okb??!";
        StringRunnable<String> modifySentence = (s) -> {
            try {
                Method method = Parser.class.getDeclaredMethod("modifySentence", String.class);
                method.setAccessible(true);
                return (String) method.invoke(null, s);
            } catch (NoSuchMethodException | IllegalAccessException | InvocationTargetException ignored) {
            }
            return "";
        };

        Assertions.assertEquals(modifySentence.run(test1), "hello ali");
        Assertions.assertEquals(modifySentence.run(test2), "hello  mmd");
        Assertions.assertEquals(modifySentence.run(test3), "hello  mmd are you okb");
        Assertions.assertEquals(modifySentence.run(test4), "hello  mmd are y    ou  okb");
    }

    @Test
    public void stemWordTest() {
        String gerund = "dancing";
        String name = "John";
        String nameWithED = "Ned";
        String noun = "astrology";
        String simplePast = "ran";
        String pastPerfect = "ate";
        String compoundVerb = "Fill In";
        String verbPPTense = "given";
        StringRunnable<String> stemWord = Parser::stemWord;
        Assertions.assertEquals(stemWord.run(gerund), "dance");
        Assertions.assertEquals(stemWord.run(name), "John");
        Assertions.assertEquals(stemWord.run(nameWithED), nameWithED);
        Assertions.assertEquals(stemWord.run(noun), noun);
        Assertions.assertEquals(stemWord.run(simplePast), "run");
        Assertions.assertEquals(stemWord.run(pastPerfect), "eat");
        Assertions.assertEquals(stemWord.run(compoundVerb), "Fill In");
        Assertions.assertEquals(stemWord.run(verbPPTense), "give");
    }

    @Test
    public void parseSentenceTest() {
        String testSentence = "Hi, his dog had ruined my beloved dolls then i have killed his dog and send the dogs image to BlahBlah@Gamil.com.";
        StringRunnable<Collection<Data>> parseSentence = Parser::parseSentence;
        String[] results = {
                "Word: com| @data.Document: 0| Had accord in: [22]",
                "Word: image| @data.Document: 0| Had accord in: [18]",
                "Word: blahblah| @data.Document: 0| Had accord in: [20]",
                "Word: doll| @data.Document: 0| Had accord in: [7]",
                "Word: then| @data.Document: 0| Had accord in: [8]",
                "Word: kill| @data.Document: 0| Had accord in: [11]",
                "Word: beloved| @data.Document: 0| Had accord in: [6]",
                "Word: gamil| @data.Document: 0| Had accord in: [21]",
                "Word: and| @data.Document: 0| Had accord in: [14]",
                "Word: have| @data.Document: 0| Had accord in: [3, 10]",
                "Word: dog| @data.Document: 0| Had accord in: [2, 13, 17]",
                "Word: ruin| @data.Document: 0| Had accord in: [4]",
                "Word: send| @data.Document: 0| Had accord in: [15]"
        };
        HashSet<String> resultSet = new HashSet<>(Arrays.asList(results));
        for (Data data : parseSentence.run(testSentence)) Assertions.assertTrue(resultSet.contains(data.toString()));
    }

    @Test
    public void parseDocumentTest() {
        Document document = new Document("salam salam. hello word nigga, iam doing fine. go go go. ha ha ha.",1);
        Collection<Data> results = Parser.parseDocument(document);
        String[] resultSetContent = {
                "Word: fine| @data.Document: 1| Had accord in: [7]",
                "Word: salam| @data.Document: 1| Had accord in: [0, 1]",
                "Word: go| @data.Document: 1| Had accord in: [7, 8, 9]",
                "Word: nigga| @data.Document: 1| Had accord in: [4]",
                "Word: ha| @data.Document: 1| Had accord in: [10, 11, 12]",
                "Word: hello| @data.Document: 1| Had accord in: [2]",
                "Word: do| @data.Document: 1| Had accord in: [6]",
                "Word: word| @data.Document: 1| Had accord in: [3]"
        };
        HashSet<String> resultSet = new HashSet<>(Arrays.asList(resultSetContent));
        for (Data data : results) Assertions.assertTrue(resultSet.contains(data.toString()));
    }

    private interface StringRunnable<T> {
        T run(String s);
    }

}
