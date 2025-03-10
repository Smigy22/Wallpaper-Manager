// This source is subject to the Creative Commons Public License.
// Please see the README.MD file for more information.
// All other rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Drawing;

namespace WallpaperManager.Models
{
    /// <summary>
    ///   Defines common wallpaper attributes.
    /// </summary>
    [ContractClass(typeof(IWallpaperCommonAttributesContracts))]
    public interface IWallpaperCommonAttributes
    {
        /// <summary>
        ///   Gets or sets a <see cref="Boolean" /> indicating whether this wallpaper is activated.
        /// </summary>
        /// <value>
        ///   A <see cref="Boolean" /> indicating whether this wallpaper is activated.
        /// </value>
        /// <remarks>
        ///   The activated status of a wallpaper usually indicates if it should be automatically cycled or not.
        /// </remarks>
        bool IsActivated { get; set; }

        /// <summary>
        ///   Gets or sets a <see cref="Boolean" /> indicating whether this wallpaper represents a wallpaper for multiple screens.
        /// </summary>
        /// <value>
        ///   A <see cref="Boolean" /> indicating whether this wallpaper represents a wallpaper for multiple screens.
        /// </value>
        bool IsMultiscreen { get; set; }

        /// <summary>
        ///   Gets or sets the pick priority of this wallpaper.
        /// </summary>
        /// <value>
        ///   The pick priority of this wallpaper.
        /// </value>
        /// <remarks>
        ///   The pick priority usually represents the chance for the wallpaper of being automatically picked when cycling.
        /// </remarks>
        byte Priority { get; set; }

        /// <summary>
        ///   Gets or sets the start time of the range in which this wallpaper should only be cycled.
        /// </summary>
        /// <value>
        ///   The start time of the range in which this wallpaper should only be cycled.
        /// </value>
        /// <seealso cref="OnlyCycleBetweenStop">OnlyCycleBetweenStop Property</seealso>
        TimeSpan OnlyCycleBetweenStart { get; set; }

        /// <summary>
        ///   Gets or sets the end time of the range in which this wallpaper should only be cycled.
        /// </summary>
        /// <value>
        ///   The end time of the range in which this wallpaper should only be cycled.
        /// </value>
        /// <seealso cref="OnlyCycleBetweenStart">OnlyCycleBetweenStart Property</seealso>
        TimeSpan OnlyCycleBetweenStop { get; set; }

        /// <summary>
        ///   Gets or sets a value defining how this wallpaper should be placed when drawn on a screen.
        /// </summary>
        /// <value>
        ///   A value defining how this wallpaper should be placed when drawn on a screen.
        /// </value>
        /// <exception cref="ArgumentOutOfRangeException">
        ///   Attempted to set a value which is not represented by a constant of the <see cref="WallpaperPlacement" /> enumeration.
        /// </exception>
        WallpaperPlacement Placement { get; set; }

        /// <summary>
        ///   Gets or sets the horizontal and vertical placement offset the wallpaper should be drawn with.
        /// </summary>
        /// <value>
        ///   The horizontal and vertical placement offset the wallpaper should be drawn with.
        /// </value>
        Point Offset { get; set; }

        /// <summary>
        ///   Gets or sets the horizontal and vertical scale the wallpaper should be drawn with.
        /// </summary>
        /// <value>
        ///   The horizontal and vertical scale the wallpaper should be drawn with.
        /// </value>
        Point Scale { get; set; }

        /// <summary>
        ///   Gets or sets the effects the wallpaper should be drawn with.
        /// </summary>
        /// <value>
        ///   The effects the wallpaper should be drawn with.
        /// </value>
        WallpaperEffects Effects { get; set; }

        /// <summary>
        ///   Gets or sets the background color drawn for this wallpaper if it does not fill out the whole screen.
        /// </summary>
        /// <value>
        ///   background color drawn for this wallpaper if it does not fill out the whole screen.
        /// </value>
        Color BackgroundColor { get; set; }

        /// <summary>
        ///   Gets a collection of screen indexes where this wallpaper is not allowed to be cycled on.
        /// </summary>
        /// <value>
        ///   A collection of screen indexes where this wallpaper is not allowed to be cycled on.
        /// </value>
        Collection<int> DisabledScreens { get; }
    }

    [ContractClassFor(typeof(IWallpaperCommonAttributes))]
    internal abstract class IWallpaperCommonAttributesContracts : IWallpaperCommonAttributes
    {
        public abstract Color BackgroundColor { get; set; }
        public abstract Collection<int> DisabledScreens { get; }
        public abstract WallpaperEffects Effects { get; set; }
        public abstract bool IsActivated { get; set; }
        public abstract bool IsMultiscreen { get; set; }
        public abstract Point Offset { get; set; }
        public abstract TimeSpan OnlyCycleBetweenStart { get; set; }
        public abstract TimeSpan OnlyCycleBetweenStop { get; set; }
        public abstract WallpaperPlacement Placement { get; set; }
        public abstract byte Priority { get; set; }
        public abstract Point Scale { get; set; }

        /// <summary>
        ///   Checks whether all properties have valid values.
        /// </summary>
        [ContractInvariantMethod]
        private void CheckInvariants()
        {
            Contract.Invariant(this.DisabledScreens != null);
            Contract.Invariant(Enum.IsDefined(typeof(WallpaperEffects), this.Effects));
            Contract.Invariant(Enum.IsDefined(typeof(WallpaperPlacement), this.Placement));
        }
    }
}