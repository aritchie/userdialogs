using System;

namespace Acr.UserDialogs
{

    public class PromptResult
    {
        public PromptResult(bool ok, string text, string textTwo = "")
        {
            this.Ok = ok;
            this.Text = text;
			this.TextTwo = textTwo;
        }

        public bool Ok { get; }
        public string Text { get; }
		// PromptTwoInputs added by Lee Bettridge
		public string TextTwo { get; }
	}
}

