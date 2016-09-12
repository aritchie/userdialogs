using System;


namespace Acr.UserDialogs
{
    using System.Collections.Generic;

    public class PickerPromptResult : AbstractStandardDialogResult<IList<int>>
    {
        public PickerPromptResult(bool ok, IList<int> selectedValues) : base(ok, selectedValues)
        {
        }

        public IList<int> SelectedValues => this.Value;
    }
}