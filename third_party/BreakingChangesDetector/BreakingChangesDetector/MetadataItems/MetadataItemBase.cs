﻿/*
    MIT License

    Copyright(c) 2014-2018 Infragistics, Inc.
    Copyright 2018 Google LLC

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
*/


namespace BreakingChangesDetector.MetadataItems
{
    /// <summary>
    /// Abstract base sealed class for all classes representing and entity obtained from reflection metadata.
    /// </summary>
    public abstract class MetadataItemBase
    {
        /// <summary>
        /// Performs the specified visitor's functionality on this instance.
        /// </summary>
        /// <param name="visitor">The visitor whose functionality should be performed on the instance.</param>
        public abstract void Accept(MetadataItemVisitor visitor);

        public abstract MetadataResolutionContext Context { get; }

        /// <summary>
        /// Gets the name to use for this item in messages.
        /// </summary>
        public abstract string DisplayName { get; }

        internal virtual bool DoesMatch(MetadataItemBase other) =>
            MetadataItemKind == other.MetadataItemKind && DisplayName == other.DisplayName;

        /// <summary>
        /// Gets the type of item the instance represents.
        /// </summary>
        public abstract MetadataItemKinds MetadataItemKind { get; }

        /// <summary>
        /// Gets the <see cref="DisplayName"/> of the item.
        /// </summary>
        /// <returns></returns>
        public sealed override string ToString() => DisplayName;
    }
}
