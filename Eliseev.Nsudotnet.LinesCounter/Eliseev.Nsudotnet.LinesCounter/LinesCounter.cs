using System;
using System.Collections.Generic;
using System.Text;

namespace Eliseev.Nsudotnet.LinesCounter
{
    public static class LinesCounter
    {
        private const int CRCode = 10;
        private const int LFCode = 13;
        private const char SingleLineCommentFirstSymbol = '/';
        private const char SingleLineCommentSecondSymbol = '/';
        private const char MultyLineCommentStartFirstSymbol = '/';
        private const char MultyLineCommentStartSecondSymbol = '*';
        private const char MultyLineCommentEndFirstSymbol = '*';
        private const char MultyLineCommentEndSecondSymbol = '/';

        public enum States
        {
            WaitingSymbols,
            SkipingSymbols,
            SkipingSingleComment,
            SkipingMultyComment
        };

        public static int CountLinesInDirectoryAndSubDirectoryes(string directory, string fileFormat)
        {
            if (!Directory.Exists(directory))
            {
                throw new ArgumentException($"Directory {directory} doesn't exist");
            }
            if (fileFormat == string.Empty)
            {
                throw new ArgumentException($"Directory {directory} doesn't exist");
            }

            int lines = CountLinesInDirectory(directory, fileFormat);

            string[] allSubDirectories = Directory.GetDirectories(directory, "*.*", SearchOption.AllDirectories);
            foreach (string dir in allSubDirectories)
            {
                lines += CountLinesInDirectory(dir, fileFormat);
            }

            return lines;
        }

        public static int CountLinesInDirectory(string dir, string fileFormat)
        {
            string[] files = Directory.GetFiles(dir, fileFormat);

            int lines = 0;
            foreach (var file in files)
            {
                lines += CountLinesInFile(file);
            }

            return lines;
        }

        public static int CountLinesInFile(string file)
        {
            int lines = 0;

            Console.WriteLine("Opening " + file);

            try
            {
                using (StreamReader streamReader = new StreamReader(file))
                {
                    States state = States.WaitingSymbols;

                    bool wasInSkippingMultyLCommentState = false;
                    bool wasNewLineInSkippingMultyLCommentState = false;

                    while (!streamReader.EndOfStream)
                    {
                        var c = streamReader.Read();
                        switch (state)
                        {
                            case States.WaitingSymbols:
                                if (CharStartsSingleLineComment(c, streamReader))
                                {
                                    state = States.SkipingSingleComment;
                                }
                                else if (CharStartsMultylineComment(c, streamReader))
                                {
                                    state = States.SkipingMultyComment;
                                }
                                else if (CharStartsNewLine(c, streamReader))
                                {
                                }
                                else if (CharIsWhitespace(c))
                                {
                                    SkipAllWhiteSpaces(streamReader);
                                }
                                else
                                {
                                    state = States.SkipingSymbols;
                                    if (!wasInSkippingMultyLCommentState ||
                                        (wasInSkippingMultyLCommentState && wasNewLineInSkippingMultyLCommentState))
                                    {
                                        lines++;
                                    }
                                    wasInSkippingMultyLCommentState = false;
                                    wasNewLineInSkippingMultyLCommentState = false;
                                }
                                break;
                            case States.SkipingSymbols:
                                if (CharStartsMultylineComment(c, streamReader))
                                {
                                    state = States.SkipingMultyComment;
                                }
                                else if (CharStartsNewLine(c, streamReader))
                                {
                                    state = States.WaitingSymbols;
                                }
                                // else: no op, same state
                                break;
                            case States.SkipingMultyComment:
                                if (CharStartsNewLine(c, streamReader))
                                {
                                    wasNewLineInSkippingMultyLCommentState = true;
                                }
                                else if (CharStartsMultyLineCommentEnd(c, streamReader))
                                {
                                    state = States.WaitingSymbols;
                                    wasInSkippingMultyLCommentState = true;
                                }
                                break;
                            case States.SkipingSingleComment:
                                if (CharStartsNewLine(c, streamReader))
                                {
                                    state = States.WaitingSymbols;
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            return lines;
        }

        private static void SkipAllWhiteSpaces(StreamReader streamReader)
        {
            while (char.IsWhiteSpace((char)streamReader.Peek()))
            {
                streamReader.Read();
            }
        }

        private static bool CharIsWhitespace(int c)
        {
            return char.IsWhiteSpace((char)c);
        }

        private static bool CharStartsMultyLineCommentEnd(int c, StreamReader streamReader)
        {
            if (c == MultyLineCommentEndFirstSymbol && streamReader.Peek() == MultyLineCommentEndSecondSymbol)
            {
                streamReader.Read();
                return true;
            }
            return false;
        }

        private static bool CharStartsNewLine(int c, StreamReader streamReader)
        {
            if (c == CRCode)
            {
                return true;
            }
            if (c == LFCode && streamReader.Peek() == CRCode)
            {
                streamReader.Read();
                return true;
            }
            return false;
        }

        private static bool CharStartsMultylineComment(int c, StreamReader streamReader)
        {
            if (c == MultyLineCommentStartFirstSymbol && streamReader.Peek() == MultyLineCommentStartSecondSymbol)
            {
                streamReader.Read();
                return true;
            }
            return false;
        }

        private static bool CharStartsSingleLineComment(int c, StreamReader streamReader)
        {
            if (c == SingleLineCommentFirstSymbol && streamReader.Peek() == SingleLineCommentSecondSymbol)
            {
                streamReader.Read();
                return true;
            }
            return false;
        }
    }
}
