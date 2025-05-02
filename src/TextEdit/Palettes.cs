namespace ImGuiColorTextEditNet;

public static class Palettes
{
    public static readonly uint[] Dark = {
        0xff7f7f7f, // Default
        0xffd69c56, // Keyword
        0xff00ff00, // Number
        0xff7070e0, // String
        0xff70a0e0, // Char literal
        0xffffffff, // Punctuation
        0xff408080, // Preprocessor
        0xffaaaaaa, // Identifier
        0xff9bc64d, // Known identifier
        0xffc040a0, // Preproc identifier
        0xff50c050, // Comment (single line)
        0xff70c050, // Comment (multi line)
        0xff101010, // Background
        0xffe0e0e0, // Cursor
        0x80a06020, // Selection
        0x800020ff, // ErrorMarker
        0x40f08000, // Breakpoint
        0xff707000, // Line number
        0x40000000, // Current line fill
        0x40808080, // Current line fill (inactive)
        0x40a0a0a0, // Current line edge
        0xa0a0a0a0, // Executing Line
        0xffd69c56, // Function
    };

    public static readonly uint[] DarkMaterial = {
        0xffc5c8c6, // Default (soft grey)
        0xffc792ea, // Keyword (soft purple)
        0xfff78c6c, // Number (peachy orange)
        0xffc3e88d, // String (green)
        0xfffcbf7e, // Char literal (light orange)
        0xff89ddff, // Punctuation (cyan)
        0xff82aaff, // Preprocessor (blue)
        0xffd0d0d0, // Identifier (light gray)
        0xffaddb67, // Known identifier (lime green)
        0xffffcb6b, // Preproc identifier (yellow)
        0xff616161, // Comment (single line, dim gray)
        0xff616161, // Comment (multi line)
        0xff212121, // Background (dark gray)
        0xffffffff, // Cursor (white)
        0x8039adb5, // Selection (semi-transparent teal)
        0x80ff5370, // ErrorMarker (semi-transparent red)
        0x40ffcb6b, // Breakpoint (soft yellow)
        0xff4a4a4a, // Line number (dim gray)
        0x40282828, // Current line fill (dark, semi-transparent)
        0x403c3f41, // Current line fill (inactive)
        0x403c3f41, // Current line edge
        0xa03dd3b0, // Executing Line (teal)
        0xffc792ea, // Function (soft purple)
    };

    public static readonly uint[] Light = {
        0xff7f7f7f, // None
        0xffff0c06, // Keyword
        0xff008000, // Number
        0xff2020a0, // String
        0xff304070, // Char literal
        0xff000000, // Punctuation
        0xff406060, // Preprocessor
        0xff404040, // Identifier
        0xff606010, // Known identifier
        0xffc040a0, // Preproc identifier
        0xff205020, // Comment (single line)
        0xff405020, // Comment (multi line)
        0xffffffff, // Background
        0xff000000, // Cursor
        0x80600000, // Selection
        0xa00010ff, // ErrorMarker
        0x80f08000, // Breakpoint
        0xff505000, // Line number
        0x40000000, // Current line fill
        0x40808080, // Current line fill (inactive)
        0x40000000, // Current line edge
        0xa0a0a0a0, // Executing Line
        0xffff0c06, // Function
    };

    public static readonly uint[] RetroBlue = {
        0xff00ffff, // None
        0xffffff00, // Keyword
        0xff00ff00, // Number
        0xff808000, // String
        0xff808000, // Char literal
        0xffffffff, // Punctuation
        0xff008000, // Preprocessor
        0xff00ffff, // Identifier
        0xffffffff, // Known identifier
        0xffff00ff, // Preproc identifier
        0xff808080, // Comment (single line)
        0xff404040, // Comment (multi line)
        0xff800000, // Background
        0xff0080ff, // Cursor
        0x80ffff00, // Selection
        0xa00000ff, // ErrorMarker
        0x80ff8000, // Breakpoint
        0xff808000, // Line number
        0x40000000, // Current line fill
        0x40808080, // Current line fill (inactive)
        0x40000000, // Current line edge
        0xa0a0a0a0, // Executing Line
        0xffffff00, // Function
    };
}
