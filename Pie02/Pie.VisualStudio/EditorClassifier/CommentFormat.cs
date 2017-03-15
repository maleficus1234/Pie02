//------------------------------------------------------------------------------
// <copyright file="PieClassifierFormat.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Pie.VisualStudio.EditorClassifier
{
    /// <summary>
    /// Defines an editor format for the PieClassifier type that has a purple background
    /// and is underlined.
    /// </summary>
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "PieCommentToken")]
    [Name("PieCommentToken")]
    [UserVisible(true)] // This should be visible to the end user
    [Order(Before = Priority.High)] // Set the priority to be after the default classifiers
    internal sealed class CommentFormat : ClassificationFormatDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReservedTokenFormat"/> class.
        /// </summary>
        public CommentFormat()
        {
            this.DisplayName = "PieCommentToken"; // Human readable version of the name
            this.ForegroundColor = Color.FromRgb(0, 140, 0);

        }
    }
}
