/*Generated by SharpKit 5 v5.01.1000*/
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
if (typeof ($CreateAnonymousDelegate) == 'undefined') {
    var $CreateAnonymousDelegate = function (target, func) {
        if (target == null || func == null)
            return func;
        var delegate = function () {
            return func.apply(target, arguments);
        };
        delegate.func = func;
        delegate.target = target;
        delegate.isDelegate = true;
        return delegate;
    }
}
if (typeof(JsTypes) == "undefined")
    var JsTypes = [];
var SharpAlg$Native$ContextFactory =
{
    fullname: "SharpAlg.Native.ContextFactory",
    baseTypeName: "System.Object",
    staticDefinition:
    {
        CreateEmpty: function ()
        {
            return new SharpAlg.Native.Context.ctor();
        },
        CreateDefault: function ()
        {
            return new SharpAlg.Native.Context.ctor().Register$$Function(SharpAlg.Native.Functions.get_Factorial()).Register$$Function(SharpAlg.Native.Functions.get_Factorial()).Register$$Function(SharpAlg.Native.Functions.get_Ln()).Register$$Function(SharpAlg.Native.Functions.get_Diff());
        },
        cctor: function ()
        {
            SharpAlg.Native.ContextFactory.Empty = null;
            SharpAlg.Native.ContextFactory.Default = null;
            SharpAlg.Native.ContextFactory.Default = SharpAlg.Native.ContextFactory.CreateDefault();
            SharpAlg.Native.ContextFactory.Default.set_ReadOnly(true);
            SharpAlg.Native.ContextFactory.Empty = SharpAlg.Native.ContextFactory.CreateEmpty();
            SharpAlg.Native.ContextFactory.Empty.set_ReadOnly(true);
        }
    },
    assemblyName: "SharpAlg",
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            System.Object.ctor.call(this);
        }
    }
};
JsTypes.push(SharpAlg$Native$ContextFactory);
if (typeof(SharpAlg) == "undefined")
    var SharpAlg = {};
if (typeof(SharpAlg.Native) == "undefined")
    SharpAlg.Native = {};
