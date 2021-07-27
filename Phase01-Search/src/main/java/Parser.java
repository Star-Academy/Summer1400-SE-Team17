import opennlp.tools.lemmatizer.DictionaryLemmatizer;
import opennlp.tools.postag.POSModel;
import opennlp.tools.postag.POSTaggerME;
import opennlp.tools.sentdetect.SentenceDetectorME;
import opennlp.tools.sentdetect.SentenceModel;
import opennlp.tools.tokenize.TokenizerME;
import opennlp.tools.tokenize.TokenizerModel;

import java.io.IOException;
import java.util.Arrays;
import java.util.Collection;
import java.util.HashMap;
import java.util.List;

public class Parser {
    private static DictionaryLemmatizer DICTIONARY_LEMMATIZER;
    private static SentenceDetectorME SENTENCE_DETECTOR;
    private static TokenizerME TOKENIZER;
    private static POSTaggerME POS_TAGGER;

    static {
        try {
            SENTENCE_DETECTOR = new SentenceDetectorME(new SentenceModel(
                    Parser.class.getResourceAsStream("Models/" + "en-sent" + ".bin")));
            POS_TAGGER = new POSTaggerME(
                    new POSModel(Parser.class.getResourceAsStream("Models/" + "en-pos-maxent" + ".bin")));
            TOKENIZER = new TokenizerME(
                    new TokenizerModel(Parser.class.getResourceAsStream("Models/" + "en-token" + ".bin")));
            DICTIONARY_LEMMATIZER = new DictionaryLemmatizer(
                    Parser.class.getResourceAsStream("Models/" + "en-lemmatizer.dict" + ".txt"));
        } catch (IOException ignored) {
        }
    }

    public static Collection<Data> parseDocument(Document document) {
        HashMap<String, Data> wordToData = new HashMap<>();
        String[] sentences = SENTENCE_DETECTOR.sentDetect(document.getContent());
        int wordsCount = 0;
        for (String sentence : sentences) {
            int newWordsCount = 0;
            for (Data data : parseSentence(sentence)) {
                String word = data.getWord();
                int id = document.getId();
                if (wordToData.get(data.getWord()) == null) {
                    wordToData.put(word, new Data(word, id));
                }
                Data existingData = wordToData.get(word);
                newWordsCount += data.getPositions().size();
                for (int position : data.getPositions()) {
                    existingData.getPositions().add(wordsCount + position);
                }
            }
            wordsCount += newWordsCount;
        }
        return wordToData.values();
    }

    public static Collection<Data> parseSentence(String sentence) {
        sentence = modifySentence(sentence);

        HashMap<String, Data> data = new HashMap<>();

        String[] words = TOKENIZER.tokenize(sentence);
        String[] POSTagsOfWords = POS_TAGGER.tag(words);
        List<String> wordList = Arrays.asList(words);
        List<String> POSTagsList = Arrays.asList(POSTagsOfWords);

        int indexOfWord = 0;
        for (int i = 0; i < wordList.size(); i++) {
            String tag = POSTagsList.get(i);
            String word = wordList.get(i);
            if (tag.matches("[A-Z$]+") && !tag.equals("$")) {
                if (isPOSTagValuable(tag)) {
                    String stemmedWord = stemWord(word, tag);
                    addData(data, indexOfWord, stemmedWord);
                }
                indexOfWord++;
            }
        }
        return data.values();
    }


    public static String stemWord(String word, String POSTag) {
        String stemmedWord = DICTIONARY_LEMMATIZER.lemmatize(new String[]{word}, new String[]{POSTag})[0];
        if (stemmedWord.equals("O")) {
            stemmedWord = word;
        }
        return stemmedWord;
    }

    public static String stemWord(String word) {
        String lemma = DICTIONARY_LEMMATIZER.lemmatize(new String[]{word},
                new String[]{POS_TAGGER.tag(new String[]{word})[0]})[0];
        return lemma.equals("O") ? word : lemma;
    }


    private static void addData(HashMap<String, Data> data, int indexOfWord, String stemmedWord) {
        Data data1 = data.get(stemmedWord);
        if (data1 != null) {
            data1.addPosition(indexOfWord);
        } else {
            Data data2 = new Data(stemmedWord);
            data2.addPosition(indexOfWord);
            data.put(stemmedWord, data2);
        }
    }

    private static String modifySentence(String sentence) {
        sentence = sentence.toLowerCase();
        sentence = sentence.replaceAll("[^\\w]", " ");
        return sentence.trim();
    }

    private static boolean isPOSTagValuable(String POSTag) {
        return !(POSTag.equals("DT") ||
                POSTag.equals("IN") ||
                POSTag.equals("TO") ||
                POSTag.equals("POS") ||
                POSTag.equals("PRP") ||
                POSTag.equals("PRP$"));
    }

}
