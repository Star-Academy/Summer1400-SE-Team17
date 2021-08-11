package executor;

import java.io.IOException;
import java.net.URISyntaxException;

public interface ContentGetter<T,K> {
    T getContent(K content);
}
