/*Generated by SharpKit 5 v5.01.1000*/
if (typeof(JsTypes) == "undefined")
    var JsTypes = [];
var SharpAlg$Native$CoreExpressionExtensions =
{
    fullname: "SharpAlg.Native.CoreExpressionExtensions",
    baseTypeName: "System.Object",
    staticDefinition:
    {
        Tail$$MultiplyExpr: function (multi)
        {
            return SharpAlg.Native.Expr.Multiply$$IEnumerable$1$Expr(SharpAlg.Native.FunctionalExtensions.Tail$1(SharpAlg.Native.Expr.ctor, multi.get_Args()));
        },
        Tail$$AddExpr: function (multi)
        {
            return SharpAlg.Native.Expr.Add$$IEnumerable$1$Expr(SharpAlg.Native.FunctionalExtensions.Tail$1(SharpAlg.Native.Expr.ctor, multi.get_Args()));
        }
    },
    assemblyName: "SharpAlg.Core",
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            System.Object.ctor.call(this);
        }
    }
};
JsTypes.push(SharpAlg$Native$CoreExpressionExtensions);
var SharpAlg$Native$Expr =
{
    fullname: "SharpAlg.Native.Expr",
    baseTypeName: "System.Object",
    staticDefinition:
    {
        cctor: function ()
        {
            SharpAlg.Native.Expr.Zero = new SharpAlg.Native.ConstantExpr.ctor(SharpAlg.Native.NumberFactory.Zero);
            SharpAlg.Native.Expr.One = new SharpAlg.Native.ConstantExpr.ctor(SharpAlg.Native.NumberFactory.One);
            SharpAlg.Native.Expr.MinusOne = new SharpAlg.Native.ConstantExpr.ctor(SharpAlg.Native.NumberFactory.MinusOne);
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
            return SharpAlg.Native.Expr.Multi(SharpAlg.Native.FunctionalExtensions.Combine$1(SharpAlg.Native.Expr.ctor, left, right), type);
        },
        Multi: function (args, type)
        {
            return System.Linq.Enumerable.Count$1$$IEnumerable$1(SharpAlg.Native.Expr.ctor, args) > 1 ? (type == 0 ? new SharpAlg.Native.AddExpr.ctor(args) : new SharpAlg.Native.MultiplyExpr.ctor(args)) : System.Linq.Enumerable.Single$1$$IEnumerable$1(SharpAlg.Native.Expr.ctor, args);
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
        Function$$String$$Expr: function (functionName, argument)
        {
            return SharpAlg.Native.Expr.Function$$String$$IEnumerable$1$Expr(functionName, SharpAlg.Native.FunctionalExtensions.AsEnumerable$1(SharpAlg.Native.Expr.ctor, argument));
        },
        Function$$String$$IEnumerable$1$Expr: function (functionName, arguments)
        {
            return new SharpAlg.Native.FunctionExpr.ctor(functionName, arguments);
        }
    },
    assemblyName: "SharpAlg.Core",
    Kind: "Class",
    definition:
    {
        ctor: function ()
        {
            System.Object.ctor.call(this);
        },
        PrintDebug: function ()
        {
            return "";
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
    assemblyName: "SharpAlg.Core",
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
    assemblyName: "SharpAlg.Core",
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
    assemblyName: "SharpAlg.Core",
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
    assemblyName: "SharpAlg.Core",
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
    assemblyName: "SharpAlg.Core",
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
    assemblyName: "SharpAlg.Core",
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
    assemblyName: "SharpAlg.Core",
    Kind: "Class",
    definition:
    {
        ctor: function (functionName, arguments)
        {
            this._FunctionName = null;
            this._Args = null;
            SharpAlg.Native.Expr.ctor.call(this);
            this.set_Args(arguments);
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
            return visitor.Function(this);
        }
    }
};
JsTypes.push(SharpAlg$Native$FunctionExpr);
if (typeof(SharpAlg) == "undefined")
    var SharpAlg = {};
if (typeof(SharpAlg.Native) == "undefined")
    SharpAlg.Native = {};
if (typeof(SharpAlg.Native.Builder) == "undefined")
    SharpAlg.Native.Builder = {};
SharpAlg.Native.Builder.ExprBuilder = function ()
{
};
SharpAlg.Native.Builder.ExprBuilder.prototype.Subtract = function (left, right)
{
    return this.Add(left, this.Minus(right));
};
SharpAlg.Native.Builder.ExprBuilder.prototype.Divide = function (left, right)
{
    return this.Multiply(left, this.Inverse(right));
};
SharpAlg.Native.Builder.ExprBuilder.prototype.Minus = function (expr)
{
    return this.Multiply(SharpAlg.Native.Expr.MinusOne, expr);
};
SharpAlg.Native.Builder.ExprBuilder.prototype.Inverse = function (expr)
{
    return this.Power(expr, SharpAlg.Native.Expr.MinusOne);
};
var SharpAlg$Native$Function =
{
    fullname: "SharpAlg.Native.Function",
    baseTypeName: "System.Object",
    assemblyName: "SharpAlg.Core",
    Kind: "Class",
    definition:
    {
        ctor: function (name)
        {
            this._Name = null;
            System.Object.ctor.call(this);
            this.set_Name(name);
        },
        Name$$: "System.String",
        get_Name: function ()
        {
            return this._Name;
        },
        set_Name: function (value)
        {
            this._Name = value;
        }
    }
};
JsTypes.push(SharpAlg$Native$Function);
var SharpAlg$Native$IDiffExpressionVisitor = {fullname: "SharpAlg.Native.IDiffExpressionVisitor", baseTypeName: "System.Object", assemblyName: "SharpAlg.Core", interfaceNames: ["SharpAlg.Native.IExpressionVisitor$1"], Kind: "Interface"};
JsTypes.push(SharpAlg$Native$IDiffExpressionVisitor);
var SharpAlg$Native$ISupportDiff = {fullname: "SharpAlg.Native.ISupportDiff", baseTypeName: "System.Object", assemblyName: "SharpAlg.Core", Kind: "Interface"};
JsTypes.push(SharpAlg$Native$ISupportDiff);
var SharpAlg$Native$ISupportCheckArgs = {fullname: "SharpAlg.Native.ISupportCheckArgs", baseTypeName: "System.Object", assemblyName: "SharpAlg.Core", Kind: "Interface"};
JsTypes.push(SharpAlg$Native$ISupportCheckArgs);
var SharpAlg$Native$IConstantFunction = {fullname: "SharpAlg.Native.IConstantFunction", baseTypeName: "System.Object", assemblyName: "SharpAlg.Core", Kind: "Interface"};
JsTypes.push(SharpAlg$Native$IConstantFunction);
var SharpAlg$Native$ISupportConvolution = {fullname: "SharpAlg.Native.ISupportConvolution", baseTypeName: "System.Object", assemblyName: "SharpAlg.Core", Kind: "Interface"};
JsTypes.push(SharpAlg$Native$ISupportConvolution);
var SharpAlg$Native$IContext = {fullname: "SharpAlg.Native.IContext", baseTypeName: "System.Object", assemblyName: "SharpAlg.Core", Kind: "Interface"};
JsTypes.push(SharpAlg$Native$IContext);
var SharpAlg$Native$IExpressionEvaluator = {fullname: "SharpAlg.Native.IExpressionEvaluator", baseTypeName: "System.Object", assemblyName: "SharpAlg.Core", interfaceNames: ["SharpAlg.Native.IExpressionVisitor$1"], Kind: "Interface"};
JsTypes.push(SharpAlg$Native$IExpressionEvaluator);
var SharpAlg$Native$IExpressionVisitor$1 = {fullname: "SharpAlg.Native.IExpressionVisitor$1", baseTypeName: "System.Object", assemblyName: "SharpAlg.Core", Kind: "Interface"};
JsTypes.push(SharpAlg$Native$IExpressionVisitor$1);
