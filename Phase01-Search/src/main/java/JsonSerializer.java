import com.google.gson.Gson;

import java.io.FileReader;
import java.io.FileWriter;
import java.lang.reflect.Type;

public class JsonSerializer {
    public static Object fromJson(FileReader reader , Type type) {
        Gson gson = new Gson();
        return gson.fromJson(reader , type);
    }
    public static void toJson(Object object , FileWriter writer) {
        Gson gson = new Gson();
        gson.toJson(object , writer);
    }
}
