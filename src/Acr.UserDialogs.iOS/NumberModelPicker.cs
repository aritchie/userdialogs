using System;
using System.Collections.Generic;
using UIKit;

namespace Acr.UserDialogs
{
    public class NumberModelPicker : UIPickerViewModel
    {
        #region fields
        public List<int> Items { get; private set; }

        private int _selectedIndex = 0;

        public int SelectedItem
        {
            get { return Items[_selectedIndex]; }
        }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
        }

        #endregion

        #region events
        public event EventHandler<EventArgs> ValueChanged;
        #endregion
        #region constructor
        public NumberModelPicker(int minNumber=0, int maxNumber =10, int selectedNumber = 0)
        {
            Items = GetItems(minNumber, maxNumber);
            _selectedIndex = Items.IndexOf(selectedNumber);


        }
        #endregion

        #region utils
        private List<int> GetItems(int minNumber, int maxNumber)
        {
            List<int> generatedItems = new List<int>();

            for (int i = minNumber; i <= maxNumber; i++)
            {
                generatedItems.Add(i);
            }

            return generatedItems;

        }
        #endregion

        #region override Picker
        public override nint GetRowsInComponent(UIPickerView picker, nint component)
        {
            return Items.Count;
        }

        public override string GetTitle(UIPickerView picker, nint row, nint component)
        {
            return Items[(int)row].ToString();
        }

        public override nint GetComponentCount(UIPickerView picker)
        {
            return 1;
        }


        public override void Selected(UIPickerView picker, nint row, nint component)
        {
            _selectedIndex = (int)row;
            if (ValueChanged != null)
            {
                ValueChanged(this, new EventArgs());
            }
        }
        #endregion

    }
}
