﻿// See https://aka.ms/new-console-template for more information
using GameboyColourDecolouriser;
using Spectre.Console;
using Spectre.Console.Cli;
using System.CommandLine;
using System.Drawing;
using Command = System.CommandLine.Command;

// https://docs.microsoft.com/en-us/dotnet/standard/commandline/
// and spectre.console
Console.WriteLine("Hello, World!");

var threeColourTile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
    @"GitHub\Pokemon-gen-2-style-tilemap/Original/Grass-flat.png");

threeColourTile = @"C:\Users\Niko Uusitalo\Documents\GitHub\Pokemon-gen-2-style-tilemap\ayylmao.png";
//threeColourTile = @"C:\Users\Niko Uusitalo\Documents\GitHub\Little-Mokki-In-The-Woods\LittleMokkiInTheWoods\assets\backgrounds\Mokki-area-export.png";
threeColourTile = @"C:\Users\Niko Uusitalo\Documents\GitHub\GameboyColour-Decolouriser\etc\mokkiarea-truecolour.png";

//var app = new CommandApp();
//app.Configure(config =>
//{
//    config.AddDelegate("foo", Foo)
//        .WithDescription("Foos the bars");

//    config.AddDelegate<BarSettings>("bar", Bar)
//        .WithDescription("Bars the foos"); ;
//});

//return app.Run(args);

// use system.commandline to understand even though i could use spectuire.console.cli
// move to dependency injection, inject the progrss tasks, and everything else
// open with commmand app typed to the program type

var rootCommand = new RootCommand("this is the description of the root");

var decolouriseCommand = new Command("Decolourise", "Decolourises a Gameboy Color image into a Gameboy image.");

//decolouriseCommand.SetHandler(() => Console.WriteLine("hello"));

var inputFile = new Option<FileInfo>(
    name: "--input",
    description: "The input image file to be decolourised.");

var outputFile = new Option<FileInfo>(
    name: "--output",
    description: "The output image file to save the decolourised as.");

decolouriseCommand.AddOption(inputFile);
decolouriseCommand.AddOption(outputFile);
rootCommand.AddCommand(decolouriseCommand);

decolouriseCommand.SetHandler((input, output) =>
{
    if (input is null || output is null)
    {
        Console.WriteLine("one of them is null");
    }
    else
    {
        Console.WriteLine($"Input file {input.Name}7, output file {output.Name}");

        AnsiConsole.Progress()
        .Start(ctx =>
        {

            // Define tasks
            var loadingImage = ctx.AddTask("[green]Loading Image[/]");
            var decolourStageOne = ctx.AddTask("[green]Decolouring Stage One[/]");
            var decolourStageTwo = ctx.AddTask("[green]Decolouring Stage Two[/]");
            var decolourStageThree = ctx.AddTask("[green]Decolouring Stage Three)[/]");
            var decolourStageFour = ctx.AddTask("[green]Decolouring Stage Four[/]");
            var generatingFinalImage = ctx.AddTask("[green]Generating Final Image[/]");

            var spectreTasks = new SpectreTasks(decolourStageOne, decolourStageTwo, decolourStageThree, decolourStageFour, generatingFinalImage);

            var gbImage = new GbImage();

            gbImage.LoadImage(input.FullName, loadingImage);

            var decolouriser = new Decolouriser();

            var recolouredImage = decolouriser.Decolourise(gbImage, spectreTasks);

            recolouredImage.Save(output.FullName);
        });



    }
},
inputFile, outputFile);


AnsiConsole.WriteLine("ayylmao2");



AnsiConsole.WriteLine("ayylmao2");

rootCommand.Invoke(args);

public record class SpectreTasks(
    ProgressTask decolourStageOne,
    ProgressTask decolourStageTwo,
    ProgressTask decolourStageThree,
    ProgressTask decolourStageFour,
    ProgressTask generatingFinalImage);
