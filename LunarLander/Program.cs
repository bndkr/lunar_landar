using System;
using System.IO;

try
{
    using var game = new LunarLander.Game1();
    game.Run();
}
catch (Exception ex)
{
    string errorMessage = $"An exception occurred: {ex.Message}";
    File.WriteAllText("error.txt", errorMessage);
}
