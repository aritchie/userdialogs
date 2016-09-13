using System;


namespace Acr.UserDialogs
{
    using System.Collections.Generic;

    public class MultiPickerPromptResult : AbstractStandardDialogResult<IList<int>>
    {
        public MultiPickerPromptResult(bool ok, IList<int> selectedValues) : base(ok, selectedValues)
        {
        }

        public IList<int> SelectedValues => this.Value;
    }
}