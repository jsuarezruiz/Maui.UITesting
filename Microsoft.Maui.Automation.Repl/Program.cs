﻿using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Automation;
using Microsoft.Maui.Automation.Driver;
using Microsoft.Maui.Automation.Remote;
using Spectre.Console;
using System.Net;

Task<string?>? readTask = null;
CancellationTokenSource ctsMain = new CancellationTokenSource();

Console.CancelKeyPress += (s, e) =>
{
	ctsMain.Cancel();
};

var platform = Platform.Maui;


var config = new AutomationConfiguration
{
	AppAgentPort = 5000,
	DevicePlatform = Platform.Winappsdk,
	AutomationPlatform = platform,
	Device = "windows",
	AppId = "D05ADD49-B96D-49E5-979C-FA3A3F42F8E0_9zz4h110yvjzm!App"
};
var driver = new AppDriver(config);

var mappings = new Dictionary<string, Func<Task>>
{
	{ "tree", Tree },
	{ "windows", Windows },
	{ "test3", async () =>
		{

			// Find the button by its MAUI AutomationId property
			var button = await driver.FirstByAutomationId("buttonOne");

			// Tap the button to increment the counter
			await driver.Tap(button);

			// Find the label we expect to have changed
			var label = await driver.By(e =>
				e.Type == "Label"
				&& e.Text.Contains("1"));

			Console.WriteLine((label?.FirstOrDefault())?.Text);
		}
	},
	{ "test2", async () =>
		{
			var button = await driver.FirstByAutomationId("buttonOne");

			await driver.Tap(button!);

			var label = await driver.FirstByAutomationId("labelCount");

			Console.WriteLine(label.Text);
		}
	},
	{ "test", async () =>
		{
			var elements = await driver.FindElements("AutomationId", "buttonOne");

			foreach (var w in elements)
			{
				var tree = new Tree(w.ToTable(ConfigureTable));

				AnsiConsole.Write(tree);
			}
		}
	}
};


//var button = await driver.FirstByAutomationId("buttonOne");

// Tap the button to increment the counter
//await driver.Tap(button);


while (!ctsMain.Token.IsCancellationRequested)
{
	var input = await ReadLineAsync(ctsMain.Token);

	try
	{
		foreach (var kvp in mappings)
		{
			if (input?.StartsWith(kvp.Key) ?? false)
			{
				_ = Task.Run(async () =>
				{
					try
					{
						await kvp.Value();
					}
					catch (Exception ex)
					{
						AnsiConsole.WriteException(ex);
					}
				});
				
				break;
			}
		}
	}
	catch (Exception ex)
	{
		AnsiConsole.WriteException(ex);
	}

	if (input != null && (input.Equals("quit", StringComparison.OrdinalIgnoreCase)
		|| input.Equals("q", StringComparison.OrdinalIgnoreCase)
		|| input.Equals("exit", StringComparison.OrdinalIgnoreCase)))
	{
		ctsMain.Cancel();
	}
}

driver.Dispose();

Environment.Exit(0);

async Task Tree()
{
	var children = await driver.GetElements();

	foreach (var w in children)
	{
		var tree = new Tree(w.ToTable(ConfigureTable));


		foreach (var d in w.Children)
		{
			PrintTree(tree, d, 1);
		}

		AnsiConsole.Write(tree);
	}
}

async Task Windows()
{
	var children = await driver.GetElements();

	foreach (var w in children)
	{
		var tree = new Tree(w.ToTable(ConfigureTable));

		AnsiConsole.Write(tree);
	}
}

void PrintTree(IHasTreeNodes node, Element element, int depth)
{
	var subnode = node.AddNode(element.ToTable(ConfigureTable));

	foreach (var c in element.Children)
		PrintTree(subnode, c, depth);
}

static void ConfigureTable(Table table)
{
	table.Border(TableBorder.Rounded);
}


async Task<string?> ReadLineAsync(CancellationToken cancellationToken = default)
{
	try
	{
		readTask = Task.Run(() => Console.ReadLine());

		await Task.WhenAny(readTask, Task.Delay(-1, cancellationToken));

		if (readTask.IsCompletedSuccessfully)
			return readTask.Result;
	}
	catch (Exception ex)
	{
		throw ex;
	}

	return null;
	
}