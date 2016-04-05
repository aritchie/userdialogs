using System;

namespace Acr.UserDialogs
{

    public class PromptResult
    {
        public PromptResult(bool ok, string text)
        {
            this.Ok = ok;
            this.Text = text;
        }

        public bool Ok { get; }
        public string Text { get; }
    }
}

