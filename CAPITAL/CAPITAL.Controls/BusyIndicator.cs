using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace CAPITAL.Controls
{
    /// <summary>
    /// A control to provide a visual indicator when an application is busy.
    /// </summary>
    [TemplateVisualState(Name = VisualStates.StateVisible, GroupName = VisualStates.GroupVisibility)]
    [TemplateVisualState(Name = VisualStates.StateHidden, GroupName = VisualStates.GroupVisibility)]
    public class BusyIndicator : ContentControl
    {
        #region Fields

        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(
            "IsBusy",
            typeof(bool),
            typeof(BusyIndicator),
            new PropertyMetadata(false, (d, e) => ((BusyIndicator)d).OnIsBusyChanged(e)));

        public static readonly DependencyProperty BusyTextProperty = DependencyProperty.Register(
            "BusyText",
            typeof(string),
            typeof(BusyIndicator),
            new PropertyMetadata(null));

        public static readonly DependencyProperty HideApplicationBarProperty = DependencyProperty.Register(
            "HideApplicationBar",
            typeof(bool),
            typeof(BusyIndicator),
            new PropertyMetadata(true, null));

        #endregion

        #region Constructor
        public BusyIndicator():base()
        {
            this.DefaultStyleKey = typeof(BusyIndicator);
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the busy indicator should show.
        /// </summary>
        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating the busy content to display to the user.
        /// </summary>
        public string BusyText
        {
            get { return (string)this.GetValue(BusyTextProperty); }
            set { SetValue(BusyTextProperty, value); }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Overrides the OnApplyTemplate method.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.ChangeVisualState(false);

            this.OnIsBusyChanged(new DependencyPropertyChangedEventArgs());
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Changes the control's visual state(s).
        /// </summary>
        /// <param name="useTransitions">True, if state transitions should be used.</param>
        protected virtual void ChangeVisualState(bool useTransitions)
        {
            VisualStateManager.GoToState(this, this.IsBusy ? VisualStates.StateVisible : VisualStates.StateHidden, useTransitions);
        }

        /// <summary>
        /// IsBusyProperty property changed handler.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnIsBusyChanged(DependencyPropertyChangedEventArgs e)
        {
            this.ChangeVisualState(true);
        }

        #endregion

        #region Helper Classes

        /// <summary>
        /// Names and helpers for visual states in the controls.
        /// </summary>
        private static class VisualStates
        {
            /// <summary>Visible state name for BusyIndicator.</summary>
            public const string StateVisible = "Visible";

            /// <summary>Hidden state name for BusyIndicator.</summary>
            public const string StateHidden = "Hidden";

            /// <summary>BusyDisplay group.</summary>
            public const string GroupVisibility = "VisibilityStates";
        }

        #endregion
    }
}