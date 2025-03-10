// This source is subject to the Creative Commons Public License.
// Please see the README.MD file for more information.
// All other rights reserved.

using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.IO;
using System.Security;
using Common;
using Common.Presentation;
using WallpaperManager.Models;

namespace WallpaperManager.ViewModels
{
    /// <commondoc select='WrappingViewModels/General/*' params="WrappedType=WallpaperTextOverlay" />
    /// <threadsafety static="true" instance="false" />
    public class ConfigurationVM : INotifyPropertyChanged
    {
        /// <summary>
        ///   <inheritdoc cref="SelectedScreenIndex" select='../value/node()' />
        /// </summary>
        private int selectedScreenIndex;

        /// <summary>
        ///   <inheritdoc cref="StartWithWindows" select='../value/node()' />
        /// </summary>
        private bool? startWithWindows;

        /// <summary>
        ///   Gets the <see cref="WallpaperManager.Models.GeneralConfig" /> instance wrapped by this View Model.
        /// </summary>
        /// <remarks>
        ///   <para>
        ///     This instance is the dummy <see cref="GeneralConfig" /> instance. It is a clone of the original
        ///     <see cref="GeneralConfig" /> object and gets modified by the attached Views.
        ///   </para>
        ///   <para>
        ///     Execute the <see cref="ApplySettingsCommand">ApplySettings Command</see> to reassign the data of this dummy back to
        ///     the original <see cref="GeneralConfig" /> instance.
        ///   </para>
        /// </remarks>
        /// <value>
        ///   The <see cref="WallpaperManager.Models.Configuration" /> instance wrapped by this View Model.
        ///   It is a cloned version of the original <see cref="WallpaperManager.Models.Configuration" />.
        /// </value>
        public GeneralConfig Configuration { get; }

        /// <summary>
        ///   Gets the original <see cref="GeneralConfig" /> instance which gets updated by the new settings when the
        ///   <see cref="ApplySettingsCommand" /> is executed.
        /// </summary>
        /// <value>
        ///   The original <see cref="GeneralConfig" /> instance.
        /// </value>
        public GeneralConfig OriginalConfiguration { get; }

        /// <inheritdoc cref="WallpaperManager.Models.GeneralConfig.StartWithWindows" />
        public bool? StartWithWindows
        {
            get { return this.startWithWindows; }
            set
            {
                this.startWithWindows = value;
                this.OnPropertyChanged("StartWithWindows");
            }
        }

        /// <summary>
        ///   Gets or sets the index of the selected screen.
        /// </summary>
        /// <value>
        ///   The index of the selected screen.
        /// </value>
        public int SelectedScreenIndex
        {
            get { return this.selectedScreenIndex; }
            set
            {
                this.selectedScreenIndex = value;

                this.OnPropertyChanged("SelectedScreenIndex");
                this.OnPropertyChanged("SelectedScreenNumber");
                this.OnPropertyChanged("SelectedScreenSettings");
            }
        }

        /// <summary>
        ///   Gets the number of the selected screen.
        /// </summary>
        /// <value>
        ///   The number of the selected screen.
        /// </value>
        public int SelectedScreenNumber
        {
            get { return this.SelectedScreenIndex + 1; }
        }

        /// <summary>
        ///   Gets the <see cref="ScreenSettings" /> of the selected screen.
        /// </summary>
        /// <value>
        ///   The <see cref="ScreenSettings" /> of the selected screen.
        /// </value>
        public ScreenSettings SelectedScreenSettings
        {
            get { return this.Configuration.ScreensSettings[this.SelectedScreenIndex]; }
        }

        /// <summary>
        ///   Occurs when closing of the bound Views is requested.
        /// </summary>
        /// <seealso cref="RequestCloseEventArgs">RequestCloseEventArgs Class</seealso>
        public event EventHandler<RequestCloseEventArgs> RequestClose;

        /// <commondoc select='ViewModels/Events/UnhandledCommandException/*' />
        public event EventHandler<CommandExceptionEventArgs> UnhandledCommandException;

        /// <summary>
        ///   Initializes a new instance of the <see cref="ConfigurationVM" /> class.
        /// </summary>
        /// <param name="configuration">
        ///   The original <see cref="GeneralConfig" /> instance.
        /// </param>
        /// <seealso cref="GeneralConfig">GeneralConfig Class</seealso>
        public ConfigurationVM(GeneralConfig configuration)
        {
            this.OriginalConfiguration = configuration;

            // Create the dummy by cloning the original configuration.
            this.Configuration = (GeneralConfig)configuration.Clone();

            try
            {
                this.startWithWindows = configuration.StartWithWindows;
            }
            catch (Exception exception)
            {
                if ((exception is UnauthorizedAccessException) || (exception is SecurityException) || (exception is IOException))
                    this.startWithWindows = null;
                else
                    throw;
            }
        }

        /// <summary>
        ///   Checks whether all properties have valid values.
        /// </summary>
        [ContractInvariantMethod]
        private void CheckInvariants()
        {
            Contract.Invariant(this.Configuration != null);
            Contract.Invariant(this.OriginalConfiguration != null);
            Contract.Invariant(this.SelectedScreenSettings != null);
            Contract.Invariant(this.ApplySettingsCommand != null);
            Contract.Invariant(this.CancelCommand != null);
        }

