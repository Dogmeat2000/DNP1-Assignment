using Entities;

namespace FileRepositories;

public interface IFilePersistance {
    Task<List<object>?> ReadFromJsonFileAsync(string jsonFilePath, object obj);
    Task<bool> SaveToJsonFileAsync(string jsonFilePath, List<object> data);
}