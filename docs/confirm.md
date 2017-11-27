# Alerts


## Config

## Theming/Defaults
- ConfirmConfig
    - DefaultYes
    - DefaultNo
    - DefaultOkText
    - DefaultCancelText

## Example

    IUserDialogs dialogs = UserDialogs.Instance;
    bool result = await dialogs.ConfirmAsync("Are you sure?", "Question", "Yes", "No");
