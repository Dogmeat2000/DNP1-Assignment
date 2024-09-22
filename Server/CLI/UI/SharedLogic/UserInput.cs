namespace ConsoleApp1.UI.SharedLogic;

public class UserInput {

    /** Pass, as an argument, the desired string to display to the user on the same line user input is taken from, in the console. */
    public async Task<string?> ReadUserInputAsync(string visCmd) {
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
}