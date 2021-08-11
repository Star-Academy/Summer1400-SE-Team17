package searchtools;

import java.util.Set;

public interface Searcher<T> {
    Set<T> search(String command);
}
