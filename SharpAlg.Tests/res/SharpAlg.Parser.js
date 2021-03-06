/*Generated by SharpKit 5 v5.01.1000*/
if (typeof ($Inherit) == 'undefined') {
	var $Inherit = function (ce, ce2) {

		if (typeof (Object.getOwnPropertyNames) == 'undefined') {

			for (var p in ce2.prototype)
				if (typeof (ce.prototype[p]) == 'undefined' || ce.prototype[p] == Object.prototype[p])
					ce.prototype[p] = ce2.prototype[p];
			for (var p in ce2)
				if (typeof (ce[p]) == 'undefined')
					ce[p] = ce2[p];
			ce.$baseCtor = ce2;

		} else {

			var props = Object.getOwnPropertyNames(ce2.prototype);
			for (var i = 0; i < props.length; i++)
				if (typeof (Object.getOwnPropertyDescriptor(ce.prototype, props[i])) == 'undefined')
					Object.defineProperty(ce.prototype, props[i], Object.getOwnPropertyDescriptor(ce2.prototype, props[i]));

			for (var p in ce2)
				if (typeof (ce[p]) == 'undefined')
					ce[p] = ce2[p];
			ce.$baseCtor = ce2;

		}

	}
};
if (typeof($CreateException)=='undefined') 
{
    var $CreateException = function(ex, error) 
    {
        if(error==null)
            error = new Error();
        if(ex==null)
            ex = new System.Exception.ctor();       
        error.message = ex.message;
        for (var p in ex)
           error[p] = ex[p];
        return error;
    }
}
if (typeof(SharpAlg) == "undefined")
    var SharpAlg = {};
if (typeof(SharpAlg.Native) == "undefined")
    SharpAlg.Native = {};
if (typeof(SharpAlg.Native.Parser) == "undefined")
    SharpAlg.Native.Parser = {};
SharpAlg.Native.Parser.Buffer = function (source)
{
    this.source = null;
    this.pos = 0;
    this.source = source;
};
SharpAlg.Native.Parser.Buffer.EOF = 65536;
SharpAlg.Native.Parser.Buffer.prototype.Read = function ()
{
    if (this.get_Pos() < this.source.length)
    {
        this.set_Pos(this.get_Pos() + 1);
        return SharpAlg.Native.PlatformHelper.CharToInt(this.source.charAt(this.get_Pos() - 1));
    }
    return 65536;
};
SharpAlg.Native.Parser.Buffer.prototype.Peek = function ()
{
    var curPos = this.get_Pos();
    var ch = this.Read();
    this.set_Pos(curPos);
    return ch;
};
SharpAlg.Native.Parser.Buffer.prototype.get_Pos = function ()
{
    return this.pos;
};
SharpAlg.Native.Parser.Buffer.prototype.set_Pos = function (value)
{
    if (value < 0 || value > this.source.length)
    {
        throw $CreateException(new SharpAlg.Native.Parser.FatalError.ctor("Buffer out of bounds access, position: " + value), new Error());
    }
    this.pos = value;
};
SharpAlg.Native.Parser.ErrorsBase = function ()
{
    this.errorsBuilder = new System.Text.StringBuilder.ctor();
    this.Count = 0;
};
SharpAlg.Native.Parser.ErrorsBase.errMsgFormat = "Error at line {0} column {1}: {2}";
SharpAlg.Native.Parser.ErrorsBase.GetErrorText = function (line, column, errorText)
{
    return System.String.Format$$String$$Object$$Object$$Object("Error at line {0} column {1}: {2}", line, column, errorText);
};
SharpAlg.Native.Parser.ErrorsBase.prototype.get_Errors = function ()
{
    return this.errorsBuilder.toString();
};
SharpAlg.Native.Parser.ErrorsBase.prototype.SynErr = function (line, column, parserErrorCode)
{
    this.AppendLine(System.String.Format$$String$$Object$$Object$$Object("Error at line {0} column {1}: {2}", line, column, this.GetErrorByCode(parserErrorCode)));
    this.Count++;
};
SharpAlg.Native.Parser.ErrorsBase.prototype.SemErr = function (line, column, errorText)
{
    this.AppendLine(SharpAlg.Native.Parser.ErrorsBase.GetErrorText(line, column, errorText));
    this.Count++;
};
SharpAlg.Native.Parser.ErrorsBase.prototype.SemErr = function (s)
{
    this.AppendLine(s);
    this.Count++;
};
SharpAlg.Native.Parser.ErrorsBase.prototype.Warning = function (line, column, errorText)
{
    this.AppendLine(System.String.Format$$String$$Object$$Object$$Object("Error at line {0} column {1}: {2}", line, column, errorText));
};
SharpAlg.Native.Parser.ErrorsBase.prototype.Warning = function (warningText)
{
    this.AppendLine(warningText);
};
SharpAlg.Native.Parser.ErrorsBase.prototype.AppendLine = function (s)
{
    this.errorsBuilder.Append$$String(s);
    this.errorsBuilder.Append$$String("\r\n");
};
if (typeof(JsTypes) == "undefined")
    var JsTypes = [];
