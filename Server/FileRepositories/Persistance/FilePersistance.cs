using System.Text.Json;
using Entities;

namespace FileRepositories.Persistance;

public class FilePersistance : IFilePersistance {
    
    
    /** You must pass an object obj as the second argument.
     This can just be a new Object of the type you wish to load.
     It is only used to identify which format the loaded data should be serialized into. */
    
    public async Task<List<object>?> ReadFromJsonFileAsync(string jsonFilePath, object obj) {
        // Attempt to load the data as plain txt:
        string? jsonText = await LoadDataAsTxt(jsonFilePath);
        if (jsonText != null) {
            // Data was loaded. Now deserialize the data, and return the deserialized data
            return DeserializeJsonFile(jsonText, obj);
        }
        
        // Return Null, since something prevented data from being loaded:
        return null;
    }
    

    public async Task<bool> SaveToJsonFileAsync(string jsonFilePath, List<object> data) {
        
        // Attempt to serialize data:
        string serializedData = JsonSerializer.Serialize(data);
        
        // Attempt to write to file:
        await File.WriteAllTextAsync(jsonFilePath, serializedData);

        // TODO: Could implement persistence check, integrity checks or something else here. But not a priority!
        return true;
    }

    
    private async Task<string?> LoadDataAsTxt(string jsonFilePath) {
        // Check if the File already exists at the given path.
        if (!CheckFileExists(jsonFilePath)) {
            // If it does not exist, attempt to create a new empty File:
            try {
                CreateEmptyFile(jsonFilePath);
                // Return an empty string, since File was just created.
                return "";
            } catch (FileNotFoundException e) {
                // Catch error given if system was unable to create the empty File, for whatever reason:
                Console.WriteLine(e.Message);
                return null;
            }
        } 
        
        // If File already exists, read data from that File and return it:
        return await File.ReadAllTextAsync(jsonFilePath);
    }


    /** Legal object entities are: Comment, Forum, Post, User, UserProfile */
    private List<object>? DeserializeJsonFile(string jsonTxt, object obj) { 
        List<object>? deserializedData;

        // Check which object type the file should be returned as, and perform the deserialization for each object type:
        switch (obj) {
                case Comment:
                    deserializedData = JsonSerializer.Deserialize<List<Comment>>(jsonTxt)?.Cast<object>().ToList() ?? null;
                    break;
                
                case Forum:
                    deserializedData = JsonSerializer.Deserialize<List<Forum>>(jsonTxt)?.Cast<object>().ToList() ?? null;
                    break;
                
                case Post:
                    deserializedData = JsonSerializer.Deserialize<List<Post>>(jsonTxt)?.Cast<object>().ToList() ?? null;
                    break;
                
                case User:
                    deserializedData = JsonSerializer.Deserialize<List<User>>(jsonTxt)?.Cast<object>().ToList() ?? null;
                    break;
                
                case UserProfile:
                    deserializedData = JsonSerializer.Deserialize<List<UserProfile>>(jsonTxt)?.Cast<object>().ToList() ?? null;
                    break;
                
                default:
                    return null;
            }
        return deserializedData;
    }
    
    
    private bool CheckFileExists(string filePath) {
        return File.Exists(filePath);
    }

    
    private void CreateEmptyFile(string filePath) {
        var maxNumberOfRetries = 10;
        var retryCount = 0;

        while (retryCount < maxNumberOfRetries) {
            // Attempt to create an empty file:
            File.WriteAllText(filePath, "");
        
            // Check that file was created:
            if (CheckFileExists(filePath)) 
                return; // File was created
            
            Task.Delay(200).Wait(); // Causes a maximum delay of 2 seconds!
            retryCount++;
        }
        
        // File was not created and retries expired. Throw an error.
        throw new FileNotFoundException($"ERROR: It is not possible to create a new '{filePath}' file. Please check system settings, and if location is readonly!");
    }
}