import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;

import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.util.ArrayList;
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
        String simpleSentence = "Hi, Im home.";
        String complexSentenceWithExtraDot = "Hi, my email address is BlahBlah@Gamil.com .";
        String complexSentenceWithExtraPunctuation = "Hi, what is John's email address?!?!?!?!";
        String complexSentenceWithPronouns = "Hi, his dog had ruined my beloved dolls then i have killed his dog.";
        StringRunnable<Collection<Data>> parseSentence = Parser::parseSentence;
        
    }

    private interface StringRunnable<T> {
        T run(String s);
    }

    private static class DataMock extends Data {

        public DataMock (String word,int index) {
            super(word,index);
        }

        @Override
        public boolean equals(Object o) {
            if (!(o instanceof Data)) return false;
            Data data = (Data) o;
            return data.getWord().equals(this.getWord()) && data.getIndexDocument() == this.getIndexDocument();
        }
    }
}
