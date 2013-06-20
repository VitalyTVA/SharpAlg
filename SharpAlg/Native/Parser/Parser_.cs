/*----------------------------------------------------------------------
SharpAlg Parser
-----------------------------------------------------------------------*/

using System;
using SharpKit.JavaScript;

namespace SharpAlg.Native.Parser {


[JsType(JsMode.Prototype, Filename = SR.JSParserName)]
public class Parser {
	public const int _EOF = 0;
	public const int _ident = 1;
	public const int _number = 2;
	public const int maxT = 4;

	const bool T = true;
	const bool x = false;
	const int minErrDist = 2;
	
	public Scanner scanner;
	public Errors  errors;

	public Token t;    // last recognized token
	public Token la;   // lookahead token
	int errDist = minErrDist;


	public int result;
	public Parser(Scanner scanner) {
		this.scanner = scanner;
		errors = new Errors();
	}

	void SynErr (int n) {
		if (errDist >= minErrDist) errors.SynErr(la.line, la.col, n);
		errDist = 0;
	}

	public void SemErr (string msg) {
		if (errDist >= minErrDist) errors.SemErr(t.line, t.col, msg);
		errDist = 0;
	}
	
	void Get () {
		for (;;) {
			t = la;
			la = scanner.Scan();
			if (la.kind <= maxT) { ++errDist; break; }

			la = t;
		}
	}
	
	void Expect (int n) {
		if (la.kind==n) Get(); else { SynErr(n); }
	}
	
	bool StartOf (int s) {
		return set[s, la.kind];
	}
	
	void ExpectWeak (int n, int follow) {
		if (la.kind == n) Get();
		else {
			SynErr(n);
			while (!StartOf(follow)) Get();
		}
	}


	bool WeakSeparator(int n, int syFol, int repFol) {
		int kind = la.kind;
		if (kind == n) {Get(); return true;}
		else if (StartOf(repFol)) {return false;}
		else {
			SynErr(n);
			while (!(set[syFol, kind] || set[repFol, kind] || set[0, kind])) {
				Get();
				kind = la.kind;
			}
			return StartOf(syFol);
		}
	}

	
	void SharpAlg() {
		int result; 
		Expression(out result);
		this.result = result; 
	}

	void Expression(out int result) {
		int right; 
		Term(out result);
		while (la.kind == 3) {
			Get();
			Term(out right);
			result += right; 
		}
	}

	void Term(out int number) {
		Expect(2);
		number = Int32.Parse(t.val); 
	}



	public void Parse() {
		la = new Token();
		la.val = "";		
		Get();
		try {
		SharpAlg();
		Expect(0);

		} catch(Exception e) {
			SemErr(e.ToString());
		}
	}
	
	static readonly bool[,] set = {
		{T,x,x,x, x,x}

	};
} // end Parser

[JsType(JsMode.Prototype, Filename = SR.JSParserName)]
public class Errors : ErrorsBase {
    protected override string GetErrorByCode(int n) {
        string s;
        switch(n) {
			case 0: s = "EOF expected"; break;
			case 1: s = "ident expected"; break;
			case 2: s = "number expected"; break;
			case 3: s = "\"+\" expected"; break;
			case 4: s = "??? expected"; break;

            default: s = "error " + n; break;
        }
        return s;
    }
}}