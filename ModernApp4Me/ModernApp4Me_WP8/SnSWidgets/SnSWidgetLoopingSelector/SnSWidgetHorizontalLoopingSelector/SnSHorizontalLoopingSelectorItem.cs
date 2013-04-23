﻿// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using ModernApp4Me_WP8.SnSWidgets.SnSWidgetLoopingSelector.SnSWidgetLoopingSelectorCore;

namespace ModernApp4Me_WP8.SnSWidgets.SnSWidgetLoopingSelector.SnSWidgetHorizontalLoopingSelector
{
    /// <summary>
    /// The items that will be contained in the LoopingSelector.
    /// </summary>
    [TemplatePart(Name = TransformPartName, Type = typeof(TranslateTransform))]
    [TemplateVisualState(GroupName = CommonGroupName, Name = NormalStateName)]
    [TemplateVisualState(GroupName = CommonGroupName, Name = ExpandedStateName)]
    [TemplateVisualState(GroupName = CommonGroupName, Name = SelectedStateName)]
    public class SnSHorizontalLoopingSelectorItem : ContentControl
    {
        private const string TransformPartName = "Transform";

        private const string CommonGroupName = "Common";
        private const string NormalStateName = "Normal";
        private const string ExpandedStateName = "Expanded";
        private const string SelectedStateName = "Selected";

        private bool _shouldClick;

        /// <summary>
        /// The states that this can be in.
        /// </summary>
        internal enum State 
        { 
            /// <summary>
            /// Not visible
            /// </summary>
            Normal, 
            /// <summary>
            /// Visible
            /// </summary>
            Expanded, 
            /// <summary>
            /// Selected
            /// </summary>
            Selected 
        };
        private State _state;

        /// <summary>
        /// Create a new LoopingSelectorItem.
        /// </summary>
        public SnSHorizontalLoopingSelectorItem()
        {
            DefaultStyleKey = typeof(SnSHorizontalLoopingSelectorItem);
            MouseLeftButtonDown += LoopingSelectorItem_MouseLeftButtonDown;
            MouseLeftButtonUp += LoopingSelectorItem_MouseLeftButtonUp;
            LostMouseCapture += LoopingSelectorItem_LostMouseCapture;
            GestureService.GetGestureListener(this).Tap += LoopingSelectorItem_Tap;
        }

        /// <summary>
        /// Put this item into a new state.
        /// </summary>
        /// <param name="newState">The new state.</param>
        /// <param name="useTransitions">Flag indicating that transitions should be used when going to the new state.</param>
        internal void SetState(State newState, bool useTransitions)
        {
            if (_state != newState)
            {
                _state = newState;
                switch (_state)
                {
                    case State.Normal:
                        VisualStateManager.GoToState(this, NormalStateName, useTransitions);
                        break;
                    case State.Expanded:
                        VisualStateManager.GoToState(this, ExpandedStateName, useTransitions);
                        break;
                    case State.Selected:
                        VisualStateManager.GoToState(this, SelectedStateName, useTransitions);
                        break;
                }
            }
        }

        /// <summary>
        /// Returns the current state.
        /// </summary>
        /// <returns>The current state.</returns>
        internal State GetState() { return _state; }

        void LoopingSelectorItem_Tap(object sender, Microsoft.Phone.Controls.GestureEventArgs e)
        {
            e.Handled = true;
        }

        void LoopingSelectorItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CaptureMouse();
            _shouldClick = true;
        }

        void LoopingSelectorItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();

            if (_shouldClick)
            {
                _shouldClick = false;
                SnSSafeRaise.Raise(Click, this);
            }
        }

        void LoopingSelectorItem_LostMouseCapture(object sender, MouseEventArgs e)
        {
            _shouldClick = false;
        }

        /// <summary>
        /// The Click event. This is needed because there is no gesture for touch-down, pause 
        /// longer than the Hold time, and touch-up. Tap will not be raise, and Hold is not 
        /// adequate.
        /// </summary>
        public event EventHandler<EventArgs> Click;

        /// <summary>
        /// Override of OnApplyTemplate
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Transform = GetTemplateChild(TransformPartName) as TranslateTransform ?? new TranslateTransform();
        }

        internal SnSHorizontalLoopingSelectorItem Previous { get; private set; }
        internal SnSHorizontalLoopingSelectorItem Next { get; private set; }

        internal void Remove()
        {
            if (Previous != null)
            {
                Previous.Next = Next;
            }
            if (Next != null)
            {
                Next.Previous = Previous;
            }
            Next = Previous = null;
        }

        internal void InsertAfter(SnSHorizontalLoopingSelectorItem after)
        {
            Next = after.Next;
            Previous = after;

            if (after.Next != null)
            {
                after.Next.Previous = this;
            }

            after.Next = this;
        }

        internal void InsertBefore(SnSHorizontalLoopingSelectorItem before)
        {
            Next = before;
            Previous = before.Previous;

            if (before.Previous != null)
            {
                before.Previous.Next = this;
            }

            before.Previous = this;
        }

        internal TranslateTransform Transform { get; private set; }
    }

}
