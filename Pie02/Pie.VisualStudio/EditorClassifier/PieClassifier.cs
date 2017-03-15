//------------------------------------------------------------------------------
// <copyright file="PieClassifier.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Irony.Parsing;

namespace Pie.VisualStudio.EditorClassifier
{
    /// <summary>
    /// Classifier that classifies all text as an instance of the "PieClassifier" classification type.
    /// </summary>
    internal class PieClassifier : IClassifier
    {
        /// <summary>
        /// Classification type.
        /// </summary>
        private readonly IClassificationType ReservedToken;
        private readonly IClassificationType LiteralToken;
        private readonly IClassificationType CommentToken;
        private ITextBuffer buffer;
        List<ClassificationSpan> spans = new List<ClassificationSpan>();

        private Irony.Parsing.Parser parser = new Irony.Parsing.Parser(new Pie02CS.Rules.PieGrammar());

        /// <summary>
        /// Initializes a new instance of the <see cref="PieClassifier"/> class.
        /// </summary>
        /// <param name="registry">Classification registry.</param>
        internal PieClassifier(IClassificationTypeRegistryService registry, ITextBuffer buffer)
        {
            this.buffer = buffer;
            this.buffer.Changed += Buffer_Changed;
            
            this.ReservedToken = registry.GetClassificationType("PieReservedToken");
            this.LiteralToken = registry.GetClassificationType("PieLiteralToken");
            this.CommentToken = registry.GetClassificationType("PieCommentToken");
        }

        private void Buffer_Changed(object sender, TextContentChangedEventArgs e)
        {
            var changes = e.Changes;
            var args = new ClassificationChangedEventArgs(new SnapshotSpan(buffer.CurrentSnapshot, 0, buffer.CurrentSnapshot.GetText().Length));
            foreach(var change in changes)
            {
                if(change.Delta > 0)
                {
                    if(change.NewText.EndsWith("/") || change.NewText.EndsWith("\""))
                    {
                        ClassificationChanged(this, args);
                    }
                }
            }
        }

        bool firstTime;

        #region IClassifier

#pragma warning disable 67

        /// <summary>
        /// An event that occurs when the classification of a span of text has changed.
        /// </summary>
        /// <remarks>
        /// This event gets raised if a non-text change would affect the classification in some way,
        /// for example typing /* would cause the classification to change in C# without directly
        /// affecting the span.
        /// </remarks>
        public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;

#pragma warning restore 67

        /// <summary>
        /// Gets all the <see cref="ClassificationSpan"/> objects that intersect with the given range of text.
        /// </summary>
        /// <remarks>
        /// This method scans the given SnapshotSpan for potential matches for this classification.
        /// In this instance, it classifies everything and returns each span as a new ClassificationSpan.
        /// </remarks>
        /// <param name="span">The span currently being classified.</param>
        /// <returns>A list of ClassificationSpans that represent spans identified to be of this classification.</returns>
        public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
        {
            var text = buffer.CurrentSnapshot.GetText();
            var tree = parser.ScanOnly(text, "");
            var classSpans = new List<ClassificationSpan>();
            foreach (var token in tree.Tokens)
            {
                if (token.Terminal != null)
                {
                    int start = token.Location.Position;
                    int length = token.Length;
                    if ((token.Terminal.Flags & TermFlags.IsReservedWord) != 0)
                    {
                        classSpans.Add(new ClassificationSpan(new SnapshotSpan(buffer.CurrentSnapshot, new Span(start, length)), this.ReservedToken));
                    }
                    if ((token.Terminal.Flags & TermFlags.IsLiteral) != 0)
                    {
                        classSpans.Add(new ClassificationSpan(new SnapshotSpan(buffer.CurrentSnapshot, new Span(start, length)), this.LiteralToken));
                    }
                    if ((token.Category == TokenCategory.Comment))
                    {
                        classSpans.Add(new ClassificationSpan(new SnapshotSpan(buffer.CurrentSnapshot, new Span(start, length)), this.CommentToken));
                    }
                }
            }

            return classSpans;
        }



        #endregion
    }
}
