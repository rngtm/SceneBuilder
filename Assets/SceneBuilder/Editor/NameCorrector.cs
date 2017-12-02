///-------------------------------------
/// SceneBuilder
/// @ 2017 RNGTM(https://github.com/rngtm)
///-------------------------------------
namespace EditorSceneBuilder
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using UnityEditor;
    using UnityEditor.Animations;
    using UnityEngine;
    using System.Text.RegularExpressions;

    /// <summary>
    /// 不正な名前の修正を行う
    /// </summary>
    public static class NameCorrector
    {
        /// <summary>
        /// 名前が不正だった場合は正しい名前へ修正
        /// </summary>
        public static string CorrectNameIfInvalid(string name)
        {
            name = RemoveInvalidChars(name);
            if (RESERVED_STRS.Contains(name) || Regex.Match(name, "^[0-9]").Success)
            {
                name = "_" + name;
            }
            return name;
        }

        /// <summary>
        /// 無効な文字を削除
        /// </summary>
        private static string RemoveInvalidChars(string str)
        {
            Array.ForEach(INVALID_CHARS, c => str = str.Replace(c, string.Empty));
            return str;
        }

        /// <summary>
        /// 無効な文字
        /// </summary>
        private static readonly string[] INVALID_CHARS =
        {
            " ", "!", "\"", "#", "$",
            "%", "&", "\'", "(", ")",
            "-", "=", "^",  "~", "\\",
            "|", "[", "{",  "@", "`",
            "]", "}", ":",  "*", ";",
            "+", "/", "?",  ".", ">",
            ",", "<",
        };

        /// <summary>
        /// C# 予約語
        /// </summary>
        private static readonly string[] RESERVED_STRS =
        {
            "abstract",
            "as",
            "async",
            "await",
            "base",
            "bool",
            "break",
            "byte",
            "case",
            "catch",
            "char",
            "checked",
            "class",
            "const",
            "continue",
            "decimal",
            "default",
            "delegate",
            "do",
            "double",
            "else",
            "enum",
            "event",
            "explicit",
            "extern",
            "FALSE",
            "finally",
            "fixed",
            "float",
            "for",
            "foreach",
            "goto",
            "if",
            "implicit",
            "in",
            "int",
            "interface",
            "internal",
            "is",
            "lock",
            "long",
            "namespace",
            "new",
            "null",
            "object",
            "operator",
            "out",
            "override",
            "params",
            "private",
            "protected",
            "public",
            "readonly",
            "ref",
            "return",
            "sbyte",
            "sealed",
            "short",
            "sizeof",
            "stackalloc",
            "static",
            "string",
            "struct",
            "switch",
            "this",
            "throw",
            "TRUE",
            "try",
            "typeof",
            "uint",
            "ulong",
            "unchecked",
            "unsafe",
            "ushort",
            "using",
            "virtual",
            "volatile",
            "void",
            "while",
        };
    }
}
