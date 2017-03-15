//------------------------------------------------------------------------------
// <copyright file="PieClassifierClassificationDefinition.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Pie.VisualStudio.EditorClassifier
{
    /// <summary>
    /// Classification type definition export for PieClassifier
    /// </summary>
    internal static class PieClassifierClassificationDefinition
    {
        // This disables "The field is never used" compiler's warning. Justification: the field is used by MEF.
#pragma warning disable 169

        /// <summary>
        /// Defines the "PieClassifier" classification type.
        /// </summary>
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("PieReservedToken")]
        private static ClassificationTypeDefinition typeDefinition;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("PieLiteralToken")]
        private static ClassificationTypeDefinition literalToken;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name("PieCommentToken")]
        private static ClassificationTypeDefinition commentToken;

#pragma warning restore 169
    }
}
