using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Pie.VisualStudio
{
    internal static class PieContentType
    {
        internal static class FileAndContentTypeDefinitions
        {
            [Export]
            [Name("pie")]
            [BaseDefinition("text")]
            internal static ContentTypeDefinition hidingContentTypeDefinition;

            [Export]
            [FileExtension(".pie")]
            [ContentType("pie")]
            internal static FileExtensionToContentTypeDefinition hiddenFileExtensionDefinition;
        }
    }
}
