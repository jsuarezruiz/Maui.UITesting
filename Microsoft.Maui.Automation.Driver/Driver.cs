﻿namespace Microsoft.Maui.Automation.Driver
{
	public abstract class Driver : IDriver
	{
		public Driver(IAutomationConfiguration configuration)
		{
			Configuration = configuration;
		}

		public abstract string Name { get; }
		
		public IAutomationConfiguration Configuration { get; }

		public abstract Task Back();

		public abstract Task ClearAppState();

		public abstract void Dispose();

		public abstract Task<IDeviceInfo> GetDeviceInfo();

		public abstract Task InputText(string text);

		public abstract Task InstallApp();

		public abstract Task KeyPress(char keyCode);

		public abstract Task LaunchApp();

		public abstract Task LongPress(int x, int y);

		public abstract Task OpenUri(string uri);

		public abstract Task<IEnumerable<Element>> FindElements(string propertyName, string pattern, bool isExpression = false, string ancestorId = "");

		public abstract Task<IEnumerable<Element>> GetElements();

		public abstract Task<string> GetProperty(string elementId, string propertyName);

		public abstract Task<PerformActionResult> PerformAction(string action, string elementId, params string[] arguments);

		public abstract Task PullFile(string remoteFile, string localDirectory);

		public abstract Task PushFile(string localFile, string destinationDirectory);

		public abstract Task RemoveApp();

		public abstract Task StopApp();

		public abstract Task Swipe((int x, int y) start, (int x, int y) end);

		public abstract Task Tap(int x, int y);

		public abstract Task Tap(Element element);
	}
}