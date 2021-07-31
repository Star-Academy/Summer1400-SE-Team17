import executor.ContentGetter;
import executor.Printer;
import executor.ProgramExecutor;
import executor.Reader;
import org.junit.jupiter.api.Test;
import org.mockito.invocation.InvocationOnMock;
import org.mockito.stubbing.Answer;
import searchtools.Searcher;

import java.util.Arrays;
import java.util.HashSet;
import java.util.Stack;

import static org.mockito.ArgumentMatchers.anyString;
import static org.mockito.Mockito.*;

public class ViewTest {
    @Test
    public void executorTest() {
        ProgramExecutor executor = ProgramExecutor.getExecutor();

        Stack<String> stringStack = new Stack<>();
        stringStack.push("--exit");
        stringStack.push("sia");
        stringStack.push("pasha");

        Reader mockReader = mock(Reader.class);
        when(mockReader.read()).thenAnswer((Answer<String>) invocationOnMock -> stringStack.pop());

        Printer mockPrinter = mock(Printer.class);

        ContentGetter<String,Integer> mockGetter = (ContentGetter<String, Integer>) mock(ContentGetter.class);
        when(mockGetter.getContent(1)).thenReturn("salam, pasha");
        when(mockGetter.getContent(2)).thenReturn("salam, sia");

        Searcher<Integer> mockSearcher = (Searcher<Integer>) mock(Searcher.class);
        when(mockSearcher.search((anyString()))).thenReturn(new HashSet<>(Arrays.asList(1,2)));

        executor.setContentGetter(mockGetter);
        executor.setReader(mockReader);
        executor.setPrinter(mockPrinter);
        executor.setSearcher(mockSearcher);

        executor.execute();

        verify(mockPrinter,times(2)).print("salam, pasha");
        verify(mockPrinter,times(2)).print("salam, sia");
    }

}
