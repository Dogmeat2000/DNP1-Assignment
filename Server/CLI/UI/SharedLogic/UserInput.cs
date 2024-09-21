namespace ConsoleApp1.UI.SharedLogic;

public class UserInput {

    public async Task<string?> ReadUserInputAsync_Main() {
        var inputReceived = false;
        string? userInput = null;
        while (!inputReceived) {
            Console.Write("\nCmd: ");
            userInput = Console.ReadLine();

            if (userInput != null)
                inputReceived = true;
        }
        return userInput;
    }
    
    public async Task<string?> ReadUserInputAsync_Alt1() {
        var inputReceived = false;
        string? userInput = null;
        while (!inputReceived) {
            userInput = Console.ReadLine();

            if (userInput != null)
                inputReceived = true;
        }
        return userInput;
    }
}