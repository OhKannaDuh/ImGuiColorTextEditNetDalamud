using System;

namespace ImGuiColorTextEditNet;

public class LuaHighlighter : ISyntaxHighlighter
{
    static readonly object DefaultState = new();
    static readonly object MultiLineCommentState = new();
    readonly SimpleTrie<Identifier> _identifiers;

    record Identifier(PaletteIndex Color)
    {
        public string Declaration = "";
    }

    public LuaHighlighter()
    {
        _identifiers = new SimpleTrie<Identifier>();

        foreach (var keyword in Lua().Keywords!)
            _identifiers.Add(keyword, new Identifier(PaletteIndex.Keyword));

        foreach (var builtin in Lua().Identifiers!)
        {
            var id = new Identifier(PaletteIndex.KnownIdentifier) { Declaration = "Built-in function" };
            _identifiers.Add(builtin, id);
        }
    }

    public bool AutoIndentation => true;
    public int MaxLinesPerFrame => 1000;

    public string? GetTooltip(string id) => _identifiers.Get(id)?.Declaration;

    public object Colorize(Span<Glyph> line, object? state)
    {
        for (int i = 0; i < line.Length;)
        {
            int result = Tokenize(line[i..], ref state);
            Util.Assert(result != 0);
            i += result > 0 ? result : 1;
        }

        return state ?? DefaultState;
    }

    int Tokenize(Span<Glyph> span, ref object? state)
    {
        int i = 0;

        while (i < span.Length && span[i].Char is ' ' or '\t')
            i++;

        if (i > 0)
            return i;

        int result;
        if ((result = TokenizeMultiLineComment(span, ref state)) != -1) return result;
        if ((result = TokenizeSingleLineComment(span)) != -1) return result;
        if ((result = TokenizeString(span)) != -1) return result;
        if ((result = TokenizeNumber(span)) != -1) return result;
        if ((result = TokenizeIdentifier(span)) != -1) return result;
        if ((result = TokenizePunctuation(span)) != -1) return result;

        return -1;
    }

    static int TokenizeMultiLineComment(Span<Glyph> span, ref object? state)
    {
        // if (state != MultiLineCommentState && !span.StartsWith("--[[".AsSpan()))
        if (state != MultiLineCommentState && !(span.Length >= 4 &&
                span[0].Char == '-' && span[1].Char == '-' &&
                span[2].Char == '[' && span[3].Char == '['))
            return -1;

        state = MultiLineCommentState;
        int i = 0;

        while (i < span.Length)
        {
            span[i] = new Glyph(span[i].Char, PaletteIndex.MultiLineComment);
            if (i + 1 < span.Length && span[i].Char == ']' && span[i + 1].Char == ']')
            {
                span[i + 1] = new Glyph(span[i + 1].Char, PaletteIndex.MultiLineComment);
                state = DefaultState;
                return i + 2;
            }
            i++;
        }

        return i;
    }

    static int TokenizeSingleLineComment(Span<Glyph> span)
    {
        // if (!span.StartsWith("--".AsSpan()))
        if (span.Length < 2 || span[0].Char != '-' || span[1].Char != '-')
            return -1;

        for (int i = 0; i < span.Length; i++)
            span[i] = new Glyph(span[i].Char, PaletteIndex.Comment);

        return span.Length;
    }

    static int TokenizeString(Span<Glyph> span)
    {
        if (span[0].Char != '"' && span[0].Char != '\'')
            return -1;

        char quote = span[0].Char;

        for (int i = 1; i < span.Length; i++)
        {
            if (span[i].Char == quote)
            {
                for (int j = 0; j <= i; j++)
                    span[j] = new Glyph(span[j].Char, PaletteIndex.String);
                return i + 1;
            }

            if (span[i].Char == '\\' && i + 1 < span.Length)
                i++;
        }

        return -1;
    }

    static int TokenizeNumber(Span<Glyph> span)
    {
        int i = 0;
        if (!char.IsDigit(span[0].Char))
            return -1;

        while (i < span.Length && (char.IsDigit(span[i].Char) || span[i].Char == '.'))
        {
            span[i] = new Glyph(span[i].Char, PaletteIndex.Number);
            i++;
        }

        return i;
    }

    int TokenizeIdentifier(Span<Glyph> span)
    {
        if (!char.IsLetter(span[0].Char) && span[0].Char != '_')
            return -1;

        int i = 1;
        while (i < span.Length && (char.IsLetterOrDigit(span[i].Char) || span[i].Char == '_'))
            i++;

        var info = _identifiers.Get<Glyph>(span[..i], x => x.Char);
        var color = info?.Color ?? PaletteIndex.Identifier;

        // Look ahead for : or . followed by another identifier and (
        if (i + 2 < span.Length && (span[i].Char == ':' || span[i].Char == '.'))
        {
            int sepIndex = i;
            int j = i + 1;

            if (char.IsLetter(span[j].Char) || span[j].Char == '_')
            {
                int idStart = j;
                j++;
                while (j < span.Length && (char.IsLetterOrDigit(span[j].Char) || span[j].Char == '_'))
                    j++;

                // Now check for '(' immediately after
                if (j < span.Length && span[j].Char == '(')
                {
                    // Color the first part normally
                    for (int k = 0; k < i; k++)
                        span[k] = new Glyph(span[k].Char, color);

                    // Color the . or :
                    span[sepIndex] = new Glyph(span[sepIndex].Char, PaletteIndex.Punctuation);

                    // Color the function call name
                    for (int k = idStart; k < j; k++)
                        span[k] = new Glyph(span[k].Char, PaletteIndex.Function);

                    return j; // Return up to '(' (let punctuation handle it)
                }
            }
        }

        // Fallback: normal identifier
        for (int j = 0; j < i; j++)
            span[j] = new Glyph(span[j].Char, color);

        return i;
    }

    static int TokenizePunctuation(Span<Glyph> span)
    {
        char c = span[0].Char;
        if ("[]{}()+-*/%^#=~<>.,:;".Contains(c))
        {
            span[0] = new Glyph(c, PaletteIndex.Punctuation);
            return 1;
        }

        return -1;
    }

    static LanguageDefinition Lua() => new("Lua")
    {
        Keywords = new[]
        {
            "and", "break", "do", "else", "elseif", "end", "false", "for",
            "function", "goto", "if", "in", "local", "nil", "not", "or",
            "repeat", "return", "then", "true", "until", "while"
        },
        Identifiers = new[]
        {
            "assert", "collectgarbage", "dofile", "error", "getmetatable",
            "ipairs", "load", "loadfile", "next", "pairs", "pcall",
            "print", "rawequal", "rawget", "rawlen", "rawset",
            "require", "select", "setmetatable", "tonumber", "tostring",
            "type", "xpcall", "coroutine", "math", "string", "table", "os", "io", "debug"
        }
    };
}