        /// <summary>
        ///   Called when closing of the bound Views is requested.
        /// </summary>
        /// <remarks>
        ///   This method raises the <see cref="RequestClose">RequestClose Event</see>.
        /// </remarks>
        /// <param name="e">
        ///   The arguments for the event.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="e" /> is <c>null</c>.
        /// </exception>
        /// <seealso cref="RequestClose">RequestClose Event</seealso>
        /// <seealso cref="RequestCloseEventArgs">RequestCloseEventArgs Class</seealso>
        protected virtual void OnRequestClose(RequestCloseEventArgs e)
        {
            if (e == null) throw new ArgumentNullException();

            this.RequestClose?.Invoke(this, e);
        }

        /// <commondoc select='ViewModels/Methods/OnUnhandledCommandException/*' />
        protected virtual void OnUnhandledCommandException(CommandExceptionEventArgs e)
        {
            if (e == null) throw new ArgumentNullException();

            this.UnhandledCommandException?.ReverseInvoke(this, e);
        }

        #region Command: ApplySettings
        /// <summary>
        ///   <inheritdoc cref="ApplySettingsCommand" select='../value/node()' />
        /// </summary>
        private DelegateCommand applySettingsCommand;

        /// <summary>
        ///   Gets the Apply Settings <see cref="DelegateCommand" />.
        /// </summary>
        /// <value>
        ///   The Apply Settings <see cref="DelegateCommand" />.
        /// </value>
        /// <seealso cref="ApplySettingsCommand_CanExecute">ApplySettingsCommand_CanExecute Method</seealso>
        /// <seealso cref="ApplySettingsCommand_Execute">ApplySettingsCommand_Execute Method</seealso>
        /// <seealso cref="Configuration">Configuration Class</seealso>
        public DelegateCommand ApplySettingsCommand
        {
            get
            {
                if (this.applySettingsCommand == null)
                    this.applySettingsCommand = new DelegateCommand(this.ApplySettingsCommand_Execute, this.ApplySettingsCommand_CanExecute);

                return this.applySettingsCommand;
            }
        }

        /// <summary>
        ///   Determines if <see cref="ApplySettingsCommand" /> can be executed.
        /// </summary>
        /// <returns>
        ///   A <see cref="bool" /> indicating whether the command can be executed or not.
        /// </returns>
        /// <seealso cref="ApplySettingsCommand" />
        protected bool ApplySettingsCommand_CanExecute()
        {
            return true;
        }

        /// <summary>
        ///   Called when <see cref="ApplySettingsCommand" /> is executed.
        ///   Assigns all settings of the <see cref="Configuration" /> instance to the <see cref="OriginalConfiguration" />
        ///   instance.
        /// </summary>
        /// <seealso cref="ApplySettingsCommand" />
        /// <seealso cref="Configuration">Configuration Class</seealso>
        protected void ApplySettingsCommand_Execute()
        {
            try
            {
                this.Configuration.AssignTo(this.OriginalConfiguration);

                if (this.StartWithWindows != null)
                    this.Configuration.StartWithWindows = this.StartWithWindows.Value;

                this.OnRequestClose(new RequestCloseEventArgs(true));
            }
            catch (Exception exception)
            {
                this.OnUnhandledCommandException(new CommandExceptionEventArgs(this.ApplySettingsCommand, exception));
            }
        }
        #endregion

        #region Command: Cancel
        /// <summary>
        ///   <inheritdoc cref="CancelCommand" select='../value/node()' />
        /// </summary>
        private DelegateCommand cancelCommand;

        /// <summary>
        ///   Gets the Cancel <see cref="DelegateCommand" />.
        /// </summary>
        /// <value>
        ///   The Cancel <see cref="DelegateCommand" />.
        /// </value>
        /// <seealso cref="CancelCommand_CanExecute">CancelCommand_CanExecute Method</seealso>
        /// <seealso cref="CancelCommand_Execute">CancelCommand_Execute Method</seealso>
        public DelegateCommand CancelCommand
        {
            get
            {
                if (this.cancelCommand == null)
                    this.cancelCommand = new DelegateCommand(this.CancelCommand_Execute, this.CancelCommand_CanExecute);

                return this.cancelCommand;
            }
        }

        /// <summary>
        ///   Determines if <see cref="CancelCommand" /> can be executed.
        /// </summary>
        /// <returns>
        ///   A <see cref="bool" /> indicating whether the command can be executed or not.
        /// </returns>
        /// <seealso cref="CancelCommand" />
        protected bool CancelCommand_CanExecute()
        {
            return true;
        }

        /// <summary>
        ///   Called when <see cref="CancelCommand" /> is executed and requests the attached views to cancel the configuration
        ///   process.
        /// </summary>
        /// <seealso cref="CancelCommand" />
        protected void CancelCommand_Execute()
        {
            try
            {
                this.OnRequestClose(new RequestCloseEventArgs(false));
            }
            catch (Exception exception)
            {
                this.OnUnhandledCommandException(new CommandExceptionEventArgs(this.CancelCommand, exception));
            }
        }
        #endregion

        #region INotifyPropertyChanged Implementation
        /// <inheritdoc cref="INotifyPropertyChanged.PropertyChanged" />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <commondoc select='INotifyPropertyChanged/Methods/OnPropertyChanged/*' />
        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}