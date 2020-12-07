using System;
using Spectre.Console;

namespace PocketCastsLogin
{
    public class LoginWindow : ILoginWindow
    {
        public (string email, string password) Show()
        {
            // Check if we can accept key strokes
            if (!AnsiConsole.Capabilities.SupportsInteraction)
            {
                AnsiConsole.MarkupLine("[red]Environment does not support interaction.[/]");
                return default;
            }

            // Secret
            AnsiConsole.WriteLine();
            AnsiConsole.Render(new Rule("[yellow]PocketCasts Login[/]").RuleStyle("grey").Centered());
            var email = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter [green]email[/]?")
                    .PromptStyle("red"));

            var password = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter [green]password[/]?")
                    .PromptStyle("red")
                    .Secret());

            Console.WriteLine("Thank you.");

            return (email, password);
        }
    }
}