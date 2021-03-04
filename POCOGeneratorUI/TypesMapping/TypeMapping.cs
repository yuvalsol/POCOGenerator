using System;
using System.Collections.Generic;
using System.Drawing;

namespace POCOGeneratorUI.TypesMapping
{
    internal class TypeMapping
    {
        public IEnumerable<TypeMappingPart> FromType { get; private set; }
        public IEnumerable<TypeMappingPart> ToType { get; private set; }

        public TypeMapping(IEnumerable<TypeMappingPart> fromType, IEnumerable<TypeMappingPart> toType)
        {
            FromType = fromType;
            ToType = toType;
        }

        public TypeMapping(string fromTypeText, Color fromTypeSyntaxColor, string toTypeText, Color toTypeSyntaxColor)
        {
            FromType = new TypeMappingPart[] { new TypeMappingPart(fromTypeText, fromTypeSyntaxColor) };
            ToType = new TypeMappingPart[] { new TypeMappingPart(toTypeText, toTypeSyntaxColor) };
        }

        public TypeMapping(string fromTypeText, Color fromTypeSyntaxColor, params TypeMappingPart[] toType)
        {
            FromType = new TypeMappingPart[] { new TypeMappingPart(fromTypeText, fromTypeSyntaxColor) };
            ToType = toType;
        }
    }

    internal class TypeMappingPart
    {
        public string Text { get; private set; }
        public Color SyntaxColor { get; private set; }

        public TypeMappingPart(string text, Color syntaxColor)
        {
            Text = text;
            SyntaxColor = syntaxColor;
        }
    }
}
