using System;
using System.IO;
using System.Collections;

namespace SharpAlg.Native.Parser {
    public class Buffer {
        public const int EOF = char.MaxValue + 1;

        string source;

        public Buffer(string source) {
            this.source = source;
        }

        public virtual int Read() {
            if(Position < source.Length)
                return source[Position++];
            return EOF;
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
                    throw new FatalError("buffer out of bounds access, position: " + value);
                }
                position = value;
            }
        }
    }
}
