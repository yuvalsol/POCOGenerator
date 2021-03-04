using System;
using System.Drawing;

namespace POCOGenerator.POCOWriters
{
    public interface ISyntaxHighlight
    {
        Color Text { get; set; }
        Color Keyword { get; set; }
        Color UserType { get; set; }
        Color String { get; set; }
        Color Comment { get; set; }
        Color Error { get; set; }
        Color Background { get; set; }
    }
}
