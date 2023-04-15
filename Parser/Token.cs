namespace PSI;
using static Token.E;

// Represents a PSI language Token
public class Token {
   public Token (Tokenizer source, E kind, string text, int line, int column) 
      => (Source, Kind, Text, Line, Column) = (source, kind, text, line, column);
   public Tokenizer Source { get; }
   public E Kind { get; }
   public string Text { get; }
   public int Line { get; }
   public int Column { get; }

   // The various types of token
   public enum E {
      // Keywords
      PROGRAM, VAR, IF, THEN, WHILE, ELSE, FOR, TO, DOWNTO,
      DO, BEGIN, END, PRINT, TYPE, NOT, OR, AND, MOD, _ENDKEYWORDS,
      // Operators
      ADD, SUB, MUL, DIV, NEQ, LEQ, GEQ, EQ, LT, GT, ASSIGN, 
      _ENDOPERATORS,
      // Punctuation
      SEMI, PERIOD, COMMA, OPEN, CLOSE, COLON, 
      _ENDPUNCTUATION,
      // Others
      IDENT, INTEGER, REAL, BOOLEAN, STRING, CHAR, EOF, ERROR
   }

   // Print a Token
   public override string ToString () => Kind switch {
      EOF or ERROR => Kind.ToString (),
      < _ENDKEYWORDS => $"\u00ab{Kind.ToString ().ToLower ()}\u00bb",
      STRING => $"\"{Text}\"",
      CHAR => $"'{Text}'",
      _ => Text,
   };

   // Utility function used to echo an error to the console
   public void PrintError () {
      if (Kind != ERROR) throw new Exception ("PrintError called on a non-error token");
      string title = $"File: {Source.FileName}";
      var headLine = string.Concat (Enumerable.Repeat ("_", 18));
      const int idxLength = 3;
      Console.WriteLine (title);
      Console.WriteLine (headLine);
      var start = Line > 2 ? Line - 3: Line - 1;
      for (int i = start; i < Line; i++)
            Console.WriteLine ($" {i,3}│{Source.Lines[i]}");
      Console.ForegroundColor = ConsoleColor.Yellow;
      var pos = Console.GetCursorPosition ();
      Console.SetCursorPosition (Column + 4, pos.Top);
      Console.WriteLine ("^");
      Console.SetCursorPosition (Column - 2, pos.Top);
      Console.WriteLine ($"{Text}");
      Console.ResetColor ();
      var end = Line > Source.Lines.Length - 2 ? Source.Lines.Length - 1 : Line + 2;
      var wid = Source.Lines[start..end].Max (a => a.Length);
      wid = (wid - Column) < 10 ? wid + 11 : wid + 4;
      Console.WindowWidth = wid;
      for (int i = Line; i < end; i++)
         Console.WriteLine ($" {i,idxLength}│{Source.Lines[i]}");

   }

   // Helper used by the parser (maps operator sequences to E values)
   public static List<(E Kind, string Text)> Match = new () {
      (NEQ, "<>"), (LEQ, "<="), (GEQ, ">="), (ASSIGN, ":="), (ADD, "+"),
      (SUB, "-"), (MUL, "*"), (DIV, "/"), (EQ, "="), (LT, "<"),
      (LEQ, "<="), (GT, ">"), (SEMI, ";"), (PERIOD, "."), (COMMA, ","),
      (OPEN, "("), (CLOSE, ")"), (COLON, ":")
   };
}
