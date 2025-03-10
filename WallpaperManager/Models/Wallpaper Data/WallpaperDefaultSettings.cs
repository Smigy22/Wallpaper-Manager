﻿// This source is subject to the Creative Commons Public License.
// Please see the README.MD file for more information.
// All other rights reserved.

using System;
using System.Diagnostics.Contracts;

namespace WallpaperManager.Models
{
    /// <summary>
    ///   Contains data to be applied to newly added wallpapers.
    /// </summary>
    /// <threadsafety static="true" instance="false" />
    public class WallpaperDefaultSettings : WallpaperSettingsBase
    {
        /// <summary>
        ///   <inheritdoc cref="AutoDetermineIsMultiscreen" select='../value/node()' />
        /// </summary>
        private bool autoDetermineIsMultiscreen;

        /// <summary>
        ///   <inheritdoc cref="AutoDeterminePlacement" select='../value/node()' />
        /// </summary>
        private bool autoDeterminePlacement;

        /// <summary>
        ///   Gets or sets a value indicating whether the <see cref="WallpaperSettingsBase.IsMultiscreen" /> property should be
        ///   determined automatically or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> whether the <see cref="WallpaperSettingsBase.IsMultiscreen" /> property should be determined
        ///   automatically; otherwise <c>false</c>.
        /// </value>
        public bool AutoDetermineIsMultiscreen
        {
            get { return this.autoDetermineIsMultiscreen; }
            set
            {
                this.autoDetermineIsMultiscreen = value;
                this.OnPropertyChanged("AutoDetermineIsMultiscreen");
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether the <see cref="WallpaperSettingsBase.Placement" />
        ///   property should be determined automatically or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> whether the <see cref="WallpaperSettingsBase.Placement" /> property should be
        ///   determined automatically; otherwise <c>false</c>.
        /// </value>
        public bool AutoDeterminePlacement
        {
            get { return this.autoDeterminePlacement; }
            set
            {
                this.autoDeterminePlacement = value;
                this.OnPropertyChanged("AutoDeterminePlacement");
            }
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="WallpaperDefaultSettings" /> class.
        /// </summary>
        public WallpaperDefaultSettings()
        {
            this.autoDetermineIsMultiscreen = true;
            this.autoDeterminePlacement = true;
        }

        #region ICloneable Implementation, IAssignable Implementation
        /// <inheritdoc />
        public override object Clone()
        {
            WallpaperDefaultSettings clonedInstance = new WallpaperDefaultSettings();

            // Clone all fields defined by WallpaperSettingsBase.
            base.Clone(clonedInstance);

            clonedInstance.autoDetermineIsMultiscreen = this.AutoDetermineIsMultiscreen;
            clonedInstance.autoDeterminePlacement = this.AutoDeterminePlacement;

            return clonedInstance;
        }

        /// <summary>
        ///   Assigns all member values of this instance to the respective members of the given instance.
        /// </summary>
        /// <param name="other">
        ///   The target instance to assign to.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="other" /> is <c>null</c>.
        /// </exception>
        protected override void AssignTo(WallpaperSettingsBase other)
        {
            if (other == null) throw new ArgumentNullException();

            // Assign all members defined by WallpaperSettingsBase.
            base.AssignTo(other);

            WallpaperDefaultSettings defaultSettingsInstance = (other as WallpaperDefaultSettings);
            if (defaultSettingsInstance != null)
            {
                defaultSettingsInstance.AutoDetermineIsMultiscreen = this.AutoDetermineIsMultiscreen;
                defaultSettingsInstance.AutoDeterminePlacement = this.AutoDeterminePlacement;
            }
        }
        #endregion
    }
}