var SharpAlg$Native$Parser$ArgsList =
{
    fullname: "SharpAlg.Native.Parser.ArgsList",
    baseTypeName: "System.Collections.Generic.List$1",
    assemblyName: "SharpAlg.Parser",
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            System.Collections.Generic.List$1.ctor.call(this, SharpAlg.Native.Expr.ctor);
        }
    }
};
JsTypes.push(SharpAlg$Native$Parser$ArgsList);
SharpAlg.Native.Parser.Parser = function (scanner, builder)
{
    this.scanner = null;
    this.errors = null;
    this.t = null;
    this.la = null;
    this.errDist = 2;
    this.builder = null;
    this.Expr = null;
    this.scanner = scanner;
    this.errors = new SharpAlg.Native.Parser.Errors();
    this.builder = builder;
};
SharpAlg.Native.Parser.Parser._EOF = 0;
SharpAlg.Native.Parser.Parser._identifier = 1;
SharpAlg.Native.Parser.Parser._number = 2;
SharpAlg.Native.Parser.Parser._floatNumber = 3;
SharpAlg.Native.Parser.Parser.maxT = 13;
SharpAlg.Native.Parser.Parser.T = true;
SharpAlg.Native.Parser.Parser.x = false;
SharpAlg.Native.Parser.Parser.minErrDist = 2;
SharpAlg.Native.Parser.Parser.set = [[true, false, false, false, false, false, false, false, false, false, false, false, false, false, false], [false, true, true, true, true, false, false, false, false, false, true, false, false, false, false]];
SharpAlg.Native.Parser.Parser.prototype.SynErr = function (n)
{
    if (this.errDist >= 2)
        this.errors.SynErr(this.la.line, this.la.col, n);
    this.errDist = 0;
};
SharpAlg.Native.Parser.Parser.prototype.SemErr = function (msg)
{
    if (this.errDist >= 2)
        this.errors.SemErr(this.t.line, this.t.col, msg);
    this.errDist = 0;
};
SharpAlg.Native.Parser.Parser.prototype.Get = function ()
{
    for (;;)
    {
        this.t = this.la;
        this.la = this.scanner.Scan();
        if (this.la.kind <= 13)
        {
            ++this.errDist;
            break;
        }
        this.la = this.t;
    }
};
SharpAlg.Native.Parser.Parser.prototype.Expect = function (n)
{
    if (this.la.kind == n)
        this.Get();
    else
    {
        this.SynErr(n);
    }
};
SharpAlg.Native.Parser.Parser.prototype.StartOf = function (s)
{
    return SharpAlg.Native.Parser.Parser.set[s][this.la.kind];
};
SharpAlg.Native.Parser.Parser.prototype.ExpectWeak = function (n, follow)
{
    if (this.la.kind == n)
        this.Get();
    else
    {
        this.SynErr(n);
        while (!this.StartOf(follow))
        this.Get();
    }
};
SharpAlg.Native.Parser.Parser.prototype.WeakSeparator = function (n, syFol, repFol)
{
    var kind = this.la.kind;
    if (kind == n)
    {
        this.Get();
        return true;
    }
    else if (this.StartOf(repFol))
    {
        return false;
    }
    else
    {
        this.SynErr(n);
        while (!(SharpAlg.Native.Parser.Parser.set[syFol][kind] || SharpAlg.Native.Parser.Parser.set[repFol][kind] || SharpAlg.Native.Parser.Parser.set[0][kind]))
        {
            this.Get();
            kind = this.la.kind;
        }
        return this.StartOf(syFol);
    }
};
SharpAlg.Native.Parser.Parser.prototype.SharpAlg = function ()
{
    var expr;
    (function ()
    {
        expr = {Value: expr};
        var $res = this.AdditiveExpression(expr);
        expr = expr.Value;
        return $res;
    }).call(this);
    this.Expr = expr;
};
SharpAlg.Native.Parser.Parser.prototype.AdditiveExpression = function (expr)
{
    var leftMinus = false, rightMinus;
    var rightExpr = null;
    if (this.la.kind == 4)
    {
        this.Get();
        leftMinus = true;
    }
    this.MultiplicativeExpression(expr);
    expr.Value = leftMinus ? this.builder.Minus(expr.Value) : expr.Value;
    while (this.la.kind == 4 || this.la.kind == 5)
    {
        (function ()
        {
            rightMinus = {Value: rightMinus};
            var $res = this.AdditiveOperation(rightMinus);
            rightMinus = rightMinus.Value;
            return $res;
        }).call(this);
        (function ()
        {
            rightExpr = {Value: rightExpr};
            var $res = this.MultiplicativeExpression(rightExpr);
            rightExpr = rightExpr.Value;
            return $res;
        }).call(this);
        expr.Value = this.builder.Add(expr.Value, (rightMinus ? this.builder.Minus(rightExpr) : rightExpr));
    }
};
SharpAlg.Native.Parser.Parser.prototype.MultiplicativeExpression = function (expr)
{
    var divide;
    var rightExpr;
    this.PowerExpression(expr);
    while (this.la.kind == 6 || this.la.kind == 7)
    {
        (function ()
        {
            divide = {Value: divide};
            var $res = this.MultiplicativeOperation(divide);
            divide = divide.Value;
            return $res;
        }).call(this);
        (function ()
        {
            rightExpr = {Value: rightExpr};
            var $res = this.PowerExpression(rightExpr);
            rightExpr = rightExpr.Value;
            return $res;
        }).call(this);
        expr.Value = this.builder.Multiply(expr.Value, (divide ? this.builder.Inverse(rightExpr) : rightExpr));
    }
};
SharpAlg.Native.Parser.Parser.prototype.AdditiveOperation = function (minus)
{
    minus.Value = false;
    if (this.la.kind == 5)
    {
        this.Get();
    }
    else if (this.la.kind == 4)
    {
        this.Get();
        minus.Value = true;
    }
    else
        this.SynErr(14);
};
SharpAlg.Native.Parser.Parser.prototype.PowerExpression = function (expr)
{
    var rightExpr;
    this.FactorialExpression(expr);
    while (this.la.kind == 8)
    {
        this.Get();
        (function ()
        {
            rightExpr = {Value: rightExpr};
            var $res = this.FactorialExpression(rightExpr);
            rightExpr = rightExpr.Value;
            return $res;
        }).call(this);
        expr.Value = this.builder.Power(expr.Value, rightExpr);
    }
};
SharpAlg.Native.Parser.Parser.prototype.MultiplicativeOperation = function (divide)
{
    divide.Value = false;
    if (this.la.kind == 6)
    {
        this.Get();
    }
    else if (this.la.kind == 7)
    {
        this.Get();
        divide.Value = true;
    }
    else
        this.SynErr(15);
};
SharpAlg.Native.Parser.Parser.prototype.FactorialExpression = function (expr)
{
    this.Terminal(expr);
    while (this.la.kind == 9)
    {
        this.Get();
        expr.Value = this.builder.Function("factorial", SharpAlg.Native.FunctionalExtensions.AsEnumerable$1(SharpAlg.Native.Expr.ctor, expr.Value));
    }
};
SharpAlg.Native.Parser.Parser.prototype.Terminal = function (expr)
{
    expr.Value = null;
    if (this.la.kind == 2)
    {
        this.Get();
        expr.Value = SharpAlg.Native.Expr.Constant(SharpAlg.Native.NumberFactory.FromIntString(this.t.val));
    }
    else if (this.la.kind == 3)
    {
        this.Get();
        expr.Value = SharpAlg.Native.Expr.Constant(SharpAlg.Native.NumberFactory.FromString(this.t.val));
    }
    else if (this.la.kind == 10)
    {
        this.Get();
        this.AdditiveExpression(expr);
        this.Expect(11);
    }
    else if (this.la.kind == 1)
    {
        this.FunctionCall(expr);
    }
    else
        this.SynErr(16);
};
SharpAlg.Native.Parser.Parser.prototype.FunctionCall = function (expr)
{
    var name;
    var args = null;
    this.Expect(1);
    name = this.t.val;
    while (this.la.kind == 10)
    {
        (function ()
        {
            args = {Value: args};
            var $res = this.ArgumentList(args);
            args = args.Value;
            return $res;
        }).call(this);
    }
    expr.Value = args != null ? this.builder.Function(name, args) : this.builder.Parameter(name);
};
SharpAlg.Native.Parser.Parser.prototype.ArgumentList = function (args)
{
    args.Value = new SharpAlg.Native.Parser.ArgsList.ctor();
    this.Expect(10);
    while (this.StartOf(1))
    {
        this.List(args.Value);
    }
    this.Expect(11);
};
SharpAlg.Native.Parser.Parser.prototype.List = function (args)
{
    var first;
    (function ()
    {
        first = {Value: first};
        var $res = this.AdditiveExpression(first);
        first = first.Value;
        return $res;
    }).call(this);
    args.Add(first);
    while (this.la.kind == 12)
    {
        this.Get();
        this.List(args);
    }
};
SharpAlg.Native.Parser.Parser.prototype.Parse = function ()
{
    this.la = {};
    this.la.val = "";
    this.Get();
    try
    {
        this.SharpAlg();
        this.Expect(0);
    }
    catch (e)
    {
        this.errors.SemErr(SharpAlg.Native.PlatformHelper.GetMessage(e));
    }
};
SharpAlg.Native.Parser.Errors = function ()
{
    SharpAlg.Native.Parser.ErrorsBase.call(this);
};
SharpAlg.Native.Parser.Errors.prototype.GetErrorByCode = function (n)
{
    var s;
    switch (n)
    {
        case 0:
            s = "EOF expected";
            break;
        case 1:
            s = "identifier expected";
            break;
        case 2:
            s = "number expected";
            break;
        case 3:
            s = "floatNumber expected";
            break;
        case 4:
            s = "\"-\" expected";
            break;
        case 5:
            s = "\"+\" expected";
            break;
        case 6:
            s = "\"*\" expected";
            break;
        case 7:
            s = "\"/\" expected";
            break;
        case 8:
            s = "\"^\" expected";
            break;
        case 9:
            s = "\"!\" expected";
            break;
        case 10:
            s = "\"(\" expected";
            break;
        case 11:
            s = "\")\" expected";
            break;
        case 12:
            s = "\",\" expected";
            break;
        case 13:
            s = "??? expected";
            break;
        case 14:
            s = "invalid AdditiveOperation";
            break;
        case 15:
            s = "invalid MultiplicativeOperation";
            break;
        case 16:
            s = "invalid Terminal";
            break;
        default :
            s = "error " + n;
            break;
    }
    return s;
};
$Inherit(SharpAlg.Native.Parser.Errors, SharpAlg.Native.Parser.ErrorsBase);
var SharpAlg$Native$Parser$Scanner =
{
    fullname: "SharpAlg.Native.Parser.Scanner",
    baseTypeName: "System.Object",
    staticDefinition:
    {
        cctor: function ()
        {
            SharpAlg.Native.Parser.Scanner.EOL = "\n";
            SharpAlg.Native.Parser.Scanner.eofSym = 0;
            SharpAlg.Native.Parser.Scanner.maxT = 13;
            SharpAlg.Native.Parser.Scanner.noSym = 13;
            SharpAlg.Native.Parser.Scanner.start = null;
            SharpAlg.Native.Parser.Scanner.start = new System.Collections.Generic.Dictionary$2.ctor(System.Int32.ctor, System.Int32.ctor);
            for (var i = 65; i <= 90; ++i)
                SharpAlg.Native.Parser.Scanner.start.set_Item$$TKey(i, 1);
            for (var i = 97; i <= 122; ++i)
                SharpAlg.Native.Parser.Scanner.start.set_Item$$TKey(i, 1);
            for (var i = 48; i <= 57; ++i)
                SharpAlg.Native.Parser.Scanner.start.set_Item$$TKey(i, 4);
            SharpAlg.Native.Parser.Scanner.start.set_Item$$TKey(46, 2);
            SharpAlg.Native.Parser.Scanner.start.set_Item$$TKey(45, 5);
            SharpAlg.Native.Parser.Scanner.start.set_Item$$TKey(43, 6);
            SharpAlg.Native.Parser.Scanner.start.set_Item$$TKey(42, 7);
            SharpAlg.Native.Parser.Scanner.start.set_Item$$TKey(47, 8);
            SharpAlg.Native.Parser.Scanner.start.set_Item$$TKey(94, 9);
            SharpAlg.Native.Parser.Scanner.start.set_Item$$TKey(33, 10);
            SharpAlg.Native.Parser.Scanner.start.set_Item$$TKey(40, 11);
            SharpAlg.Native.Parser.Scanner.start.set_Item$$TKey(41, 12);
            SharpAlg.Native.Parser.Scanner.start.set_Item$$TKey(44, 13);
            SharpAlg.Native.Parser.Scanner.start.set_Item$$TKey(65536, -1);
        }
    },
    assemblyName: "SharpAlg.Parser",
    Kind: "Class",
    definition:
    {
        ctor: function (source)
        {
            this.buffer = null;
            this.t = null;
            this.ch = 0;
            this.pos = 0;
            this.charPos = 0;
            this.col = 0;
            this.line = 0;
            this.oldEols = 0;
            this.tokens = null;
            this.pt = null;
            this.tval = [];
            this.tlen = 0;
            System.Object.ctor.call(this);
            this.buffer = new SharpAlg.Native.Parser.Buffer(source);
            this.Init();
        },
        Init: function ()
        {
            this.pos = -1;
            this.line = 1;
            this.col = 0;
            this.charPos = -1;
            this.oldEols = 0;
            this.NextCh();
            this.pt = this.tokens = {};
        },
        NextCh: function ()
        {
            if (this.oldEols > 0)
            {
                this.ch = 10;
                this.oldEols--;
            }
            else
            {
                this.pos = this.buffer.get_Pos();
                this.ch = this.buffer.Read();
                this.col++;
                this.charPos++;
                if (this.ch == 13 && this.buffer.Peek() != 10)
                    this.ch = 10;
                if (this.ch == 10)
                {
                    this.line++;
                    this.col = 0;
                }
            }
        },
        AddCh: function ()
        {
            if (this.tlen >= this.tval.length)
            {
                var newBuf = [];
                for (var i = 0; i < this.tval.length; i++)
                {
                    newBuf[i] = this.tval[i];
                }
                this.tval = newBuf;
            }
            if (this.ch != 65536)
            {
                this.tval[this.tlen++] = this.GetCurrentChar();
                this.NextCh();
            }
        },
        GetCurrentChar: function ()
        {
            return SharpAlg.Native.PlatformHelper.IntToChar(this.ch);
        },
        CheckLiteral: function ()
        {
            switch (this.t.val)
            {
                default :
                    break;
            }
        },
        NextToken: function ()
        {
            while (this.ch == 32 || this.ch >= 9 && this.ch <= 10 || this.ch == 13)
            this.NextCh();
            var recKind = 13;
            var recEnd = this.pos;
            this.t = {};
            this.t.pos = this.pos;
            this.t.col = this.col;
            this.t.line = this.line;
            this.t.charPos = this.charPos;
            var state;
            if (SharpAlg.Native.Parser.Scanner.start.ContainsKey(this.ch))
            {
                state = SharpAlg.Native.Parser.Scanner.start.get_Item$$TKey(this.ch);
            }
            else
            {
                state = 0;
            }
            this.tlen = 0;
            this.AddCh();
            var done = false;
            while (!done)
            {
                switch (state)
                {
                    case -1:
                        {
                            this.t.kind = 0;
                            done = true;
                            break;
                        }
                    case 0:
                        {
                            if (recKind != 13)
                            {
                                this.tlen = recEnd - this.t.pos;
                                this.SetScannerBehindT();
                            }
                            this.t.kind = recKind;
                            done = true;
                            break;
                        }
                    case 1:
                        recEnd = this.pos;
                        recKind = 1;
                        if (this.ch >= 48 && this.ch <= 57 || this.ch >= 65 && this.ch <= 90 || this.ch >= 97 && this.ch <= 122)
                        {
                            this.AddCh();
                            state = 1;
                            break;
                        }
                        else
                        {
                            this.t.kind = 1;
                            done = true;
                            break;
                        }
                    case 2:
                        if (this.ch >= 48 && this.ch <= 57)
                        {
                            this.AddCh();
                            state = 3;
                            break;
                        }
                        else
                        {
                            state = 0;
                            break;
                        }
                    case 3:
                        recEnd = this.pos;
                        recKind = 3;
                        if (this.ch >= 48 && this.ch <= 57)
                        {
                            this.AddCh();
                            state = 3;
                            break;
                        }
                        else
                        {
                            this.t.kind = 3;
                            done = true;
                            break;
                        }
                    case 4:
                        recEnd = this.pos;
                        recKind = 2;
                        if (this.ch >= 48 && this.ch <= 57)
                        {
                            this.AddCh();
                            state = 4;
                            break;
                        }
                        else if (this.ch == 46)
                        {
                            this.AddCh();
                            state = 2;
                            break;
                        }
                        else
                        {
                            this.t.kind = 2;
                            done = true;
                            break;
                        }
                    case 5:
                        {
                            this.t.kind = 4;
                            done = true;
                            break;
                        }
                    case 6:
                        {
                            this.t.kind = 5;
                            done = true;
                            break;
                        }
                    case 7:
                        {
                            this.t.kind = 6;
                            done = true;
                            break;
                        }
                    case 8:
                        {
                            this.t.kind = 7;
                            done = true;
                            break;
                        }
                    case 9:
                        {
                            this.t.kind = 8;
                            done = true;
                            break;
                        }
                    case 10:
                        {
                            this.t.kind = 9;
                            done = true;
                            break;
                        }
                    case 11:
                        {
                            this.t.kind = 10;
                            done = true;
                            break;
                        }
                    case 12:
                        {
                            this.t.kind = 11;
                            done = true;
                            break;
                        }
                    case 13:
                        {
                            this.t.kind = 12;
                            done = true;
                            break;
                        }
                }
            }
            this.t.val = System.String.Empty;
            for (var i = 0; i < this.tlen; i++)
            {
                this.t.val += this.tval[i];
            }
            return this.t;
        },
        SetScannerBehindT: function ()
        {
            this.buffer.set_Pos(this.t.pos);
            this.NextCh();
            this.line = this.t.line;
            this.col = this.t.col;
            this.charPos = this.t.charPos;
            for (var i = 0; i < this.tlen; i++)
                this.NextCh();
        },
        Scan: function ()
        {
            if (this.tokens.next == null)
            {
                return this.NextToken();
            }
            else
            {
                this.pt = this.tokens = this.tokens.next;
                return this.tokens;
            }
        },
        Peek: function ()
        {
            do {
                if (this.pt.next == null)
                {
                    this.pt.next = this.NextToken();
                }
                this.pt = this.pt.next;
            }
            while (this.pt.kind > 13)
            return this.pt;
        },
        ResetPeek: function ()
        {
            this.pt = this.tokens;
        }
    }
};
JsTypes.push(SharpAlg$Native$Parser$Scanner);
