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
    assemblyName: "SharpAlg.Implementation",
    interfaceNames: ["SharpAlg.Native.IContext"],
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            this.names = new System.Collections.Generic.Dictionary$2.ctor(System.String.ctor, SharpAlg.Native.Expr.ctor);
            this.functions = new System.Collections.Generic.Dictionary$2.ctor(System.String.ctor, SharpAlg.Native.Function.ctor);
            this._ReadOnly = false;
            System.Object.ctor.call(this);
        },
        ReadOnly$$: "System.Boolean",
        get_ReadOnly: function ()
        {
            return this._ReadOnly;
        },
        set_ReadOnly: function (value)
        {
            this._ReadOnly = value;
        },
        Register$$Function: function (func)
        {
            this.CheckReadonly();
            this.functions.set_Item$$TKey(func.get_Name(), func);
            return this;
        },
        GetFunction: function (name)
        {
            return SharpAlg.Native.FunctionalExtensions.TryGetValue$2(System.String.ctor, SharpAlg.Native.Function.ctor, this.functions, name);
        },
        Register$$String$$Expr: function (name, value)
        {
            this.CheckReadonly();
            this.names.set_Item$$TKey(name, value);
            return this;
        },
        GetValue: function (name)
        {
            return SharpAlg.Native.FunctionalExtensions.TryGetValue$2(System.String.ctor, SharpAlg.Native.Expr.ctor, this.names, name);
        },
        CheckReadonly: function ()
        {
            if (this.get_ReadOnly())
                throw $CreateException(new System.InvalidOperationException.ctor(), new Error());
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
    return this.GetDefault(functionExpr);
};
SharpAlg.Native.DiffExpressionVisitor = function (builder, context, parameterName)
{
    this.context = null;
    this.parameterName = null;
    this.autoParameterName = false;
    this.builder = null;
    this.builder = builder;
    this.context = context;
    this.parameterName = parameterName;
    this.autoParameterName = !this.get_HasParameter();
};
SharpAlg.Native.DiffExpressionVisitor.prototype.get_HasParameter = function ()
{
    return !System.String.IsNullOrEmpty(this.parameterName);
};
SharpAlg.Native.DiffExpressionVisitor.prototype.get_Builder = function ()
{
    return this.builder;
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
        result = this.get_Builder().Add(result, x.Visit$1(SharpAlg.Native.Expr.ctor, this));
    }));
    return result;
};
SharpAlg.Native.DiffExpressionVisitor.prototype.Multiply = function (multi)
{
    var tail = SharpAlg.Native.CoreExpressionExtensions.Tail$$MultiplyExpr(multi);
    var expr1 = this.get_Builder().Multiply(System.Linq.Enumerable.First$1$$IEnumerable$1(SharpAlg.Native.Expr.ctor, multi.get_Args()).Visit$1(SharpAlg.Native.Expr.ctor, this), tail);
    var expr2 = this.get_Builder().Multiply(System.Linq.Enumerable.First$1$$IEnumerable$1(SharpAlg.Native.Expr.ctor, multi.get_Args()), tail.Visit$1(SharpAlg.Native.Expr.ctor, this));
    return this.get_Builder().Add(expr1, expr2);
};
SharpAlg.Native.DiffExpressionVisitor.prototype.Power = function (power)
{
    var sum1 = this.get_Builder().Multiply(power.get_Right().Visit$1(SharpAlg.Native.Expr.ctor, this), SharpAlg.Native.FunctionFactory.Ln(power.get_Left()));
    var sum2 = this.get_Builder().Divide(this.get_Builder().Multiply(power.get_Right(), power.get_Left().Visit$1(SharpAlg.Native.Expr.ctor, this)), power.get_Left());
    var sum = this.get_Builder().Add(sum1, sum2);
    return this.get_Builder().Multiply(power, sum);
};
SharpAlg.Native.DiffExpressionVisitor.prototype.Function = function (functionExpr)
{
    return SharpAlg.Native.MayBe.Return(SharpAlg.Native.FunctionalExtensions.Convert$1(SharpAlg.Native.ISupportDiff.ctor, this.context.GetFunction(functionExpr.get_FunctionName())), $CreateAnonymousDelegate(this, function (x)
    {
        return x.Diff(this, functionExpr.get_Args());
    }), $CreateAnonymousDelegate(this, function ()
    {
        throw $CreateException(new System.InvalidOperationException.ctor(), new Error());
    }));
};
var SharpAlg$Native$ExpressionDefferentiationException =
{
    fullname: "SharpAlg.Native.ExpressionDefferentiationException",
    baseTypeName: "System.Exception",
    assemblyName: "SharpAlg.Implementation",
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
var SharpAlg$Native$ExpressionEqualityComparer =
{
    fullname: "SharpAlg.Native.ExpressionEqualityComparer",
    baseTypeName: "System.Object",
    assemblyName: "SharpAlg.Implementation",
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
                return x1.get_FunctionName() == x2.get_FunctionName() && SharpAlg.Native.FunctionalExtensions.EnumerableEqual$1(SharpAlg.Native.Expr.ctor, x1.get_Args(), x2.get_Args(), $CreateDelegate(this, this.EqualsCore));
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
    assemblyName: "SharpAlg.Implementation",
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
    var func = this.context.GetFunction(functionExpr.get_FunctionName());
    if (func != null)
    {
        return func.Evaluate(this, functionExpr.get_Args());
    }
    throw $CreateException(new System.NotImplementedException.ctor(), new Error());
};
var SharpAlg$Native$ExpressionEvaluationException =
{
    fullname: "SharpAlg.Native.ExpressionEvaluationException",
    baseTypeName: "System.Exception",
    assemblyName: "SharpAlg.Implementation",
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
var SharpAlg$Native$InvalidArgumentCountException =
{
    fullname: "SharpAlg.Native.InvalidArgumentCountException",
    baseTypeName: "System.Exception",
    assemblyName: "SharpAlg.Implementation",
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
    assemblyName: "SharpAlg.Implementation",
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
    assemblyName: "SharpAlg.Implementation",
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
    assemblyName: "SharpAlg.Implementation",
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
    assemblyName: "SharpAlg.Implementation",
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
            if (SharpAlg.Native.ImplementationExpressionExtensions.ExprEquals(System.Linq.Enumerable.First$1$$IEnumerable$1(SharpAlg.Native.Expr.ctor, args), SharpAlg.Native.Expr.One))
                return SharpAlg.Native.Expr.Zero;
            return null;
        }
    }
};
JsTypes.push(SharpAlg$Native$LnFunction);
var SharpAlg$Native$FunctionFactory =
{
    fullname: "SharpAlg.Native.FunctionFactory",
    baseTypeName: "System.Object",
    staticDefinition:
    {
        cctor: function ()
        {
            SharpAlg.Native.FunctionFactory.FactorialName = "factorial";
            SharpAlg.Native.FunctionFactory.LnName = "ln";
        },
        Factorial: function (argument)
        {
            return SharpAlg.Native.Expr.Function$$String$$Expr("factorial", argument);
        },
        Ln: function (argument)
        {
            return SharpAlg.Native.Expr.Function$$String$$Expr("ln", argument);
        }
    },
    assemblyName: "SharpAlg.Implementation",
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            System.Object.ctor.call(this);
        }
    }
};
JsTypes.push(SharpAlg$Native$FunctionFactory);
var SharpAlg$Native$ImplementationExpressionExtensions =
{
    fullname: "SharpAlg.Native.ImplementationExpressionExtensions",
    baseTypeName: "System.Object",
    staticDefinition:
    {
        ExprEquals: function (expr1, expr2)
        {
            return expr1.Visit$1(System.Boolean.ctor, new SharpAlg.Native.ExpressionEqualityComparer.ctor(expr2));
        },
        ExprEquivalent: function (expr1, expr2)
        {
            return expr1.Visit$1(System.Boolean.ctor, new SharpAlg.Native.ExpressionEquivalenceComparer.ctor(expr2));
        }
    },
    assemblyName: "SharpAlg.Implementation",
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            System.Object.ctor.call(this);
        }
    }
};
JsTypes.push(SharpAlg$Native$ImplementationExpressionExtensions);
