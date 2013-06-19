using System;

namespace SharpAlg.Native.Parser {
    public abstract class ErrorsBase {
        public int count = 0;                                    // number of errors detected
        public System.IO.TextWriter errorStream = Console.Out;   // error messages go to this stream
        public string errMsgFormat = "-- line {0} col {1}: {2}"; // 0=line, 1=column, 2=text

        protected abstract string GetErrorByCode(int n);
        public virtual void SynErr(int line, int col, int n) {
            string s;
            s = GetErrorByCode(n);
            errorStream.WriteLine(errMsgFormat, line, col, s);
            count++;
        }

        public virtual void SemErr(int line, int col, string s) {
            errorStream.WriteLine(errMsgFormat, line, col, s);
            count++;
        }

        public virtual void SemErr(string s) {
            errorStream.WriteLine(s);
            count++;
        }

        public virtual void Warning(int line, int col, string s) {
            errorStream.WriteLine(errMsgFormat, line, col, s);
        }

        public virtual void Warning(string s) {
            errorStream.WriteLine(s);
        }
    } // Errors
}
