using System;
using System.IO;
using System.Collections;
using SharpKit.JavaScript;

namespace SharpAlg.Native.Parser {
    [JsType(JsMode.Prototype, Filename = SR.JSParserName)]
    public class Buffer {
        public const int EOF = char.MaxValue + 1;

        string source;

        public Buffer(string source) {
            this.source = source;
        }

        public virtual int Read() {
            if(Position < source.Length) {
                Position++;
                return GetIntFromChar(source[Position - 1]);
            }
            return EOF;
        }
        //TODO move to compatibility layer
        [JsMethod(Code = "return c.charCodeAt();")]
        static int GetIntFromChar(char c) {
            return c;
        }
        public int Peek() {
            int curPos = Position;
            int ch = Read();
            Position = curPos;
            return ch;
        }

        //// beg .. begin, zero-based, inclusive, in byte
        //// end .. end, zero-based, exclusive, in byte
        //public string GetString(int beg, int end) {
        //}

        int position;
        public int Position {
            get { return position; }
            set {
                if(value < 0 || value > source.Length) {
                    throw new FatalError(SR.STR_Parser_BufferOutOfBoundsAccessPosition + value);
                }
                position = value;
            }
        }
    }
}