SharpAlg.Native.DefaultExpressionVisitor = function ()
{
};
SharpAlg.Native.DefaultExpressionVisitor.prototype.Constant = function (constant)
{
    return this.GetDefault(constant);
};
SharpAlg.Native.DefaultExpressionVisitor.prototype.Parameter = function (parameter)
{
    return this.GetDefault(parameter);
};
SharpAlg.Native.DefaultExpressionVisitor.prototype.Add = function (multi)
{
    return this.GetDefault(multi);
};
SharpAlg.Native.DefaultExpressionVisitor.prototype.Multiply = function (multi)
{
    return this.GetDefault(multi);
};
SharpAlg.Native.DefaultExpressionVisitor.prototype.Power = function (power)
{
    return this.GetDefault(power);
};
SharpAlg.Native.DefaultExpressionVisitor.prototype.Function = function (functionExpr)
{
    return this.GetDefault(functionExpr);
};
var SharpAlg$Native$ExpressionExtensions =
{
    fullname: "SharpAlg.Native.ExpressionExtensions",
    baseTypeName: "System.Object",
    staticDefinition:
    {
        Evaluate: function (expr, context)
        {
            return expr.Visit$1(SharpAlg.Native.Number.ctor, new SharpAlg.Native.ExpressionEvaluator((context != null ? context : SharpAlg.Native.ContextFactory.Default)));
        },
        Diff: function (expr, parameterName)
        {
            return expr.Visit$1(SharpAlg.Native.Expr.ctor, new SharpAlg.Native.DiffExpressionVisitor(SharpAlg.Native.Builder.ConvolutionExprBuilder.CreateDefault(), SharpAlg.Native.ContextFactory.Default, parameterName));
        },
        ExprEquals: function (expr1, expr2)
        {
            return expr1.Visit$1(System.Boolean.ctor, new SharpAlg.Native.ExpressionEqualityComparer.ctor(expr2));
        },
        ExprEquivalent: function (expr1, expr2)
        {
            return expr1.Visit$1(System.Boolean.ctor, new SharpAlg.Native.ExpressionEquivalenceComparer.ctor(expr2));
        },
        Print: function (expr)
        {
            return expr.Visit$1(System.String.ctor, SharpAlg.Native.Printer.ExpressionPrinter.Instance);
        },
        Parse: function (expression, builder)
        {
            return SharpAlg.Native.ExpressionExtensions.GetExpression(SharpAlg.Native.ExpressionExtensions.ParseCore(expression, (builder != null ? builder : SharpAlg.Native.Builder.ConvolutionExprBuilder.CreateDefault())));
        },
        GetExpression: function (parser)
        {
            if (parser.errors.Count > 0)
                throw $CreateException(new System.InvalidOperationException.ctor$$String("String can not be parsed"), new Error());
            return parser.Expr;
        },
        ParseCore: function (expression, builder)
        {
            var scanner = new SharpAlg.Native.Parser.Scanner.ctor(expression);
            var parser = new SharpAlg.Native.Parser.Parser(scanner, builder);
            parser.Parse();
            return parser;
        }
    },
    assemblyName: "SharpAlg",
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            System.Object.ctor.call(this);
        }
    }
};
JsTypes.push(SharpAlg$Native$ExpressionExtensions);
var SharpAlg$Native$InvalidArgumentCountException =
{
    fullname: "SharpAlg.Native.InvalidArgumentCountException",
    baseTypeName: "System.Exception",
    assemblyName: "SharpAlg",
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            System.Exception.ctor.call(this);
        },
        ctor$$String: function (message)
        {
            System.Exception.ctor$$String.call(this, message);
        }
    }
};
JsTypes.push(SharpAlg$Native$InvalidArgumentCountException);
var SharpAlg$Native$SingleArgumentFunction =
{
    fullname: "SharpAlg.Native.SingleArgumentFunction",
    baseTypeName: "SharpAlg.Native.Function",
    staticDefinition:
    {
        IsValidArgsCount$1: function (T, args)
        {
            return System.Linq.Enumerable.Count$1$$IEnumerable$1(T, args) == 1;
        },
        CheckArgsCount$1: function (T, args)
        {
            if (!SharpAlg.Native.SingleArgumentFunction.IsValidArgsCount$1(T, args))
                throw $CreateException(new SharpAlg.Native.InvalidArgumentCountException.ctor(), new Error());
        }
    },
    assemblyName: "SharpAlg",
    interfaceNames: ["SharpAlg.Native.ISupportCheckArgs"],
    Kind: "Class",
    definition:
    {
        ctor: function (name)
        {
            SharpAlg.Native.Function.ctor.call(this, name);
        },
        Evaluate: function (evaluator, args)
        {
            return this.EvaluateCore(System.Linq.Enumerable.Select$2$$IEnumerable$1$$Func$2(SharpAlg.Native.Expr.ctor, SharpAlg.Native.Number.ctor, args, $CreateAnonymousDelegate(this, function (x)
            {
                return x.Visit$1(SharpAlg.Native.Number.ctor, evaluator);
            })));
        },
        EvaluateCore: function (args)
        {
            SharpAlg.Native.SingleArgumentFunction.CheckArgsCount$1(SharpAlg.Native.Number.ctor, args);
            return this.Evaluate$$Number(System.Linq.Enumerable.First$1$$IEnumerable$1(SharpAlg.Native.Number.ctor, args));
        },
        Check: function (args)
        {
            return SharpAlg.Native.SingleArgumentFunction.IsValidArgsCount$1(SharpAlg.Native.Expr.ctor, args) ? System.String.Empty : System.String.Format$$String$$Object$$Object("Error, (in {0}) expecting 1 argument, got {1}", this.get_Name(), System.Linq.Enumerable.Count$1$$IEnumerable$1(SharpAlg.Native.Expr.ctor, args));
        }
    }
};
JsTypes.push(SharpAlg$Native$SingleArgumentFunction);
var SharpAlg$Native$SingleArgumentDifferentiableFunction =
{
    fullname: "SharpAlg.Native.SingleArgumentDifferentiableFunction",
    baseTypeName: "SharpAlg.Native.SingleArgumentFunction",
    assemblyName: "SharpAlg",
    interfaceNames: ["SharpAlg.Native.ISupportDiff"],
    Kind: "Class",
    definition:
    {
        ctor: function (name)
        {
            SharpAlg.Native.SingleArgumentFunction.ctor.call(this, name);
        },
        Diff: function (diffVisitor, args)
        {
            SharpAlg.Native.SingleArgumentFunction.CheckArgsCount$1(SharpAlg.Native.Expr.ctor, args);
            var arg = System.Linq.Enumerable.First$1$$IEnumerable$1(SharpAlg.Native.Expr.ctor, args);
            return diffVisitor.get_Builder().Multiply(arg.Visit$1(SharpAlg.Native.Expr.ctor, diffVisitor), this.DiffCore(diffVisitor.get_Builder(), arg));
        }
    }
};
JsTypes.push(SharpAlg$Native$SingleArgumentDifferentiableFunction);
var SharpAlg$Native$FactorialFunction =
{
    fullname: "SharpAlg.Native.FactorialFunction",
    baseTypeName: "SharpAlg.Native.SingleArgumentFunction",
    assemblyName: "SharpAlg",
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            SharpAlg.Native.SingleArgumentFunction.ctor.call(this, "factorial");
        },
        Evaluate$$Number: function (arg)
        {
            var result = SharpAlg.Native.Number.One;
            for (var i = SharpAlg.Native.Number.Two; SharpAlg.Native.Number.op_LessThanOrEqual(i, arg); i = SharpAlg.Native.Number.op_Addition(i, SharpAlg.Native.Number.One))
            {
                result = SharpAlg.Native.Number.op_Multiply(result, i);
            }
            return result;
        }
    }
};
JsTypes.push(SharpAlg$Native$FactorialFunction);
var SharpAlg$Native$LnFunction =
{
    fullname: "SharpAlg.Native.LnFunction",
    baseTypeName: "SharpAlg.Native.SingleArgumentDifferentiableFunction",
    assemblyName: "SharpAlg",
    interfaceNames: ["SharpAlg.Native.ISupportConvolution"],
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            SharpAlg.Native.SingleArgumentDifferentiableFunction.ctor.call(this, "ln");
        },
        Evaluate$$Number: function (arg)
        {
            return SharpAlg.Native.Number.Ln(arg);
        },
        DiffCore: function (builder, arg)
        {
            return builder.Inverse(arg);
        },
        Convolute: function (args)
        {
            if (SharpAlg.Native.ExpressionExtensions.ExprEquals(System.Linq.Enumerable.First$1$$IEnumerable$1(SharpAlg.Native.Expr.ctor, args), SharpAlg.Native.Expr.One))
                return SharpAlg.Native.Expr.Zero;
            return null;
        }
    }
};
JsTypes.push(SharpAlg$Native$LnFunction);
var SharpAlg$Native$DiffFunction =
{
    fullname: "SharpAlg.Native.DiffFunction",
    baseTypeName: "SharpAlg.Native.Function",
    assemblyName: "SharpAlg",
    interfaceNames: ["SharpAlg.Native.ISupportConvolution"],
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            SharpAlg.Native.Function.ctor.call(this, "diff");
        },
        Evaluate: function (evaluator, args)
        {
            return this.Convolute(args).Visit$1(SharpAlg.Native.Number.ctor, evaluator);
        },
        Convolute: function (args)
        {
            var argsTail = SharpAlg.Native.FunctionalExtensions.Tail$1(SharpAlg.Native.Expr.ctor, args);
            if (!System.Linq.Enumerable.All$1(SharpAlg.Native.Expr.ctor, argsTail, $CreateAnonymousDelegate(this, function (x)
            {
                return Is(x, SharpAlg.Native.ParameterExpr.ctor);
            })))
                throw $CreateException(new SharpAlg.Native.ExpressionDefferentiationException.ctor$$String("All diff arguments should be parameters"), new Error());
            var diffList = System.Linq.Enumerable.Cast$1(SharpAlg.Native.ParameterExpr.ctor, argsTail);
            if (!System.Linq.Enumerable.Any$1$$IEnumerable$1(SharpAlg.Native.ParameterExpr.ctor, diffList))
                return SharpAlg.Native.ExpressionExtensions.Diff(System.Linq.Enumerable.First$1$$IEnumerable$1(SharpAlg.Native.Expr.ctor, args), null);
            var result = System.Linq.Enumerable.First$1$$IEnumerable$1(SharpAlg.Native.Expr.ctor, args);
            SharpAlg.Native.FunctionalExtensions.ForEach$1(SharpAlg.Native.ParameterExpr.ctor, diffList, $CreateAnonymousDelegate(this, function (x)
            {
                result = SharpAlg.Native.ExpressionExtensions.Diff(result, x.get_ParameterName());
            }));
            return result;
        }
    }
};
JsTypes.push(SharpAlg$Native$DiffFunction);
var SharpAlg$Native$Functions =
{
    fullname: "SharpAlg.Native.Functions",
    baseTypeName: "System.Object",
    staticDefinition:
    {
        cctor: function ()
        {
            SharpAlg.Native.Functions.factorial = null;
            SharpAlg.Native.Functions.ln = null;
            SharpAlg.Native.Functions.diff = null;
        },
        Factorial$$: "SharpAlg.Native.Function",
        get_Factorial: function ()
        {
            return (SharpAlg.Native.Functions.factorial != null ? SharpAlg.Native.Functions.factorial : (SharpAlg.Native.Functions.factorial = new SharpAlg.Native.FactorialFunction.ctor()));
        },
        Ln$$: "SharpAlg.Native.Function",
        get_Ln: function ()
        {
            return (SharpAlg.Native.Functions.ln != null ? SharpAlg.Native.Functions.ln : (SharpAlg.Native.Functions.ln = new SharpAlg.Native.LnFunction.ctor()));
        },
        Diff$$: "SharpAlg.Native.Function",
        get_Diff: function ()
        {
            return (SharpAlg.Native.Functions.diff != null ? SharpAlg.Native.Functions.diff : (SharpAlg.Native.Functions.diff = new SharpAlg.Native.DiffFunction.ctor()));
        }
    },
    assemblyName: "SharpAlg",
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            System.Object.ctor.call(this);
        }
    }
};
JsTypes.push(SharpAlg$Native$Functions);
