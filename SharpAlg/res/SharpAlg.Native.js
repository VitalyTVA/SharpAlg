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
SharpAlg.Native.DiffExpressionVisitor = function (builder)
{
    this.builder = null;
    this.builder = builder;
};
SharpAlg.Native.DiffExpressionVisitor.prototype.Constant = function (constant)
{
    return SharpAlg.Native.Expr.Zero;
};
SharpAlg.Native.DiffExpressionVisitor.prototype.Parameter = function (parameter)
{
    return SharpAlg.Native.Expr.One;
};
SharpAlg.Native.DiffExpressionVisitor.prototype.Multi = function (multi)
{
    switch (multi.get_Operation())
    {
        case 0:
            return this.VisitAdditive(multi);
        case 1:
            return this.VisitMultiply(multi);
        default :
            throw $CreateException(new System.NotImplementedException.ctor(), new Error());
    }
};
SharpAlg.Native.DiffExpressionVisitor.prototype.Power = function (power)
{
    if (!(Is(power.get_Right(), SharpAlg.Native.ConstantExpr.ctor)))
        throw $CreateException(new System.NotImplementedException.ctor(), new Error());
    return SharpAlg.Native.Expr.Multiply(power.get_Right(), this.builder.Multiply(power.get_Left().Visit$1(SharpAlg.Native.Expr.ctor, this), this.builder.Power(power.get_Left(), this.builder.Subtract(power.get_Right(), SharpAlg.Native.Expr.One))));
};
SharpAlg.Native.DiffExpressionVisitor.prototype.Unary = function (unary)
{
    switch (unary.get_Operation())
    {
        case 0:
            return SharpAlg.Native.Expr.Unary(unary.get_Expr().Visit$1(SharpAlg.Native.Expr.ctor, this), unary.get_Operation());
        case 1:
            return SharpAlg.Native.Expr.Divide(this.builder.Unary(unary.get_Expr().Visit$1(SharpAlg.Native.Expr.ctor, this), 0), this.builder.Multiply(unary.get_Expr(), unary.get_Expr()));
        default :
            throw $CreateException(new System.NotImplementedException.ctor(), new Error());
    }
};
SharpAlg.Native.DiffExpressionVisitor.prototype.VisitAdditive = function (multi)
{
    var result = null;
    SharpAlg.Native.ExpressionExtensions.Accumulate(multi, $CreateAnonymousDelegate(this, function (x)
    {
        result = x.Visit$1(SharpAlg.Native.Expr.ctor, this);
    }), $CreateAnonymousDelegate(this, function (x)
    {
        result = this.builder.Add(result, x.Visit$1(SharpAlg.Native.Expr.ctor, this));
    }));
    return result;
};
SharpAlg.Native.DiffExpressionVisitor.prototype.VisitMultiply = function (expr)
{
    var tail = SharpAlg.Native.ExpressionExtensions.Tail(expr);
    var expr1 = this.builder.Multiply(System.Linq.Enumerable.First$1$$IEnumerable$1(SharpAlg.Native.Expr.ctor, expr.get_Args()).Visit$1(SharpAlg.Native.Expr.ctor, this), tail);
    var expr2 = this.builder.Multiply(System.Linq.Enumerable.First$1$$IEnumerable$1(SharpAlg.Native.Expr.ctor, expr.get_Args()), tail.Visit$1(SharpAlg.Native.Expr.ctor, this));
    return this.builder.Add(expr1, expr2);
};
var SharpAlg$Native$Expr =
{
    fullname: "SharpAlg.Native.Expr",
    baseTypeName: "System.Object",
    staticDefinition:
    {
        cctor: function ()
        {
            SharpAlg.Native.Expr.Zero = new SharpAlg.Native.ConstantExpr.ctor(0);
            SharpAlg.Native.Expr.One = new SharpAlg.Native.ConstantExpr.ctor(1);
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
            return new SharpAlg.Native.MultiExpr.ctor(args, type);
        },
        Add: function (left, right)
        {
            return SharpAlg.Native.Expr.Binary(left, right, 0);
        },
        Subtract: function (left, right)
        {
            return SharpAlg.Native.Expr.Add(left, SharpAlg.Native.Expr.Minus(right));
        },
        Multiply: function (left, right)
        {
            return SharpAlg.Native.Expr.Binary(left, right, 1);
        },
        Divide: function (left, right)
        {
            return SharpAlg.Native.Expr.Multiply(left, SharpAlg.Native.Expr.Inverse(right));
        },
        Power: function (left, right)
        {
            return new SharpAlg.Native.PowerExpr.ctor(left, right);
        },
        Unary: function (expr, operation)
        {
            return new SharpAlg.Native.UnaryExpr.ctor(expr, operation);
        },
        Minus: function (expr)
        {
            return SharpAlg.Native.Expr.Unary(expr, 0);
        },
        Inverse: function (expr)
        {
            return SharpAlg.Native.Expr.Unary(expr, 1);
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
            this._Value = 0;
            SharpAlg.Native.Expr.ctor.call(this);
            this.set_Value(value);
        },
        Value$$: "System.Double",
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
        ctor: function (args, operation)
        {
            this._Operation = 0;
            this._Args = null;
            SharpAlg.Native.Expr.ctor.call(this);
            this.set_Args(args);
            this.set_Operation(operation);
        },
        Operation$$: "SharpAlg.Native.BinaryOperation",
        get_Operation: function ()
        {
            return this._Operation;
        },
        set_Operation: function (value)
        {
            this._Operation = value;
        },
        Args$$: "System.Collections.Generic.IEnumerable`1[[SharpAlg.Native.Expr]]",
        get_Args: function ()
        {
            return this._Args;
        },
        set_Args: function (value)
        {
            this._Args = value;
        },
        Visit$1: function (T, visitor)
        {
            return visitor.Multi(this);
        }
    }
};
JsTypes.push(SharpAlg$Native$MultiExpr);
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
var SharpAlg$Native$UnaryExpr =
{
    fullname: "SharpAlg.Native.UnaryExpr",
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
        ctor: function (expr, operation)
        {
            this._Expr = null;
            this._Operation = 0;
            SharpAlg.Native.Expr.ctor.call(this);
            this.set_Expr(expr);
            this.set_Operation(operation);
        },
        Expr$$: "SharpAlg.Native.Expr",
        get_Expr: function ()
        {
            return this._Expr;
        },
        set_Expr: function (value)
        {
            this._Expr = value;
        },
        Operation$$: "SharpAlg.Native.UnaryOperation",
        get_Operation: function ()
        {
            return this._Operation;
        },
        set_Operation: function (value)
        {
            this._Operation = value;
        },
        Visit$1: function (T, visitor)
        {
            return visitor.Unary(this);
        }
    }
};
JsTypes.push(SharpAlg$Native$UnaryExpr);
SharpAlg.Native.ExprBuilder = function ()
{
};
SharpAlg.Native.ExprBuilder.prototype.Add = function (left, right)
{
    return this.Binary(left, right, 0);
};
SharpAlg.Native.ExprBuilder.prototype.Subtract = function (left, right)
{
    return this.Add(left, this.Minus(right));
};
SharpAlg.Native.ExprBuilder.prototype.Multiply = function (left, right)
{
    return this.Binary(left, right, 1);
};
SharpAlg.Native.ExprBuilder.prototype.Divide = function (left, right)
{
    return this.Multiply(left, this.Inverse(right));
};
SharpAlg.Native.ExprBuilder.prototype.Minus = function (expr)
{
    return this.Unary(expr, 0);
};
SharpAlg.Native.ExprBuilder.prototype.Inverse = function (expr)
{
    return this.Unary(expr, 1);
};
SharpAlg.Native.TrivialExprBuilder = function ()
{
    SharpAlg.Native.ExprBuilder.call(this);
};
SharpAlg.Native.TrivialExprBuilder.prototype.Binary = function (left, right, operation)
{
    return SharpAlg.Native.Expr.Binary(left, right, operation);
};
SharpAlg.Native.TrivialExprBuilder.prototype.Unary = function (expr, operation)
{
    return new SharpAlg.Native.UnaryExpr.ctor(expr, operation);
};
SharpAlg.Native.TrivialExprBuilder.prototype.Power = function (left, right)
{
    return SharpAlg.Native.Expr.Power(left, right);
};
$Inherit(SharpAlg.Native.TrivialExprBuilder, SharpAlg.Native.ExprBuilder);
SharpAlg.Native.ConvolutionExprBuilder = function ()
{
    SharpAlg.Native.ExprBuilder.call(this);
};
SharpAlg.Native.ConvolutionExprBuilder.prototype.Unary = function (expr, operation)
{
    var defaultResult = SharpAlg.Native.Expr.Unary(expr, operation);
    var constant = SharpAlg.Native.ConvolutionExprBuilder.GetConstValue(defaultResult);
    if (constant != null)
    {
        return SharpAlg.Native.Expr.Constant(constant.get_Value());
    }
    return defaultResult;
};
SharpAlg.Native.ConvolutionExprBuilder.prototype.Binary = function (left, right, operation)
{
    return (this.ConstantConvolution(left, right, operation) != null ? this.ConstantConvolution(left, right, operation) : (this.EqualityConvolution(left, right, operation) != null ? this.EqualityConvolution(left, right, operation) : (this.MultiConvolution(left, right, operation) != null ? this.MultiConvolution(left, right, operation) : SharpAlg.Native.Expr.Binary(left, right, operation))));
};
SharpAlg.Native.ConvolutionExprBuilder.prototype.Power = function (left, right)
{
    return (this.ConstantPowerConvolution(left, right) != null ? this.ConstantPowerConvolution(left, right) : SharpAlg.Native.Expr.Power(left, right));
};
SharpAlg.Native.ConvolutionExprBuilder.prototype.MultiConvolution = function (left, right, operation)
{
    var args = System.Linq.Enumerable.ToList$1(SharpAlg.Native.Expr.ctor, System.Linq.Enumerable.Concat$1(SharpAlg.Native.Expr.ctor, SharpAlg.Native.ConvolutionExprBuilder.GetArgs(left, operation), SharpAlg.Native.ConvolutionExprBuilder.GetArgs(right, operation)));
    for (var i = 0; i < args.get_Count(); i++)
    {
        for (var j = i + 1; j < args.get_Count(); j++)
        {
            var convoluted = (this.ConstantConvolution(args.get_Item$$Int32(i), args.get_Item$$Int32(j), operation) != null ? this.ConstantConvolution(args.get_Item$$Int32(i), args.get_Item$$Int32(j), operation) : this.EqualityConvolution(args.get_Item$$Int32(i), args.get_Item$$Int32(j), operation));
            if (convoluted != null)
            {
                args.set_Item$$Int32(i, convoluted);
                args.RemoveAt(j);
            }
        }
    }
    return args.get_Count() > 1 ? SharpAlg.Native.Expr.Multi(args, operation) : args.get_Item$$Int32(0);
};
SharpAlg.Native.ConvolutionExprBuilder.GetArgs = function (expr, operation)
{
    if (Is(expr, SharpAlg.Native.MultiExpr.ctor) && (Cast(expr, SharpAlg.Native.MultiExpr.ctor)).get_Operation() == operation)
        return (Cast(expr, SharpAlg.Native.MultiExpr.ctor)).get_Args();
    return [expr];
};
SharpAlg.Native.ConvolutionExprBuilder.prototype.ConstantConvolution = function (left, right, operation)
{
    var leftConst = SharpAlg.Native.ConvolutionExprBuilder.GetConstValue(left);
    if (leftConst == 0)
    {
        if (operation == 0)
            return right;
        if (operation == 1)
            return SharpAlg.Native.Expr.Zero;
    }
    if (leftConst == 1)
    {
        if (operation == 1)
            return right;
    }
    var rightConst = SharpAlg.Native.ConvolutionExprBuilder.GetConstValue(right);
    if (rightConst == 0)
    {
        if (operation == 0)
            return left;
        if (operation == 1)
            return SharpAlg.Native.Expr.Zero;
    }
    if (rightConst == 1)
    {
        if (operation == 1)
            return left;
    }
    if (rightConst != null && leftConst != null)
        return SharpAlg.Native.Expr.Constant(SharpAlg.Native.ExpressionEvaluator.GetBinaryOperationEvaluator(operation)(leftConst.get_Value(), rightConst.get_Value()));
    return null;
};
SharpAlg.Native.ConvolutionExprBuilder.prototype.ConstantPowerConvolution = function (left, right)
{
    var leftConst = SharpAlg.Native.ConvolutionExprBuilder.GetConstValue(left);
    if (leftConst == 0)
    {
        return SharpAlg.Native.Expr.Zero;
    }
    if (leftConst == 1)
    {
        return SharpAlg.Native.Expr.One;
    }
    var rightConst = SharpAlg.Native.ConvolutionExprBuilder.GetConstValue(right);
    if (rightConst == 0)
    {
        return SharpAlg.Native.Expr.One;
    }
    if (rightConst == 1)
    {
        return left;
    }
    if (rightConst != null && leftConst != null)
        return SharpAlg.Native.Expr.Constant(System.Math.Pow(leftConst.get_Value(), rightConst.get_Value()));
    return null;
};
SharpAlg.Native.ConvolutionExprBuilder.prototype.EqualityConvolution = function (left, right, operation)
{
    var leftInfo = SharpAlg.Native.UnaryExpressionExtractor.ExtractUnaryInfo(left, operation);
    var rightInfo = SharpAlg.Native.UnaryExpressionExtractor.ExtractUnaryInfo(right, operation);
    if (SharpAlg.Native.ExpressionExtensions.ExprEquals(rightInfo.Expr, leftInfo.Expr))
    {
        var coeff = (SharpAlg.Native.ExpressionEvaluator.IsInvertedOperation(leftInfo.Operation) ? -1 : 1) + (SharpAlg.Native.ExpressionEvaluator.IsInvertedOperation(rightInfo.Operation) ? -1 : 1);
        if (operation == 0)
            return this.Multiply(SharpAlg.Native.Expr.Constant(coeff), rightInfo.Expr);
        if (operation == 1)
            return this.Power(left, SharpAlg.Native.Expr.Constant(coeff));
    }
    return null;
};
SharpAlg.Native.ConvolutionExprBuilder.GetConstValue = function (expr)
{
    var unary = As(expr, SharpAlg.Native.UnaryExpr.ctor);
    if ((unary != null && Is(unary.get_Expr(), SharpAlg.Native.ConstantExpr.ctor)) || Is(expr, SharpAlg.Native.ConstantExpr.ctor))
    {
        return SharpAlg.Native.ExpressionExtensions.Evaluate(expr, new SharpAlg.Native.Context.ctor());
    }
    return null;
};
$Inherit(SharpAlg.Native.ConvolutionExprBuilder, SharpAlg.Native.ExprBuilder);
var SharpAlg$Native$ExpressionComparer =
{
    fullname: "SharpAlg.Native.ExpressionComparer",
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
                return x1.get_Value() == x2.get_Value();
            }));
        },
        Multi: function (multi)
        {
            return this.DoEqualityCheck$1(SharpAlg.Native.MultiExpr.ctor, multi, $CreateAnonymousDelegate(this, function (x1, x2)
            {
                return x1.get_Operation() == x2.get_Operation() && SharpAlg.Native.FunctionalExtensions.EnumerableEqual(x1.get_Args(), x2.get_Args(), $CreateAnonymousDelegate(this, function (x, y)
                {
                    return SharpAlg.Native.ExpressionExtensions.ExprEquals(x, y);
                }));
            }));
        },
        Power: function (power)
        {
            return this.DoEqualityCheck$1(SharpAlg.Native.PowerExpr.ctor, power, $CreateAnonymousDelegate(this, function (x1, x2)
            {
                return SharpAlg.Native.ExpressionExtensions.ExprEquals(x1.get_Left(), x2.get_Left()) && SharpAlg.Native.ExpressionExtensions.ExprEquals(x1.get_Right(), x2.get_Right());
            }));
        },
        Unary: function (unary)
        {
            return this.DoEqualityCheck$1(SharpAlg.Native.UnaryExpr.ctor, unary, $CreateAnonymousDelegate(this, function (x1, x2)
            {
                return SharpAlg.Native.ExpressionExtensions.ExprEquals(x1.get_Expr(), x2.get_Expr()) && x1.get_Operation() == x2.get_Operation();
            }));
        },
        Parameter: function (parameter)
        {
            return this.DoEqualityCheck$1(SharpAlg.Native.ParameterExpr.ctor, parameter, $CreateAnonymousDelegate(this, function (x1, x2)
            {
                return x1.get_ParameterName() == x2.get_ParameterName();
            }));
        },
        DoEqualityCheck$1: function (T, expr2, equalityCheck)
        {
            var other = As(this.expr, T);
            return other != null && equalityCheck(other, expr2);
        }
    }
};
JsTypes.push(SharpAlg$Native$ExpressionComparer);
SharpAlg.Native.ExpressionEvaluator = function (context)
{
    this.context = null;
    this.context = context;
};
SharpAlg.Native.ExpressionEvaluator.prototype.Constant = function (constant)
{
    return constant.get_Value();
};
SharpAlg.Native.ExpressionEvaluator.prototype.Multi = function (multi)
{
    var result = 0;
    SharpAlg.Native.ExpressionExtensions.Accumulate(multi, $CreateAnonymousDelegate(this, function (x)
    {
        result = x.Visit$1(System.Double.ctor, this);
    }), $CreateAnonymousDelegate(this, function (x)
    {
        result = SharpAlg.Native.ExpressionEvaluator.GetBinaryOperationEvaluator(multi.get_Operation())(result, x.Visit$1(System.Double.ctor, this));
    }));
    return result;
};
SharpAlg.Native.ExpressionEvaluator.prototype.Power = function (power)
{
    return System.Math.Pow(power.get_Left().Visit$1(System.Double.ctor, this), power.get_Right().Visit$1(System.Double.ctor, this));
};
SharpAlg.Native.ExpressionEvaluator.prototype.Unary = function (unary)
{
    switch (unary.get_Operation())
    {
        case 0:
            return -unary.get_Expr().Visit$1(System.Double.ctor, this);
        case 1:
            return 1 / unary.get_Expr().Visit$1(System.Double.ctor, this);
        default :
            throw $CreateException(new System.NotImplementedException.ctor(), new Error());
    }
};
SharpAlg.Native.ExpressionEvaluator.prototype.Parameter = function (parameter)
{
    var parameterValue = this.context.GetValue(parameter.get_ParameterName());
    if (parameterValue == null)
        throw $CreateException(new SharpAlg.Native.ExpressionEvaluationException.ctor$$String(System.String.Format$$String$$Object("{0} value is undefined", parameter.get_ParameterName())), new Error());
    return parameterValue.Visit$1(System.Double.ctor, this);
};
SharpAlg.Native.ExpressionEvaluator.GetBinaryOperationEvaluator = function (operation)
{
    return SharpAlg.Native.ExpressionEvaluator.GetBinaryOperationEvaluatorEx(SharpAlg.Native.ExpressionEvaluator.GetBinaryOperationEx(operation));
};
SharpAlg.Native.ExpressionEvaluator.GetBinaryOperationEvaluatorEx = function (operation)
{
    switch (operation)
    {
        case 0:
            return function (x1, x2)
            {
                return x1 + x2;
            };
        case 1:
            return function (x1, x2)
            {
                return x1 - x2;
            };
        case 2:
            return function (x1, x2)
            {
                return x1 * x2;
            };
        case 3:
            return function (x1, x2)
            {
                return x1 / x2;
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
            return expr.Visit$1(System.Double.ctor, new SharpAlg.Native.ExpressionEvaluator((context != null ? context : new SharpAlg.Native.Context.ctor())));
        },
        Diff: function (expr)
        {
            return expr.Visit$1(SharpAlg.Native.Expr.ctor, new SharpAlg.Native.DiffExpressionVisitor(new SharpAlg.Native.ConvolutionExprBuilder()));
        },
        ExprEquals: function (expr1, expr2)
        {
            return expr1.Visit$1(System.Boolean.ctor, new SharpAlg.Native.ExpressionComparer.ctor(expr2));
        },
        Print: function (expr)
        {
            return expr.Visit$1(System.String.ctor, new SharpAlg.Native.ExpressionPrinter(0));
        },
        Parse: function (expression)
        {
            var parser = SharpAlg.Native.ExpressionExtensions.ParseCore(expression, new SharpAlg.Native.ConvolutionExprBuilder());
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
        Accumulate: function (multi, init, next)
        {
            var enumerator = multi.get_Args().GetEnumerator();
            if (enumerator.MoveNext())
                init(enumerator.get_Current());
            else
                throw $CreateException(new System.InvalidOperationException.ctor(), new Error());
            while (enumerator.MoveNext())
            {
                next(enumerator.get_Current());
            }
        },
        Tail: function (multi)
        {
            var count = System.Linq.Enumerable.Count$1$$IEnumerable$1(SharpAlg.Native.Expr.ctor, multi.get_Args());
            if (count > 2)
                return SharpAlg.Native.Expr.Multi(SharpAlg.Native.FunctionalExtensions.RemoveAt(multi.get_Args(), 0), multi.get_Operation());
            if (count == 2)
                return System.Linq.Enumerable.ElementAt$1(SharpAlg.Native.Expr.ctor, multi.get_Args(), 1);
            throw $CreateException(new System.InvalidOperationException.ctor(), new Error());
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
SharpAlg.Native.ExpressionPrinter = function (priority)
{
    this.priority = 0;
    this.priority = priority;
};
SharpAlg.Native.ExpressionPrinter.prototype.Constant = function (constant)
{
    var stringValue = constant.get_Value().toString();
    return constant.get_Value() >= 0 ? stringValue : this.Wrap(stringValue, 1);
};
SharpAlg.Native.ExpressionPrinter.prototype.Multi = function (multi)
{
    var sb = new System.Text.StringBuilder.ctor();
    var nextPrinter = new SharpAlg.Native.ExpressionPrinter(SharpAlg.Native.ExpressionPrinter.GetPriority(multi.get_Operation()));
    SharpAlg.Native.ExpressionExtensions.Accumulate(multi, $CreateAnonymousDelegate(this, function (x)
    {
        sb.Append$$String(x.Visit$1(System.String.ctor, nextPrinter));
    }), $CreateAnonymousDelegate(this, function (x)
    {
        var info = SharpAlg.Native.UnaryExpressionExtractor.ExtractUnaryInfo(x, multi.get_Operation());
        sb.Append$$String(" ");
        sb.Append$$String(SharpAlg.Native.ExpressionPrinter.GetBinaryOperationSymbol(info.Operation));
        sb.Append$$String(" ");
        sb.Append$$String(info.Expr.Visit$1(System.String.ctor, nextPrinter));
    }));
    return this.Wrap(sb.toString(), SharpAlg.Native.ExpressionPrinter.GetPriority(multi.get_Operation()));
};
SharpAlg.Native.ExpressionPrinter.prototype.Power = function (power)
{
    var nextPrinter = new SharpAlg.Native.ExpressionPrinter(3);
    return this.Wrap(System.String.Format$$String$$Object$$Object("{0} ^ {1}", power.get_Left().Visit$1(System.String.ctor, nextPrinter), power.get_Right().Visit$1(System.String.ctor, nextPrinter)), 3);
};
SharpAlg.Native.ExpressionPrinter.prototype.Unary = function (unary)
{
    var newPriority = SharpAlg.Native.ExpressionPrinter.GetPriority(unary.get_Operation());
    return this.Wrap(System.String.Format$$String$$Object$$Object("{0}{1}", SharpAlg.Native.ExpressionPrinter.GetUnaryOperationSymbol(unary.get_Operation()), unary.get_Expr().Visit$1(System.String.ctor, new SharpAlg.Native.ExpressionPrinter(newPriority))), newPriority);
};
SharpAlg.Native.ExpressionPrinter.prototype.Parameter = function (parameter)
{
    return parameter.get_ParameterName();
};
SharpAlg.Native.ExpressionPrinter.GetUnaryOperationSymbol = function (operation)
{
    switch (operation)
    {
        case 0:
            return "-";
        case 1:
            return "1 / ";
        default :
            throw $CreateException(new System.NotImplementedException.ctor(), new Error());
    }
};
SharpAlg.Native.ExpressionPrinter.GetBinaryOperationSymbol = function (operation)
{
    switch (operation)
    {
        case 0:
            return "+";
        case 1:
            return "-";
        case 2:
            return "*";
        case 3:
            return "/";
        default :
            throw $CreateException(new System.NotImplementedException.ctor(), new Error());
    }
};
SharpAlg.Native.ExpressionPrinter.GetPriority = function (operation)
{
    switch (operation)
    {
        case 0:
            return 1;
        case 1:
            return 2;
        default :
            throw $CreateException(new System.NotImplementedException.ctor(), new Error());
    }
};
SharpAlg.Native.ExpressionPrinter.GetPriority = function (operation)
{
    switch (operation)
    {
        case 0:
            return 1;
        case 1:
            return 2;
        default :
            throw $CreateException(new System.NotImplementedException.ctor(), new Error());
    }
};
SharpAlg.Native.ExpressionPrinter.prototype.Wrap = function (s, newPriority)
{
    if (newPriority <= this.priority)
        return "(" + s + ")";
    return s;
};
SharpAlg.Native.UnaryExpressionInfo = function (expr, operation)
{
    this.Expr = null;
    this.Operation = 0;
    this.Operation = operation;
    this.Expr = expr;
};
SharpAlg.Native.UnaryExpressionExtractor = function (operation)
{
    this.operation = 0;
    this.operation = operation;
};
SharpAlg.Native.UnaryExpressionExtractor.ExtractUnaryInfo = function (expr, operation)
{
    return expr.Visit$1(SharpAlg.Native.UnaryExpressionInfo, new SharpAlg.Native.UnaryExpressionExtractor(operation));
};
SharpAlg.Native.UnaryExpressionExtractor.prototype.Constant = function (constant)
{
    return constant.get_Value() >= 0 || this.operation != 0 ? this.GetDefaultInfo(constant) : new SharpAlg.Native.UnaryExpressionInfo(SharpAlg.Native.Expr.Constant(-constant.get_Value()), 1);
};
SharpAlg.Native.UnaryExpressionExtractor.prototype.Parameter = function (parameter)
{
    return this.GetDefaultInfo(parameter);
};
SharpAlg.Native.UnaryExpressionExtractor.prototype.Multi = function (multi)
{
    return this.GetDefaultInfo(multi);
};
SharpAlg.Native.UnaryExpressionExtractor.prototype.Power = function (power)
{
    return this.GetDefaultInfo(power);
};
SharpAlg.Native.UnaryExpressionExtractor.prototype.Unary = function (unary)
{
    if (this.operation == 0 && unary.get_Operation() == 0)
    {
        return new SharpAlg.Native.UnaryExpressionInfo(unary.get_Expr(), 1);
    }
    if (this.operation == 1 && unary.get_Operation() == 1)
    {
        return new SharpAlg.Native.UnaryExpressionInfo(unary.get_Expr(), 3);
    }
    return this.GetDefaultInfo(unary);
};
SharpAlg.Native.UnaryExpressionExtractor.prototype.GetDefaultInfo = function (expr)
{
    return new SharpAlg.Native.UnaryExpressionInfo(expr, SharpAlg.Native.ExpressionEvaluator.GetBinaryOperationEx(this.operation));
};
SharpAlg.Native.FunctionalExtensions = function ()
{
};
SharpAlg.Native.FunctionalExtensions.STR_InputSequencesHaveDifferentLength = "Input sequences have different length.";
SharpAlg.Native.FunctionalExtensions.Map = function (action, input1, input2)
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
};
SharpAlg.Native.FunctionalExtensions.EnumerableEqual = function (first, second, comparer)
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
};
SharpAlg.Native.FunctionalExtensions.RemoveAt = function (source, index)
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
};
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
