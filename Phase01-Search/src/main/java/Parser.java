import opennlp.tools.lemmatizer.DictionaryLemmatizer;
import opennlp.tools.postag.POSModel;
import opennlp.tools.postag.POSTaggerME;
import opennlp.tools.sentdetect.SentenceDetectorME;
import opennlp.tools.sentdetect.SentenceModel;
import opennlp.tools.tokenize.TokenizerME;
import opennlp.tools.tokenize.TokenizerModel;

import javax.print.Doc;
import java.io.File;
import java.io.IOException;
import java.io.InputStream;
import java.util.*;

public class Parser {
    private static DictionaryLemmatizer DICTIONARY_LEMMATIZER;
    private static SentenceDetectorME SENTENCE_DETECTOR;
    private static TokenizerME TOKENIZER;
    private static POSTaggerME POS_TAGGER;

    static {
        try {
            SENTENCE_DETECTOR = new SentenceDetectorME(new SentenceModel(Parser.class.getResourceAsStream("Models/" + "en-sent" + ".bin")));
            POS_TAGGER = new POSTaggerME(new POSModel(Parser.class.getResourceAsStream("Models/" + "en-pos-maxent" + ".bin")));
            TOKENIZER = new TokenizerME(new TokenizerModel(Parser.class.getResourceAsStream("Models/" + "en-token" + ".bin")));
        } catch (IOException e) {
            e.printStackTrace();
        }
    }


    public static Data[] parseDocument(Document document) {
        return null;
    }

    public static Collection<Data> parseSentence(String sentence) {
        HashMap<String, Data> data = new HashMap<>();
        String[] words = TOKENIZER.tokenize(sentence);
        String[] POSTagsOfWords = POS_TAGGER.tag(words);
        List<String> wordList = Arrays.asList(words);
        List<String> POSTagsList = Arrays.asList(POSTagsOfWords);
        int indexOfWord = 0;
        for (int i = 0; i < wordList.size(); i++) {
            String tag = POSTagsList.get(i);
            if (tag.matches("[A-Z]+")) {
                if (isPOSTagValuable(tag)) {
                    String stemmedWord = stemWord(wordList.get(i), POSTagsList.get(i));
                    Data data1 = data.get(stemmedWord);
                    if (data1 != null) {
                        data1.addPosition(indexOfWord);
                    } else {
                        Data data2 = new Data(stemmedWord);
                        data2.addPosition(indexOfWord);
                        data.put(stemmedWord, data2);
                    }
                }
                indexOfWord++;
            }
        }
        return data.values();
    }

    private static boolean isPOSTagValuable(String POSTag) {
        return !(POSTag.equals("DT") && POSTag.equals("IN") && POSTag.equals("TO"));
    }

    public static String stemWord(String word, String POSTag) {
        return DICTIONARY_LEMMATIZER.lemmatize(new String[]{word}, new String[]{POSTag})[0];
    }

    


}
