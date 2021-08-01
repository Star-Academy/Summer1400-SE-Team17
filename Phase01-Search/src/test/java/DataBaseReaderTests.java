import data.DatabaseReader;
import data.Document;
import executor.ProgramExecutor;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;
import org.mockito.Mockito;
import searchtools.Searcher;

import java.io.IOException;
import java.net.URISyntaxException;
import java.util.ArrayList;
import java.util.Set;

public class DataBaseReaderTests {
    @Test
    public void getDocsInDirectoryTest() throws IOException, URISyntaxException {
        int expectedId = 57110;
        String expectedContent = "I have a 42 yr old male friend, misdiagnosed as havin osteopporosis for two y" +
                "ears, who recently found out that hi illness is the rare Gaucher's disease.Gaucher's diseas" +
                "e symptoms include: brittle bones (he lost 9 inches off his hieght); enlarged liver and spl" +
                "een; interna bleeding; and fatigue (all the time). The problem (in Type 1) i attributed to a gen" +
                "etic mutation where there is a lack of th enzyme glucocerebroside in macrophages so the cells swell " +
                "up This will eventually cause deathEnyzme replacement therapy has been successfully developed an appr" +
                "oved by the FDA in the last few years so that those patient administered with this drug (called Ceredas" +
                "e) report a remarkabl improvement in their condition. Ceredase, which is manufacture by biotech biggy comp" +
                "any--Genzyme--costs the patient $380,00 per year. Gaucher\\'s disease has justifyably been called \"the mo" +
                "s expensive disease in the world\"NEED INFOI have researched Gaucher's disease at the library but am relyin" +
                " on netlanders to provide me with any additional information**news, stories, report**people you know with t" +
                "his diseas**ideas, articles about Genzyme Corp, how to get a hold o   enough money to buy some, programs av" +
                "ailable to help wit   costs**Basically ANY HELP YOU CAN OFFEThanks so very muchDeborah";
        ArrayList<Document> documents = DatabaseReader.getDocsInDirectory("src/main/resources/searchtools/EnglishData");
        Document wantedDocument = null;
        for(Document document : documents)
            if(document.getId() == expectedId)
                wantedDocument = document;
        Assertions.assertEquals(wantedDocument.getContent() , expectedContent);
        Assertions.assertEquals(wantedDocument.getId() , expectedId);
        Mockito.when(ProgramExecutor.abc()).thenReturn(10);
    }
}
