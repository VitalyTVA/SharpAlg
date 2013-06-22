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
	public const int maxT = 7;

	const bool T = true;
	const bool x = false;
	const int minErrDist = 2;
	
	public Scanner scanner;
	public Errors  errors;

	public Token t;    // last recognized token
	public Token la;   // lookahead token
	int errDist = minErrDist;


	public Expr Expr { get; private set; }
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
		return set[s][la.kind];
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
			while (!(set[syFol][kind] || set[repFol][kind] || set[0][kind])) {
				Get();
				kind = la.kind;
			}
			return StartOf(syFol);
		}
	}

	
	void SharpAlg() {
		Expr expr; 
		AdditiveExpression(out expr);
		this.Expr = expr; 
	}

	void AdditiveExpression(out Expr expr) {
		BinaryOperation operation; Expr rightExpr; 
		Terminal(out expr);
		while (StartOf(1)) {
			AdditiveOperation(out operation);
			Terminal(out rightExpr);
			expr = Expr.Binary(expr, rightExpr, operation); 
		}
	}

	void Terminal(out Expr expr) {
		Expect(2);
		expr = Expr.Constant(Int32.Parse(t.val)); 
	}

	void AdditiveOperation(out BinaryOperation operation) {
		operation = BinaryOperation.Add; 
		if (la.kind == 3) {
			Get();
		} else if (la.kind == 4) {
			Get();
			operation = BinaryOperation.Subtract; 
		} else if (la.kind == 5) {
			Get();
			operation = BinaryOperation.Multiply; 
		} else if (la.kind == 6) {
			Get();
			operation = BinaryOperation.Divide; 
		} else SynErr(8);
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
/*original set	
	static readonly bool[,] set = {
<--initialization
	};
*/
//parser set patch begin
	static readonly bool[][] set = {
		new bool[] {T,x,x,x, x,x,x,x, x},
		new bool[] {x,x,x,T, T,T,T,x, x}

	};
//parser set patch end
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
			case 4: s = "\"-\" expected"; break;
			case 5: s = "\"*\" expected"; break;
			case 6: s = "\"/\" expected"; break;
			case 7: s = "??? expected"; break;
			case 8: s = "invalid AdditiveOperation"; break;

            default: s = "error " + n; break;
        }
        return s;
    }
}}