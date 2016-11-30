using System;
using System.Collections.Generic;

namespace Acr.UserDialogs
{
	public class PromptConfigMultiInput
	{
		public static string DefaultOkText { get; set; } = "Ok";
		public static string DefaultCancelText { get; set; } = "Cancel";
		public static int? DefaultAndroidStyleId { get; set; }

		public string Title { get; set; }
		public string Message { get; set; }
		public Action<PromptResultMultiInput> OnAction { get; set; }

		public bool IsCancellable { get; set; } = true;

		public string OkText { get; set; } = DefaultOkText;
		public string CancelText { get; set; } = DefaultCancelText;
		public int? AndroidStyleId { get; set; }

		public List<PromptInput> Inputs { get; set; }

		public Action<PromptTextChangedArgs> OnTextChanged { get; set; }


		public PromptConfigMultiInput SetTitle(string title)
		{
			this.Title = title;
			return this;
		}


		public PromptConfigMultiInput SetMessage(string message)
		{
			this.Message = message;
			return this;
		}


		public PromptConfigMultiInput SetCancellable(bool cancel)
		{
			this.IsCancellable = cancel;
			return this;
		}


		public PromptConfigMultiInput SetOkText(string text)
		{
			this.OkText = text;
			return this;
		}

		public PromptConfigMultiInput SetCancelText(string cancelText)
		{
			this.IsCancellable = true;
			this.CancelText = cancelText;
			return this;
		}

		public PromptConfigMultiInput SetInputMode(PromptInput promptInput)
		{
			this.Inputs.Add(promptInput);
			return this;
		}


		public PromptConfigMultiInput SetOnTextChanged(Action<PromptTextChangedArgs> onChange)
		{
			this.OnTextChanged = onChange;
			return this;
		}
	}
}
