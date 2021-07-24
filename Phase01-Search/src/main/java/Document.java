import lombok.Getter;

@Getter
public class Document {
    private final String content;
    private final int id;
    public Document(String content , int id) {
        this.content = content;
        this.id = id;
    }
}
