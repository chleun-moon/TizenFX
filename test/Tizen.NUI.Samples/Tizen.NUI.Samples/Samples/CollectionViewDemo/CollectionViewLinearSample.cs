﻿using System.Collections.Generic;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;
using Tizen.NUI.Binding;
using System.ComponentModel;
using System;

namespace Tizen.NUI.Samples
{
    public class CollectionViewLinearSample : IExample
    {
        CollectionView colView;
        int itemCount = 500;
        string selectedItem;
        ItemSelectionMode selMode;

        public void Activate()
        {
            Window window = NUIApplication.GetDefaultWindow();

            var myViewModelSource = new GalleryViewModel(itemCount);
            selMode = ItemSelectionMode.SingleSelection;
            DefaultTitleItem myTitle = new DefaultTitleItem();
            myTitle.Text = "Linear Sample Count["+itemCount+"]";
            //Set Width Specification as MatchParent to fit the Item width with parent View.
            myTitle.WidthSpecification = LayoutParamPolicies.MatchParent;

            colView = new CollectionView()
            {
                ItemsSource = myViewModelSource,
                ItemsLayouter = new LinearLayouter(),
                ItemTemplate = new DataTemplate(() =>
                {
                    var rand = new Random();
                    DefaultLinearItem item = new DefaultLinearItem();
                    //Set Width Specification as MatchParent to fit the Item width with parent View.
                    item.WidthSpecification = LayoutParamPolicies.MatchParent;

                    //Decorate Label
                    item.Label.SetBinding(TextLabel.TextProperty, "ViewLabel");
                    item.Label.HorizontalAlignment = HorizontalAlignment.Begin;

                    //Decorate SubLabel
                    if ((rand.Next() % 2) == 0)
                    {
                        item.SubLabel.SetBinding(TextLabel.TextProperty, "Name");
                        item.SubLabel.HorizontalAlignment = HorizontalAlignment.Begin;
                    }

                    //Decorate Icon
                    item.Icon.SetBinding(ImageView.ResourceUrlProperty, "ImageUrl");
                    item.Icon.WidthSpecification = 48;
                    item.Icon.HeightSpecification = 48;

                    //Decorate Extra RadioButton.
                    //[NOTE] This is sample of RadioButton usage in CollectionView.
                    // RadioButton change their selection by IsSelectedProperty bindings with
                    // SelectionChanged event with SingleSelection ItemSelectionMode of CollectionView.
                    // be aware of there are no RadioButtonGroup.
                    item.Extra = new RadioButton();
                    //FIXME : SetBinding in RadioButton crashed as Sensitive Property is disposed.
                    //item.Extra.SetBinding(RadioButton.IsSelectedProperty, "Selected");
                    item.Extra.WidthSpecification = 48;
                    item.Extra.HeightSpecification = 48;

                    return item;
                }),
                Header = myTitle,
                ScrollingDirection = ScrollableBase.Direction.Vertical,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
				SelectionMode = selMode
            };
            colView.SelectionChanged += SelectionEvt;

            window.Add(colView);

        }

        public void SelectionEvt(object sender, SelectionChangedEventArgs ev)
        {
            //Tizen.Log.Debug("NUI", "LSH :: SelectionEvt called");

            //SingleSelection Only have 1 or nil object in the list.
            foreach (object item in ev.PreviousSelection)
            {
                if (item == null) break;
                Gallery unselItem = (Gallery)item;

                unselItem.Selected = false;
                selectedItem = null;
                //Tizen.Log.Debug("NUI", "LSH :: Unselected: {0}", unselItem.ViewLabel);
            }
            foreach (object item in ev.CurrentSelection)
            {
                if (item == null) break;
                Gallery selItem = (Gallery)item;
                selItem.Selected = true;
                selectedItem = selItem.Name;
                //Tizen.Log.Debug("NUI", "LSH :: Selected: {0}", selItem.ViewLabel);
            }
            if (colView.Header != null && colView.Header is DefaultTitleItem)
            {
                DefaultTitleItem title = (DefaultTitleItem)colView.Header;
                title.Text = "Linear Sample Count[" + itemCount + (selectedItem != null ? "] Selected [" + selectedItem + "]" : "]");
            }
        }
        public void Deactivate()
        {
            if (colView != null)
            {
                colView.Dispose();
            }
        }
    }
}
