import opennlp.tools.postag.POSModel;
import opennlp.tools.postag.POSTaggerME;
import opennlp.tools.sentdetect.SentenceDetectorME;
import opennlp.tools.sentdetect.SentenceModel;

import java.io.IOException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

public class Parser {
    private static SentenceDetectorME SENTENCE_DETECTOR;
    private static POSTaggerME POS_TAGGER;

    static {
        try {
            SENTENCE_DETECTOR = new SentenceDetectorME(new SentenceModel(Parser.class.getResourceAsStream("Models/" + "en-sent" + ".bin")));
            POS_TAGGER = new POSTaggerME(new POSModel(Parser.class.getResourceAsStream("Models/" + "en-pos-maxent" + ".bin")));
        } catch (IOException e) {
            e.printStackTrace();
        }
    }


    public static ArrayList<Data> parseDocument(Document document) {
        ArrayList<Data> result = new ArrayList<>();
        HashMap<String, Data> wordToData = new HashMap<>();
        String[] sentences = SENTENCE_DETECTOR.sentDetect(document.getContent());
        int wordsCount = 0;
        for (String sentence : sentences) {
            int newWordsCount = 0;
            for (Data data : parseSentence(sentence)) {
                if (wordToData.get(data.getWord()) == null) {
                    Data newData = new Data();
                    result.add(newData);
                    newData.setWord(data.getWord());
                    newData.setIndexDocument(document.getId());
                    wordToData.put(data.getWord(), new Data());
                }
                Data existingData = wordToData.get(data.getWord());
                newWordsCount += existingData.getPositions().size();
                for(int position : existingData.getPositions())
                    existingData.getPositions().add(wordsCount + position);
            }
            wordsCount += newWordsCount;
        }
        return result;
    }

    public static List<Data> parseSentence(String sentence) {
        return null;
    }

    public static Data getDataOfWordInSentence(String word) {
        return null;
    }


}
