import opennlp.tools.postag.POSModel;
import opennlp.tools.postag.POSTaggerME;
import opennlp.tools.sentdetect.SentenceDetectorME;
import opennlp.tools.sentdetect.SentenceModel;

import javax.print.Doc;
import java.io.File;
import java.io.IOException;
import java.io.InputStream;
import java.util.ArrayList;

public class Parser {
    private static SentenceDetectorME SENTENCE_DETECTOR;
    private static POSTaggerME POS_TAGGER;

    static {
        try {
            SENTENCE_DETECTOR = new SentenceDetectorME(new SentenceModel(Parser.class.getResourceAsStream("Models/" + "en-sent" +".bin")));
            POS_TAGGER = new POSTaggerME(new POSModel(Parser.class.getResourceAsStream("Models/" + "en-pos-maxent" +".bin")));
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    
    public static Data[] parseDocument(Document document) {
        return null;
    }

    public static Data[] parseSentence(String sentence) {

    }

    public static Data getDataOfWordInSentence(String word) {
        return null;
    }






}
