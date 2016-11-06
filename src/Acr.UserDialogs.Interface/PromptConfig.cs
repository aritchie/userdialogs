using System;


namespace Acr.UserDialogs
{
    public class PromptConfig : IAndroidStyleDialogConfig
    {
        public static string DefaultPositiveText { get; set; } = "Ok";
        public static string DefaultNeutralText { get; set; } = "Cancel";
        public static string DefaultNegativeText { get; set; } = "Remove";
        public static int? DefaultAndroidStyleId { get; set; }
        public static int? DefaultMaxLength { get; set; }

        public string Title { get; set; }
        public string Message { get; set; }
        public Action<DialogResult<string>> OnAction { get; set; }

        public bool IsCancellable { get; set; } = true;
        public string Text { get; set; }

        public string PositiveText { get; set; } = DefaultPositiveText;
        public string NeutralText { get; set; } = DefaultNeutralText;
        public string NegativeText { get; set; } = DefaultNegativeText;
        public string Placeholder { get; set; }
        public int? MaxLength { get; set; } = DefaultMaxLength;
        public int? AndroidStyleId { get; set; }
        public InputType InputType { get; set; } = InputType.Default;
        public Action<PromptTextChangedArgs> OnTextChanged { get; set; }


        public PromptConfig SetTitle(string title)
        {
            this.Title = title;
            return this;
        }


        public PromptConfig SetMessage(string message)
        {
            this.Message = message;
            return this;
        }


        public PromptConfig SetCancellable(bool cancel)
        {
            this.IsCancellable = cancel;
            return this;
        }


        public PromptConfig SetText(DialogChoice choice, string text)
        {
            switch (choice)
            {
                case DialogChoice.Negative:
                    this.NegativeText = text;
                    break;

                case DialogChoice.Neutral:
                    this.NeutralText = text;
                    break;

                case DialogChoice.Positive:
                    this.PositiveText = text;
                    break;
            }
            return this;
        }


        public PromptConfig SetMaxLength(int maxLength)
        {
            this.MaxLength = maxLength;
            return this;
        }


        public PromptConfig SetText(string text)
        {
            this.Text = text;
            return this;
        }


        public PromptConfig SetPlaceholder(string placeholder)
        {
            this.Placeholder = placeholder;
            return this;
        }


        public PromptConfig SetInputMode(InputType inputType)
        {
            this.InputType = inputType;
            return this;
        }


        public PromptConfig SetOnTextChanged(Action<PromptTextChangedArgs> onChange)
        {
            this.OnTextChanged = onChange;
            return this;
        }
    }
}
