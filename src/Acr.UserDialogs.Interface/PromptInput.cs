using System;
namespace Acr.UserDialogs
{
	public class PromptInput
	{

		public InputType InputType { get; set; } = InputType.Default;
		public string Placeholder { get; set; } = string.Empty;
		public int MaxLength { get; set; } = -1;
		public string Text { get; set; } = string.Empty;

		public PromptInput()
		{
		}
	}
}
