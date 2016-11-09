using System;
using System.Drawing;

namespace Acr.UserDialogs
{
    public class PromptConfig : IAndroidStyleDialogConfig
    {
        public static DialogButton DefaultPositive { get; } = new DialogButton(DialogChoice.Positive, "Ok", null, false);
        public static DialogButton DefaultNeutral { get; } = new DialogButton(DialogChoice.Neutral, "Cancel", null, false);
        public static DialogButton DefaultNegative { get; } = new DialogButton(DialogChoice.Negative, "Remove", null, false);
        public static int? DefaultAndroidStyleId { get; set; }
        public static int? DefaultMaxLength { get; set; }

        public string Title { get; set; }
        public string Message { get; set; }
        public string Text { get; set; }
        public Color? BackgroundColor { get; set; }
        public Action<DialogResult<string>> OnAction { get; set; }


        public DialogButton Positive { get; } = new DialogButton(DialogChoice.Positive, DefaultPositive.Text, DefaultPositive.TextColor, true);
        public DialogButton Neutral { get; } = new DialogButton(DialogChoice.Neutral, DefaultNeutral.Text, DefaultNeutral.TextColor, true);
        public DialogButton Negative { get; } = new DialogButton(DialogChoice.Negative, DefaultNegative.Text, DefaultNegative.TextColor, false);
        public string Placeholder { get; set; }
        public int? MaxLength { get; set; } = DefaultMaxLength;
        public int? AndroidStyleId { get; set; }
        public InputType InputType { get; set; } = InputType.Default;
        public Action<PromptTextChangedArgs> OnTextChanged { get; set; }


        public PromptConfig SetText(DialogChoice choice, string text = null)
        {
            switch (choice)
            {
                case DialogChoice.Negative:
                    this.Negative.Text = text;
                    this.Negative.IsVisible = true;
                    break;

                case DialogChoice.Neutral:
                    this.Neutral.Text = text;
                    this.Neutral.IsVisible = true;
                    break;

                case DialogChoice.Positive:
                    this.Neutral.Text = text;
                    this.Neutral.IsVisible = true;
                    break;
            }
            return this;            
        }


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


        public PromptConfig SetInputType(InputType inputType)
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
