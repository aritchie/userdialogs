using System;
using System.Collections.Generic;

namespace Acr.UserDialogs
{
	public class PromptResultMultiInput : AbstractStandardDialogResult<List<PromptInput>>
	{
		public PromptResultMultiInput(bool ok, List<PromptInput> text) : base(ok, text)
		{
		}

		public List<PromptInput> PromptInputs => this.Value;
	}
}
