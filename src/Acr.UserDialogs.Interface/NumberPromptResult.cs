using System;
namespace Acr.UserDialogs
{
    public class NumberPromptResult : AbstractStandardDialogResult<int>
    {
        public NumberPromptResult(bool ok, int value) : base(ok, value)
        {
            
        }

        public int SelectedNumber => this.Value;
    }
}
