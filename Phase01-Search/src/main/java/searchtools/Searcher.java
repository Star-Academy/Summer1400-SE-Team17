package searchtools;

import data.Data;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.HashSet;

public class Searcher {
    private final HashMap<String, ArrayList<Data>> dataBase;

    public Searcher(InvertedIndex invertedIndex) {
        dataBase = invertedIndex.getDataBase();
    }

    public HashSet<Integer> search(String command) {
        String[] words = getWords(command);
        stemWords(words);
        return getResult(words);
    }

    private HashSet<Integer> getResult(String[] words) {
        HashSet<Integer> result = new HashSet<>(getEssentialWordsDocs(words));
        result.addAll(getWillingWordsDocs(words));
        result.removeAll(getBannedWordsDocs(words));
        return result;
    }


    private HashSet<Integer> getBannedWordsDocs(String[] words) {
        HashSet<Integer> result = new HashSet<>();
        for (String word : words) {
            if (isBannedWord(word)) {
                result.addAll(getWordsDocs(word.substring(1)));
            }
        }
        return result;
    }

    private HashSet<Integer> getWillingWordsDocs(String[] words) {
        HashSet<Integer> result = new HashSet<>();
        for (String word : words) {
            if (isWillingWord(word)) {
                result.addAll(getWordsDocs(word.substring(1)));
            }
        }
        return result;
    }

    private HashSet<Integer> getEssentialWordsDocs(String[] words) {
        HashSet<Integer> result = new HashSet<>();
        boolean isFirstEssential = true;
        for (String word : words) {
            if (isWillingWord(word) || isBannedWord(word)) {
                continue;
            }
            HashSet<Integer> documentsWithWord = getWordsDocs(word);
            if (isFirstEssential) {
                result.addAll(documentsWithWord);
                isFirstEssential = false;
            } else {
                result.retainAll(documentsWithWord);
            }
        }
        return result;
    }

    private HashSet<Integer> getWordsDocs(String word) {
        HashSet<Integer> result = new HashSet<>();
        if (!Parser.isWordValuable(word)) return result;
        for (Data data : dataBase.getOrDefault(word, new ArrayList<>())) {
            result.add(data.getIndexDocument());
        }
        return result;
    }


    private void stemWords(String[] words) {
        for (int i = 0; i < words.length; ++i) {
            if (isWillingWord(words[i]) || isBannedWord(words[i])) {
                words[i] = words[i].charAt(0) + Parser.stemWord(words[i].substring(1));
            } else {
                words[i] = Parser.stemWord(words[i]);
            }
        }
    }

    private boolean isWillingWord(String str) {
        return str.startsWith("+");
    }

    private boolean isBannedWord(String str) {
        return str.startsWith("-");
    }

    private String[] getWords(String command) {
        return command.trim().toLowerCase().split(" ");
    }


}
