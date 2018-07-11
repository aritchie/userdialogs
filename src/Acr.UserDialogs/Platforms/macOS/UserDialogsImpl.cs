﻿using System;
using AppKit;
using CoreGraphics;


namespace Acr.UserDialogs
{
    public class UserDialogsImpl : AbstractUserDialogs
    {
        readonly Func<NSViewController> viewControllerFunc;
        
        public UserDialogsImpl() : this(() => NSApplication.SharedApplication.MainWindow.ContentViewController)
        {
        }
        
        public UserDialogsImpl(Func<NSViewController> viewControllerFunc)
        {
            this.viewControllerFunc = viewControllerFunc;
        }

        public override IDisposable Alert(AlertConfig config)
        {
            var alert = new NSAlert
            {
                MessageText = config.Message
            };
            alert.AddButton(config.OkText);
            alert.RunModal();
            config.OnAction?.Invoke();
            return alert;
        }


        public override IDisposable ActionSheet(ActionSheetConfig config)
        {
            var alert = new NSAlert
            {
                MessageText = config.Message
            };
            foreach (var opt in config.Options)
            {
                var btn = alert.AddButton(opt.Text);
                //if (opt.ItemIcon != null)
                //    btn.Image = opt.ItemIcon.ToNative();
            }
            var actionIndex = alert.RunSheetModal(null); // TODO: get top NSWindow
            config.Options[(int)actionIndex].Action?.Invoke();
            return alert;
        }


        public override IDisposable Confirm(ConfirmConfig config)
        {
            var alert = new NSAlert
            {
                MessageText = config.Message
            };
            alert.AddButton(config.OkText);
            alert.AddButton(config.CancelText);
            var actionIndex = alert.RunModal();
            config.OnAction?.Invoke(actionIndex == 0);

            return alert;
        }


        public override IDisposable DatePrompt(DatePromptConfig config)
        {
            throw new NotImplementedException();
        }


        public override IDisposable TimePrompt(TimePromptConfig config)
        {
            throw new NotImplementedException();
        }


        public override IDisposable Login(LoginConfig config)
        {
            var txt = new NSTextField(new CGRect(0, 0, 300, 20));
            var alert = new NSAlert
            {
                AccessoryView = txt,
                MessageText = config.Message
            };
            alert.AddButton(config.OkText);
            if (config.CancelText != null)
                alert.AddButton(config.CancelText);

            var actionIndex = alert.RunModal();
            //config.OnAction?.Invoke(new LoginResult(actionIndex == 0, ));

            return alert;
        }


        public override IDisposable Prompt(PromptConfig config)
        {
            var txt = new NSTextField(new CGRect(0, 0, 300, 20));
            var alert = new NSAlert
            {
                AccessoryView = txt,
                MessageText = config.Message
            };
            alert.AddButton(config.OkText);
            if (config.CancelText != null)
                alert.AddButton(config.CancelText);

            var actionIndex = alert.RunModal();
            config.OnAction?.Invoke(new PromptResult(actionIndex == 0, txt.StringValue));

            return alert;
        }


        public override IDisposable Toast(ToastConfig config)
        {
            var alert = new NSAlert
            {
                MessageText = config.Message
            };
            alert.RunModal();
            return alert;
        }


        protected override IProgressDialog CreateDialogInstance(ProgressDialogConfig config)
        {
            throw new NotImplementedException();
        }
    }
}
