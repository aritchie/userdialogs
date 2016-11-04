# Prompt


## Config

## Theming/Defaults

- PromptConfig
    - DefaultOkText
    - DefaultCancelText

## Additional Functionality
```csharp
new PromptConfig
{
    OnTextChanged = args => {
        args.IsValid = bool; // setting this to false will disable the OK/Positive button
        args.Text = ""; // you can read the current value as well as replace the textbox value here
    }
}
```

Text Max Length

Input Types