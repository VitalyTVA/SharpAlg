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
if (typeof($CreateDelegate)=='undefined'){
    if(typeof($iKey)=='undefined') var $iKey = 0;
    if(typeof($pKey)=='undefined') var $pKey = String.fromCharCode(1);
    var $CreateDelegate = function(target, func){
        if (target == null || func == null) 
            return func;
        if(func.target==target && func.func==func)
            return func;
        if (target.$delegateCache == null)
            target.$delegateCache = {};
        if (func.$key == null)
            func.$key = $pKey + String(++$iKey);
        var delegate;
        if(target.$delegateCache!=null)
            delegate = target.$delegateCache[func.$key];
        if (delegate == null){
            delegate = function(){
                return func.apply(target, arguments);
            };
            delegate.func = func;
            delegate.target = target;
            delegate.isDelegate = true;
            if(target.$delegateCache!=null)
                target.$delegateCache[func.$key] = delegate;
        }
        return delegate;
    }
}
if (typeof(JsTypes) == "undefined")
    var JsTypes = [];
var SharpAlg$Native$Context =
{
    fullname: "SharpAlg.Native.Context",
    baseTypeName: "System.Object",
    assemblyName: "SharpAlg",
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            this.names = new System.Collections.Generic.Dictionary$2.ctor(System.String.ctor, SharpAlg.Native.Expr.ctor);
            System.Object.ctor.call(this);
        },
        Register: function (name, value)
        {
            this.names.set_Item$$TKey(name, value);
        },
        GetValue: function (name)
        {
            var result;
            (function ()
            {
                result = {Value: result};
                var $res = this.names.TryGetValue(name, result);
                result = result.Value;
                return $res;
            }).call(this);
            return result;
        }
    }
};
JsTypes.push(SharpAlg$Native$Context);
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
    throw $CreateException(new System.NotImplementedException.ctor(), new Error());
};
SharpAlg.Native.DiffExpressionVisitor = function (builder, parameterName)
{
    this.builder = null;
    this.parameterName = null;
    this.autoParameterName = false;
    this.builder = builder;
    this.parameterName = parameterName;
    this.autoParameterName = !this.get_HasParameter();
};
SharpAlg.Native.DiffExpressionVisitor.prototype.get_HasParameter = function ()
{
    return !System.String.IsNullOrEmpty(this.parameterName);
};
SharpAlg.Native.DiffExpressionVisitor.prototype.Constant = function (constant)
{
    return SharpAlg.Native.Expr.Zero;
};
SharpAlg.Native.DiffExpressionVisitor.prototype.Parameter = function (parameter)
{
    if (!this.get_HasParameter())
    {
        this.parameterName = parameter.get_ParameterName();
        this.autoParameterName = true;
    }
    if (this.parameterName == parameter.get_ParameterName())
    {
        return SharpAlg.Native.Expr.One;
    }
    else
    {
        if (this.autoParameterName)
            throw $CreateException(new SharpAlg.Native.ExpressionDefferentiationException.ctor$$String("Expression contains more than one independent variable"), new Error());
        return SharpAlg.Native.Expr.Zero;
    }
};
SharpAlg.Native.DiffExpressionVisitor.prototype.Add = function (multi)
{
    var result = null;
    SharpAlg.Native.FunctionalExtensions.Accumulate$1(SharpAlg.Native.Expr.ctor, multi.get_Args(), $CreateAnonymousDelegate(this, function (x)
    {
        result = x.Visit$1(SharpAlg.Native.Expr.ctor, this);
    }), $CreateAnonymousDelegate(this, function (x)
    {
        result = this.builder.Add(result, x.Visit$1(SharpAlg.Native.Expr.ctor, this));
    }));
    return result;
};
SharpAlg.Native.DiffExpressionVisitor.prototype.Multiply = function (multi)
{
    var tail = SharpAlg.Native.ExpressionExtensions.Tail$$MultiplyExpr(multi);
    var expr1 = this.builder.Multiply(System.Linq.Enumerable.First$1$$IEnumerable$1(SharpAlg.Native.Expr.ctor, multi.get_Args()).Visit$1(SharpAlg.Native.Expr.ctor, this), tail);
    var expr2 = this.builder.Multiply(System.Linq.Enumerable.First$1$$IEnumerable$1(SharpAlg.Native.Expr.ctor, multi.get_Args()), tail.Visit$1(SharpAlg.Native.Expr.ctor, this));
    return this.builder.Add(expr1, expr2);
};
SharpAlg.Native.DiffExpressionVisitor.prototype.Power = function (power)
{
    if (!(Is(power.get_Right(), SharpAlg.Native.ConstantExpr.ctor)))
        throw $CreateException(new System.NotImplementedException.ctor(), new Error());
    return SharpAlg.Native.Expr.Multiply$$Expr$$Expr(power.get_Right(), this.builder.Multiply(power.get_Left().Visit$1(SharpAlg.Native.Expr.ctor, this), this.builder.Power(power.get_Left(), this.builder.Subtract(power.get_Right(), SharpAlg.Native.Expr.One))));
};
SharpAlg.Native.DiffExpressionVisitor.prototype.Function = function (functionExpr)
{
    throw $CreateException(new System.NotImplementedException.ctor(), new Error());
};
var SharpAlg$Native$ExpressionDefferentiationException =
{
    fullname: "SharpAlg.Native.ExpressionDefferentiationException",
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
JsTypes.push(SharpAlg$Native$ExpressionDefferentiationException);
var SharpAlg$Native$Expr =
{
    fullname: "SharpAlg.Native.Expr",
    baseTypeName: "System.Object",
    staticDefinition:
    {
        cctor: function ()
        {
            SharpAlg.Native.Expr.Zero = new SharpAlg.Native.ConstantExpr.ctor(SharpAlg.Native.Number.Zero);
            SharpAlg.Native.Expr.One = new SharpAlg.Native.ConstantExpr.ctor(SharpAlg.Native.Number.One);
            SharpAlg.Native.Expr.MinusOne = new SharpAlg.Native.ConstantExpr.ctor(SharpAlg.Native.Number.MinusOne);
        },
        Constant: function (constant)
        {
            return new SharpAlg.Native.ConstantExpr.ctor(constant);
        },
        Parameter: function (parameterName)
        {
            return new SharpAlg.Native.ParameterExpr.ctor(parameterName);
        },
        Binary: function (left, right, type)
        {
            return SharpAlg.Native.Expr.Multi([left, right], type);
        },
        Multi: function (args, type)
        {
            return System.Linq.Enumerable.Count$1$$IEnumerable$1(SharpAlg.Native.Expr.ctor, args) > 1 ? (type == 0 ? new SharpAlg.Native.AddExpr.ctor(args) : new SharpAlg.Native.MultiplyExpr.ctor(args)) : System.Linq.Enumerable.First$1$$IEnumerable$1(SharpAlg.Native.Expr.ctor, args);
        },
        Add$$IEnumerable$1$Expr: function (args)
        {
            return SharpAlg.Native.Expr.Multi(args, 0);
        },
        Multiply$$IEnumerable$1$Expr: function (args)
        {
            return SharpAlg.Native.Expr.Multi(args, 1);
        },
        Add$$Expr$$Expr: function (left, right)
        {
            return SharpAlg.Native.Expr.Binary(left, right, 0);
        },
        Subtract: function (left, right)
        {
            return SharpAlg.Native.Expr.Add$$Expr$$Expr(left, SharpAlg.Native.Expr.Minus(right));
        },
        Multiply$$Expr$$Expr: function (left, right)
        {
            return SharpAlg.Native.Expr.Binary(left, right, 1);
        },
        Divide: function (left, right)
        {
            return SharpAlg.Native.Expr.Multiply$$Expr$$Expr(left, SharpAlg.Native.Expr.Inverse(right));
        },
        Power: function (left, right)
        {
            return new SharpAlg.Native.PowerExpr.ctor(left, right);
        },
        Minus: function (expr)
        {
            return SharpAlg.Native.Expr.Multiply$$Expr$$Expr(SharpAlg.Native.Expr.MinusOne, expr);
        },
        Inverse: function (expr)
        {
            return SharpAlg.Native.Expr.Power(expr, SharpAlg.Native.Expr.MinusOne);
        },
        Function: function (functionName)
        {
            return new SharpAlg.Native.FunctionExpr.ctor(functionName);
        }
    },
    assemblyName: "SharpAlg",
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            System.Object.ctor.call(this);
        },
        Print$$: "System.String",
        get_Print: function ()
        {
            return SharpAlg.Native.ExpressionExtensions.Print(this);
        }
    }
};
JsTypes.push(SharpAlg$Native$Expr);
var SharpAlg$Native$ConstantExpr =
{
    fullname: "SharpAlg.Native.ConstantExpr",
    baseTypeName: "SharpAlg.Native.Expr",
    staticDefinition:
    {
        cctor: function ()
        {
        }
    },
    assemblyName: "SharpAlg",
    Kind: "Class",
    definition:
    {
        ctor: function (value)
        {
            this._Value = null;
            SharpAlg.Native.Expr.ctor.call(this);
            this.set_Value(value);
        },
        Value$$: "SharpAlg.Native.Number",
        get_Value: function ()
        {
            return this._Value;
        },
        set_Value: function (value)
        {
            this._Value = value;
        },
        Visit$1: function (T, visitor)
        {
            return visitor.Constant(this);
        }
    }
};
JsTypes.push(SharpAlg$Native$ConstantExpr);
var SharpAlg$Native$ParameterExpr =
{
    fullname: "SharpAlg.Native.ParameterExpr",
    baseTypeName: "SharpAlg.Native.Expr",
    staticDefinition:
    {
        cctor: function ()
        {
        }
    },
    assemblyName: "SharpAlg",
    Kind: "Class",
    definition:
    {
        ctor: function (parameterName)
        {
            this._ParameterName = null;
            SharpAlg.Native.Expr.ctor.call(this);
            this.set_ParameterName(parameterName);
        },
        ParameterName$$: "System.String",
        get_ParameterName: function ()
        {
            return this._ParameterName;
        },
        set_ParameterName: function (value)
        {
            this._ParameterName = value;
        },
        Visit$1: function (T, visitor)
        {
            return visitor.Parameter(this);
        }
    }
};
JsTypes.push(SharpAlg$Native$ParameterExpr);
var SharpAlg$Native$MultiExpr =
{
    fullname: "SharpAlg.Native.MultiExpr",
    baseTypeName: "SharpAlg.Native.Expr",
    staticDefinition:
    {
        cctor: function ()
        {
        }
    },
    assemblyName: "SharpAlg",
    Kind: "Class",
    definition:
    {
        ctor: function (args)
        {
            this._Args = null;
            SharpAlg.Native.Expr.ctor.call(this);
            this.set_Args(args);
        },
        Args$$: "System.Collections.Generic.IEnumerable`1[[SharpAlg.Native.Expr]]",
        get_Args: function ()
        {
            return this._Args;
        },
        set_Args: function (value)
        {
            this._Args = value;
        }
    }
};
JsTypes.push(SharpAlg$Native$MultiExpr);
var SharpAlg$Native$AddExpr =
{
    fullname: "SharpAlg.Native.AddExpr",
    baseTypeName: "SharpAlg.Native.MultiExpr",
    staticDefinition:
    {
        cctor: function ()
        {
        }
    },
    assemblyName: "SharpAlg",
    Kind: "Class",
    definition:
    {
        ctor: function (args)
        {
            SharpAlg.Native.MultiExpr.ctor.call(this, args);
        },
        Visit$1: function (T, visitor)
        {
            return visitor.Add(this);
        }
    }
};
JsTypes.push(SharpAlg$Native$AddExpr);
var SharpAlg$Native$MultiplyExpr =
{
    fullname: "SharpAlg.Native.MultiplyExpr",
    baseTypeName: "SharpAlg.Native.MultiExpr",
    staticDefinition:
    {
        cctor: function ()
        {
        }
    },
    assemblyName: "SharpAlg",
    Kind: "Class",
    definition:
    {
        ctor: function (args)
        {
            SharpAlg.Native.MultiExpr.ctor.call(this, args);
        },
        Visit$1: function (T, visitor)
        {
            return visitor.Multiply(this);
        }
    }
};
JsTypes.push(SharpAlg$Native$MultiplyExpr);
var SharpAlg$Native$PowerExpr =
{
    fullname: "SharpAlg.Native.PowerExpr",
    baseTypeName: "SharpAlg.Native.Expr",
    staticDefinition:
    {
        cctor: function ()
        {
        }
    },
    assemblyName: "SharpAlg",
    Kind: "Class",
    definition:
    {
        ctor: function (left, right)
        {
            this._Left = null;
            this._Right = null;
            SharpAlg.Native.Expr.ctor.call(this);
            this.set_Right(right);
            this.set_Left(left);
        },
        Left$$: "SharpAlg.Native.Expr",
        get_Left: function ()
        {
            return this._Left;
        },
        set_Left: function (value)
        {
            this._Left = value;
        },
        Right$$: "SharpAlg.Native.Expr",
        get_Right: function ()
        {
            return this._Right;
        },
        set_Right: function (value)
        {
            this._Right = value;
        },
        Visit$1: function (T, visitor)
        {
            return visitor.Power(this);
        }
    }
};
JsTypes.push(SharpAlg$Native$PowerExpr);
var SharpAlg$Native$FunctionExpr =
{
    fullname: "SharpAlg.Native.FunctionExpr",
    baseTypeName: "SharpAlg.Native.Expr",
    staticDefinition:
    {
        cctor: function ()
        {
        }
    },
    assemblyName: "SharpAlg",
    Kind: "Class",
    definition:
    {
        ctor: function (functionName)
        {
            this._FunctionName = null;
            SharpAlg.Native.Expr.ctor.call(this);
            this.set_FunctionName(functionName);
        },
        FunctionName$$: "System.String",
        get_FunctionName: function ()
        {
            return this._FunctionName;
        },
        set_FunctionName: function (value)
        {
            this._FunctionName = value;
        },
        Visit$1: function (T, visitor)
        {
            return visitor.Function(this);
        }
    }
};
JsTypes.push(SharpAlg$Native$FunctionExpr);
var SharpAlg$Native$ExpressionEqualityComparer =
{
    fullname: "SharpAlg.Native.ExpressionEqualityComparer",
    baseTypeName: "System.Object",
    assemblyName: "SharpAlg",
    interfaceNames: ["SharpAlg.Native.IExpressionVisitor$1"],
    Kind: "Class",
    definition:
    {
        ctor: function (expr)
        {
            this.expr = null;
            System.Object.ctor.call(this);
            this.expr = expr;
        },
        Constant: function (constant)
        {
            return this.DoEqualityCheck$1(SharpAlg.Native.ConstantExpr.ctor, constant, $CreateAnonymousDelegate(this, function (x1, x2)
            {
                return System.Object.Equals$$Object$$Object(x1.get_Value(), x2.get_Value());
            }));
        },
        Add: function (multi)
        {
            return this.CompareMultiExpr$1(SharpAlg.Native.AddExpr.ctor, multi);
        },
        Multiply: function (multi)
        {
            return this.CompareMultiExpr$1(SharpAlg.Native.MultiplyExpr.ctor, multi);
        },
        Power: function (power)
        {
            return this.DoEqualityCheck$1(SharpAlg.Native.PowerExpr.ctor, power, $CreateAnonymousDelegate(this, function (x1, x2)
            {
                return this.EqualsCore(x1.get_Left(), x2.get_Left()) && this.EqualsCore(x1.get_Right(), x2.get_Right());
            }));
        },
        Parameter: function (parameter)
        {
            return this.DoEqualityCheck$1(SharpAlg.Native.ParameterExpr.ctor, parameter, $CreateAnonymousDelegate(this, function (x1, x2)
            {
                return x1.get_ParameterName() == x2.get_ParameterName();
            }));
        },
        Function: function (functionExpr)
        {
            return this.DoEqualityCheck$1(SharpAlg.Native.FunctionExpr.ctor, functionExpr, $CreateAnonymousDelegate(this, function (x1, x2)
            {
                return x1.get_FunctionName() == x2.get_FunctionName();
            }));
        },
        DoEqualityCheck$1: function (T, expr2, equalityCheck)
        {
            var other = As(this.expr, T);
            return other != null && equalityCheck(other, expr2);
        },
        EqualsCore: function (expr1, expr2)
        {
            return expr1.Visit$1(System.Boolean.ctor, this.Clone(expr2));
        },
        GetArgsEqualComparer: function ()
        {
            return $CreateAnonymousDelegate(this, function (x, y)
            {
                return SharpAlg.Native.FunctionalExtensions.EnumerableEqual$1(SharpAlg.Native.Expr.ctor, x, y, $CreateDelegate(this, this.EqualsCore));
            });
        },
        Clone: function (expr)
        {
            return new SharpAlg.Native.ExpressionEqualityComparer.ctor(expr);
        },
        CompareMultiExpr$1: function (T, multi)
        {
            return this.DoEqualityCheck$1(T, multi, $CreateAnonymousDelegate(this, function (x1, x2)
            {
                return this.GetArgsEqualComparer()(x1.get_Args(), x2.get_Args());
            }));
        }
    }
};
JsTypes.push(SharpAlg$Native$ExpressionEqualityComparer);
var SharpAlg$Native$ExpressionEquivalenceComparer =
{
    fullname: "SharpAlg.Native.ExpressionEquivalenceComparer",
    baseTypeName: "SharpAlg.Native.ExpressionEqualityComparer",
    assemblyName: "SharpAlg",
    Kind: "Class",
    definition:
    {
        ctor: function (expr)
        {
            SharpAlg.Native.ExpressionEqualityComparer.ctor.call(this, expr);
        },
        GetArgsEqualComparer: function ()
        {
            return $CreateAnonymousDelegate(this, function (x, y)
            {
                return SharpAlg.Native.FunctionalExtensions.SetEqual$1(SharpAlg.Native.Expr.ctor, x, y, $CreateDelegate(this, this.EqualsCore));
            });
        },
        Clone: function (expr)
        {
            return new SharpAlg.Native.ExpressionEquivalenceComparer.ctor(expr);
        }
    }
};
JsTypes.push(SharpAlg$Native$ExpressionEquivalenceComparer);
SharpAlg.Native.ExpressionEvaluator = function (context)
{
    this.context = null;
    this.context = context;
};
SharpAlg.Native.ExpressionEvaluator.prototype.Constant = function (constant)
{
    return constant.get_Value();
};
SharpAlg.Native.ExpressionEvaluator.prototype.Add = function (multi)
{
    return this.EvaluateMulti(multi, $CreateAnonymousDelegate(this, function (x1, x2)
    {
        return SharpAlg.Native.Number.op_Addition(x1, x2);
    }));
};
SharpAlg.Native.ExpressionEvaluator.prototype.Multiply = function (multi)
{
    return this.EvaluateMulti(multi, $CreateAnonymousDelegate(this, function (x1, x2)
    {
        return SharpAlg.Native.Number.op_Multiply(x1, x2);
    }));
};
SharpAlg.Native.ExpressionEvaluator.prototype.EvaluateMulti = function (multi, evaluator)
{
    var result = SharpAlg.Native.Number.Zero;
    SharpAlg.Native.FunctionalExtensions.Accumulate$1(SharpAlg.Native.Expr.ctor, multi.get_Args(), $CreateAnonymousDelegate(this, function (x)
    {
        result = x.Visit$1(SharpAlg.Native.Number.ctor, this);
    }), $CreateAnonymousDelegate(this, function (x)
    {
        result = evaluator(result, x.Visit$1(SharpAlg.Native.Number.ctor, this));
    }));
    return result;
};
SharpAlg.Native.ExpressionEvaluator.prototype.Power = function (power)
{
    return SharpAlg.Native.Number.op_ExclusiveOr(power.get_Left().Visit$1(SharpAlg.Native.Number.ctor, this), power.get_Right().Visit$1(SharpAlg.Native.Number.ctor, this));
};
SharpAlg.Native.ExpressionEvaluator.prototype.Parameter = function (parameter)
{
    var parameterValue = this.context.GetValue(parameter.get_ParameterName());
    if (parameterValue == null)
        throw $CreateException(new SharpAlg.Native.ExpressionEvaluationException.ctor$$String(System.String.Format$$String$$Object("{0} value is undefined", parameter.get_ParameterName())), new Error());
    return parameterValue.Visit$1(SharpAlg.Native.Number.ctor, this);
};
SharpAlg.Native.ExpressionEvaluator.GetBinaryOperationEvaluator = function (operation)
{
    switch (operation)
    {
        case 0:
            return function (x1, x2)
            {
                return SharpAlg.Native.Number.op_Addition(x1, x2);
            };
        case 1:
            return function (x1, x2)
            {
                return SharpAlg.Native.Number.op_Multiply(x1, x2);
            };
        default :
            throw $CreateException(new System.NotImplementedException.ctor(), new Error());
    }
};
SharpAlg.Native.ExpressionEvaluator.GetBinaryOperationEx = function (operation)
{
    switch (operation)
    {
        case 0:
            return 0;
        case 1:
            return 2;
        default :
            throw $CreateException(new System.NotImplementedException.ctor(), new Error());
    }
};
SharpAlg.Native.ExpressionEvaluator.IsInvertedOperation = function (operation)
{
    switch (operation)
    {
        case 1:
        case 3:
            return true;
    }
    return false;
};
SharpAlg.Native.ExpressionEvaluator.prototype.Function = function (functionExpr)
{
    throw $CreateException(new System.NotImplementedException.ctor(), new Error());
};
var SharpAlg$Native$ExpressionEvaluationException =
{
    fullname: "SharpAlg.Native.ExpressionEvaluationException",
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
JsTypes.push(SharpAlg$Native$ExpressionEvaluationException);
var SharpAlg$Native$ExpressionExtensions =
{
    fullname: "SharpAlg.Native.ExpressionExtensions",
    baseTypeName: "System.Object",
    staticDefinition:
    {
        Evaluate: function (expr, context)
        {
            return expr.Visit$1(SharpAlg.Native.Number.ctor, new SharpAlg.Native.ExpressionEvaluator((context != null ? context : new SharpAlg.Native.Context.ctor())));
        },
        Diff: function (expr, parameterName)
        {
            return expr.Visit$1(SharpAlg.Native.Expr.ctor, new SharpAlg.Native.DiffExpressionVisitor(SharpAlg.Native.Builder.ConvolutionExprBuilder.Instance, parameterName));
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
        Parse: function (expression)
        {
            return SharpAlg.Native.ExpressionExtensions.GetExpression(SharpAlg.Native.ExpressionExtensions.ParseCore(expression, SharpAlg.Native.Builder.ConvolutionExprBuilder.Instance));
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
        },
        Tail$$MultiplyExpr: function (multi)
        {
            return SharpAlg.Native.Expr.Multiply$$IEnumerable$1$Expr(SharpAlg.Native.FunctionalExtensions.Tail$1(SharpAlg.Native.Expr.ctor, multi.get_Args()));
        },
        Tail$$AddExpr: function (multi)
        {
            return SharpAlg.Native.Expr.Add$$IEnumerable$1$Expr(SharpAlg.Native.FunctionalExtensions.Tail$1(SharpAlg.Native.Expr.ctor, multi.get_Args()));
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
SharpAlg.Native.PlatformHelper = function ()
{
};
SharpAlg.Native.PlatformHelper.ToString = function (d)
{
    return d.toString();
};
SharpAlg.Native.PlatformHelper.Parse = function (s)
{
    return System.Double.Parse$$String(s);
};
SharpAlg.Native.PlatformHelper.IntToChar = function (n)
{
    return String.fromCharCode(n);
};
var SharpAlg$Native$FunctionalExtensions =
{
    fullname: "SharpAlg.Native.FunctionalExtensions",
    baseTypeName: "System.Object",
    staticDefinition:
    {
        cctor: function ()
        {
            SharpAlg.Native.FunctionalExtensions.STR_InputSequencesHaveDifferentLength = "Input sequences have different length.";
        },
        Map$2: function (TIn1, TIn2, action, input1, input2)
        {
            var enumerator1 = input1.GetEnumerator();
            var enumerator2 = input2.GetEnumerator();
            while (enumerator1.MoveNext())
            {
                if (!enumerator2.MoveNext())
                    throw $CreateException(new System.ArgumentException.ctor$$String("Input sequences have different length."), new Error());
                action(enumerator1.get_Current(), enumerator2.get_Current());
            }
            if (enumerator2.MoveNext())
                throw $CreateException(new System.ArgumentException.ctor$$String("Input sequences have different length."), new Error());
        },
        EnumerableEqual$1: function (T, first, second, comparer)
        {
            var en1 = first.GetEnumerator();
            var en2 = second.GetEnumerator();
            while (en1.MoveNext())
            {
                if (!en2.MoveNext())
                    return false;
                if (!comparer(en1.get_Current(), en2.get_Current()))
                    return false;
            }
            return !en2.MoveNext();
        },
        SetEqual$1: function (T, first, second, comparer)
        {
            var list = System.Linq.Enumerable.ToList$1(T, second);
            var $it1 = first.GetEnumerator();
            while ($it1.MoveNext())
            {
                var item = $it1.get_Current();
                var found = false;
                var $it2 = list.GetEnumerator();
                while ($it2.MoveNext())
                {
                    var item2 = $it2.get_Current();
                    if (comparer(item, item2))
                    {
                        list.Remove(item2);
                        found = true;
                        break;
                    }
                }
                if (found == false)
                    return false;
            }
            return list.get_Count() == 0;
        },
        RemoveAt$1: function (T, source, index)
        {
            var $yield = [];
            var en = source.GetEnumerator();
            while (en.MoveNext())
            {
                if (index != 0)
                    $yield.push(en.get_Current());
                index--;
            }
            if (index > 0)
                throw $CreateException(new System.IndexOutOfRangeException.ctor$$String("index"), new Error());
            return $yield;
        },
        Accumulate$1: function (T, source, init, next)
        {
            var enumerator = source.GetEnumerator();
            if (enumerator.MoveNext())
                init(enumerator.get_Current());
            else
                throw $CreateException(new System.InvalidOperationException.ctor(), new Error());
            while (enumerator.MoveNext())
            {
                next(enumerator.get_Current());
            }
        },
        ForEach$1: function (T, source, action)
        {
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
            {
                action(enumerator.get_Current());
            }
        },
        Tail$1: function (T, source)
        {
            return System.Linq.Enumerable.Skip$1(T, source, 1);
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
JsTypes.push(SharpAlg$Native$FunctionalExtensions);
var SharpAlg$Native$IExpressionVisitor$1 = {fullname: "SharpAlg.Native.IExpressionVisitor$1", baseTypeName: "System.Object", assemblyName: "SharpAlg", Kind: "Interface"};
JsTypes.push(SharpAlg$Native$IExpressionVisitor$1);
SharpAlg.Native.MayBe = function ()
{
};
SharpAlg.Native.MayBe.With = function (input, evaluator)
{
    if (input == null)
        return null;
    return evaluator(input);
};
SharpAlg.Native.MayBe.Return = function (input, evaluator, fallback)
{
    if (!input.get_HasValue())
        return fallback != null ? fallback() : Default(TR);
    return evaluator(input.get_Value());
};
SharpAlg.Native.MayBe.Return = function (input, evaluator, fallback)
{
    if (input == null)
        return fallback != null ? fallback() : Default(TR);
    return evaluator(input);
};
SharpAlg.Native.MayBe.ReturnSuccess = function (input)
{
    return input != null;
};
SharpAlg.Native.MayBe.If = function (input, evaluator)
{
    if (input == null)
        return null;
    return evaluator(input) ? input : null;
};
SharpAlg.Native.MayBe.Do = function (input, action)
{
    if (input == null)
        return null;
    action(input);
    return input;
};
