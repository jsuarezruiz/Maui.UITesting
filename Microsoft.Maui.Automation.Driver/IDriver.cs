﻿namespace Microsoft.Maui.Automation.Driver
{
	public interface IDriver : IDisposable
	{
		string Name { get; }

		IAutomationConfiguration Configuration { get; }

		Task<IDeviceInfo> GetDeviceInfo();

		Task InstallApp();

		Task RemoveApp();

		Task LaunchApp();
		Task StopApp();

		Task ClearAppState();

		Task PushFile(string localFile, string destinationDirectory);
		Task PullFile(string remoteFile, string localDirectory);

		Task Tap(int x, int y);

		Task LongPress(int x, int y);

		Task Tap(Element element);

		Task KeyPress(char keyCode);

		Task Swipe((int x, int y) start, (int x, int y) end);

		Task Back();

		Task InputText(string text);

		Task OpenUri(string uri);

		Task<string> GetProperty(string elementId, string propertyName);

		Task<IEnumerable<Element>> GetElements();

		Task<IEnumerable<Element>> FindElements(string propertyName, string pattern, bool isExpression = false, string ancestorId = "");

		Task<PerformActionResult> PerformAction(string action, string elementId, params string[] arguments);
	}
}