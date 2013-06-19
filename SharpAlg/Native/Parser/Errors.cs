using System;
using System.Text;

namespace SharpAlg.Native.Parser {
    public abstract class ErrorsBase {
        const string errMsgFormat = "Error at line {0} column {1}: {2}";
        internal static string GetErrorText(int line, int column, string errorText) {
            return string.Format(errMsgFormat, line, column, errorText);
        }

        StringBuilder errorsBuilder = new StringBuilder();
        public int Count { get; private set; }
        public string Errors { get { return errorsBuilder.ToString(); } }
        

        public virtual void SynErr(int line, int column, int n) {
            AppendLine(string.Format(errMsgFormat, line, column, GetErrorByCode(n)));
            Count++;
        }

        public virtual void SemErr(int line, int column, string errorText) {
            AppendLine(GetErrorText(line, column, errorText));
            Count++;
        }

        public virtual void SemErr(string s) {
            AppendLine(s);
            Count++;
        }

        public virtual void Warning(int line, int column, string errorText) {
            AppendLine(string.Format(errMsgFormat, line, column, errorText));
        }

        public virtual void Warning(string warningText) {
            AppendLine(warningText);
        }
        void AppendLine(string s) {
            errorsBuilder.AppendLine(s);
        }
        protected abstract string GetErrorByCode(int n);
    }
}
