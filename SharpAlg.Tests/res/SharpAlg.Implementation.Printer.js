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
if (typeof(SharpAlg.Native.Printer) == "undefined")
    SharpAlg.Native.Printer = {};
SharpAlg.Native.Printer.ExpressionPrinter = function (context)
{
    this.context = null;
    this.context = context;
};
SharpAlg.Native.Printer.ExpressionPrinter.IsMinusExpression = function (multi)
{
    return System.Linq.Enumerable.Count$1$$IEnumerable$1(SharpAlg.Native.Expr.ctor, multi.get_Args()) == 2 && SharpAlg.Native.ImplementationExpressionExtensions.ExprEquals(SharpAlg.Native.Expr.MinusOne, System.Linq.Enumerable.ElementAt$1(SharpAlg.Native.Expr.ctor, multi.get_Args(), 0));
};
SharpAlg.Native.Printer.ExpressionPrinter.IsInverseExpression = function (power)
{
    return SharpAlg.Native.ImplementationExpressionExtensions.ExprEquals(SharpAlg.Native.Expr.MinusOne, power.get_Right());
};
SharpAlg.Native.Printer.ExpressionPrinter.Create = function (context)
{
    return new SharpAlg.Native.Printer.ExpressionPrinter(context);
};
SharpAlg.Native.Printer.ExpressionPrinter.prototype.Constant = function (constant)
{
    return constant.get_Value().toString();
};
SharpAlg.Native.Printer.ExpressionPrinter.prototype.Add = function (multi)
{
    var sb = new System.Text.StringBuilder.ctor();
    SharpAlg.Native.FunctionalExtensions.Accumulate$1(SharpAlg.Native.Expr.ctor, multi.get_Args(), $CreateAnonymousDelegate(this, function (x)
    {
        sb.Append$$String(x.Visit$1(System.String.ctor, this));
    }), $CreateAnonymousDelegate(this, function (x)
    {
        var info = x.Visit$1(SharpAlg.Native.Printer.ExpressionPrinter.UnaryExpressionInfo, SharpAlg.Native.Printer.ExpressionPrinter.AddUnaryExpressionExtractor.AddInstance);
        sb.Append$$String(SharpAlg.Native.Printer.ExpressionPrinter.GetBinaryOperationSymbol(info.Operation));
        sb.Append$$String(this.WrapFromAdd(info.Expr));
    }));
    return sb.toString();
};
SharpAlg.Native.Printer.ExpressionPrinter.prototype.Multiply = function (multi)
{
    if (SharpAlg.Native.ImplementationExpressionExtensions.ExprEquals(System.Linq.Enumerable.First$1$$IEnumerable$1(SharpAlg.Native.Expr.ctor, multi.get_Args()), SharpAlg.Native.Expr.MinusOne))
    {
        var exprText = this.WrapFromAdd(SharpAlg.Native.CoreExpressionExtensions.Tail$$MultiplyExpr(multi));
        return System.String.Format$$String$$Object("-{0}", exprText);
    }
    var sb = new System.Text.StringBuilder.ctor();
    SharpAlg.Native.FunctionalExtensions.Accumulate$1(SharpAlg.Native.Expr.ctor, multi.get_Args(), $CreateAnonymousDelegate(this, function (x)
    {
        sb.Append$$String(this.WrapFromMultiply(x, 0));
    }), $CreateAnonymousDelegate(this, function (x)
    {
        var info = x.Visit$1(SharpAlg.Native.Printer.ExpressionPrinter.UnaryExpressionInfo, SharpAlg.Native.Printer.ExpressionPrinter.MultiplyUnaryExpressionExtractor.MultiplyInstance);
        sb.Append$$String(SharpAlg.Native.Printer.ExpressionPrinter.GetBinaryOperationSymbol(info.Operation));
        sb.Append$$String(this.WrapFromMultiply(info.Expr, 1));
    }));
    return sb.toString();
};
SharpAlg.Native.Printer.ExpressionPrinter.prototype.Power = function (power)
{
    if (SharpAlg.Native.Printer.ExpressionPrinter.IsInverseExpression(power))
    {
        return System.String.Format$$String$$Object("1 / {0}", this.WrapFromMultiply(power.get_Left(), 1));
    }
    return System.String.Format$$String$$Object$$Object("{0} ^ {1}", this.WrapFromPower(power.get_Left()), this.WrapFromPower(power.get_Right()));
};
SharpAlg.Native.Printer.ExpressionPrinter.prototype.Parameter = function (parameter)
{
    return parameter.get_ParameterName();
};
SharpAlg.Native.Printer.ExpressionPrinter.prototype.Function = function (functionExpr)
{
    if (SharpAlg.Native.Printer.ExpressionPrinter.IsFactorial(this.context, functionExpr))
        return System.String.Format$$String$$Object("{0}!", this.WrapFromFactorial(System.Linq.Enumerable.First$1$$IEnumerable$1(SharpAlg.Native.Expr.ctor, functionExpr.get_Args())));
    var sb = new System.Text.StringBuilder.ctor$$String(functionExpr.get_FunctionName());
    sb.Append$$String("(");
    SharpAlg.Native.FunctionalExtensions.Accumulate$1(SharpAlg.Native.Expr.ctor, functionExpr.get_Args(), $CreateAnonymousDelegate(this, function (x)
    {
        sb.Append$$String(x.Visit$1(System.String.ctor, this));
    }), $CreateAnonymousDelegate(this, function (x)
    {
        sb.Append$$String(", ");
        sb.Append$$String(x.Visit$1(System.String.ctor, this));
    }));
    sb.Append$$String(")");
    return sb.toString();
};
SharpAlg.Native.Printer.ExpressionPrinter.IsFactorial = function (context, functionExpr)
{
    return Is(context.GetFunction(functionExpr.get_FunctionName()), SharpAlg.Native.FactorialFunction.ctor);
};
SharpAlg.Native.Printer.ExpressionPrinter.GetBinaryOperationSymbol = function (operation)
{
    switch (operation)
    {
        case 0:
            return " + ";
        case 1:
            return " - ";
        case 2:
            return " * ";
        case 3:
            return " / ";
        default :
            throw $CreateException(new System.NotImplementedException.ctor(), new Error());
    }
};
SharpAlg.Native.Printer.ExpressionPrinter.GetPriority = function (operation)
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
SharpAlg.Native.Printer.ExpressionPrinter.prototype.WrapFromAdd = function (expr)
{
    return this.Wrap(expr, 1, 1);
};
SharpAlg.Native.Printer.ExpressionPrinter.prototype.WrapFromMultiply = function (expr, order)
{
    return this.Wrap(expr, 2, order);
};
SharpAlg.Native.Printer.ExpressionPrinter.prototype.WrapFromPower = function (expr)
{
    return this.Wrap(expr, 3, 1);
};
SharpAlg.Native.Printer.ExpressionPrinter.prototype.WrapFromFactorial = function (expr)
{
    return this.Wrap(expr, 4, 1);
};
SharpAlg.Native.Printer.ExpressionPrinter.prototype.Wrap = function (expr, currentPriority, order)
{
    var wrap = expr.Visit$1(System.Boolean.ctor, new SharpAlg.Native.Printer.ExpressionPrinter.ExpressionWrapperVisitor(this.context, currentPriority, order));
    var s = expr.Visit$1(System.String.ctor, this);
    if (wrap)
        return "(" + s + ")";
    return s;
};
if (typeof(SharpAlg.Native.Printer.ExpressionPrinter) == "undefined")
    SharpAlg.Native.Printer.ExpressionPrinter = {};
SharpAlg.Native.Printer.ExpressionPrinter.ExpressionWrapperVisitor = function (context, priority, order)
{
    this.order = 0;
    this.priority = 0;
    this.context = null;
    this.context = context;
    this.order = order;
    this.priority = priority;
};
SharpAlg.Native.Printer.ExpressionPrinter.ExpressionWrapperVisitor.prototype.Constant = function (constant)
{
    if (constant.get_Value().get_IsFraction())
        return this.ShouldWrap(3);
    if (this.order == 0)
        return false;
    return SharpAlg.Native.Number.op_LessThan(constant.get_Value(), SharpAlg.Native.NumberFactory.Zero);
};
SharpAlg.Native.Printer.ExpressionPrinter.ExpressionWrapperVisitor.prototype.Parameter = function (parameter)
{
    return false;
};
SharpAlg.Native.Printer.ExpressionPrinter.ExpressionWrapperVisitor.prototype.Add = function (multi)
{
    return this.ShouldWrap(1);
};
SharpAlg.Native.Printer.ExpressionPrinter.ExpressionWrapperVisitor.prototype.Multiply = function (multi)
{
    if (SharpAlg.Native.Printer.ExpressionPrinter.IsMinusExpression(multi))
        return true;
    return this.ShouldWrap(2);
};
SharpAlg.Native.Printer.ExpressionPrinter.ExpressionWrapperVisitor.prototype.Power = function (power)
{
    if (SharpAlg.Native.Printer.ExpressionPrinter.IsInverseExpression(power))
        return this.ShouldWrap(2);
    return this.ShouldWrap(3);
};
SharpAlg.Native.Printer.ExpressionPrinter.ExpressionWrapperVisitor.prototype.Function = function (functionExpr)
{
    if (SharpAlg.Native.Printer.ExpressionPrinter.IsFactorial(this.context, functionExpr))
        return this.ShouldWrap(4);
    return false;
};
SharpAlg.Native.Printer.ExpressionPrinter.ExpressionWrapperVisitor.prototype.ShouldWrap = function (exprPriority)
{
    return this.priority >= exprPriority;
};
SharpAlg.Native.Printer.ExpressionPrinter.UnaryExpressionExtractor = function ()
{
    SharpAlg.Native.DefaultExpressionVisitor.call(this);
};
SharpAlg.Native.Printer.ExpressionPrinter.UnaryExpressionExtractor.prototype.Constant = function (constant)
{
    return SharpAlg.Native.Number.op_GreaterThanOrEqual(constant.get_Value(), SharpAlg.Native.NumberFactory.Zero) || this.get_Operation() != 0 ? SharpAlg.Native.DefaultExpressionVisitor.prototype.Constant.call(this, constant) : new SharpAlg.Native.Printer.ExpressionPrinter.UnaryExpressionInfo(SharpAlg.Native.Expr.Constant(SharpAlg.Native.Number.op_Subtraction(SharpAlg.Native.NumberFactory.Zero, constant.get_Value())), 1);
};
SharpAlg.Native.Printer.ExpressionPrinter.UnaryExpressionExtractor.prototype.GetDefault = function (expr)
{
    return new SharpAlg.Native.Printer.ExpressionPrinter.UnaryExpressionInfo(expr, SharpAlg.Native.ExpressionEvaluator.GetBinaryOperationEx(this.get_Operation()));
};
$Inherit(SharpAlg.Native.Printer.ExpressionPrinter.UnaryExpressionExtractor, SharpAlg.Native.DefaultExpressionVisitor);
SharpAlg.Native.Printer.ExpressionPrinter.MultiplyUnaryExpressionExtractor = function ()
{
    SharpAlg.Native.Printer.ExpressionPrinter.UnaryExpressionExtractor.call(this);
};
SharpAlg.Native.Printer.ExpressionPrinter.MultiplyUnaryExpressionExtractor.MultiplyInstance = new SharpAlg.Native.Printer.ExpressionPrinter.MultiplyUnaryExpressionExtractor();
SharpAlg.Native.Printer.ExpressionPrinter.MultiplyUnaryExpressionExtractor.prototype.get_Operation = function ()
{
    return 1;
};
SharpAlg.Native.Printer.ExpressionPrinter.MultiplyUnaryExpressionExtractor.prototype.Power = function (power)
{
    if (SharpAlg.Native.Printer.ExpressionPrinter.IsInverseExpression(power))
    {
        return new SharpAlg.Native.Printer.ExpressionPrinter.UnaryExpressionInfo(power.get_Left(), 3);
    }
    return SharpAlg.Native.DefaultExpressionVisitor.prototype.Power.call(this, power);
};
$Inherit(SharpAlg.Native.Printer.ExpressionPrinter.MultiplyUnaryExpressionExtractor, SharpAlg.Native.Printer.ExpressionPrinter.UnaryExpressionExtractor);
SharpAlg.Native.Printer.ExpressionPrinter.AddUnaryExpressionExtractor = function ()
{
    SharpAlg.Native.Printer.ExpressionPrinter.UnaryExpressionExtractor.call(this);
};
SharpAlg.Native.Printer.ExpressionPrinter.AddUnaryExpressionExtractor.AddInstance = new SharpAlg.Native.Printer.ExpressionPrinter.AddUnaryExpressionExtractor();
SharpAlg.Native.Printer.ExpressionPrinter.AddUnaryExpressionExtractor.ExtractAddUnaryInfo = function (expr)
{
    return expr.Visit$1(SharpAlg.Native.Printer.ExpressionPrinter.UnaryExpressionInfo, new SharpAlg.Native.Printer.ExpressionPrinter.AddUnaryExpressionExtractor());
};
SharpAlg.Native.Printer.ExpressionPrinter.AddUnaryExpressionExtractor.prototype.get_Operation = function ()
{
    return 0;
};
SharpAlg.Native.Printer.ExpressionPrinter.AddUnaryExpressionExtractor.prototype.Multiply = function (multi)
{
    var headConstant = As(System.Linq.Enumerable.First$1$$IEnumerable$1(SharpAlg.Native.Expr.ctor, multi.get_Args()), SharpAlg.Native.ConstantExpr.ctor);
    if (SharpAlg.Native.MayBe.Return(headConstant, $CreateAnonymousDelegate(this, function (x)
    {
        return SharpAlg.Native.Number.op_LessThan(x.get_Value(), SharpAlg.Native.NumberFactory.Zero);
    }), $CreateAnonymousDelegate(this, function ()
    {
        return false;
    })))
    {
        var exprConstant = SharpAlg.Native.Expr.Constant(SharpAlg.Native.Number.op_Subtraction(SharpAlg.Native.NumberFactory.Zero, SharpAlg.Native.MayBe.With(System.Linq.Enumerable.First$1$$IEnumerable$1(SharpAlg.Native.Expr.ctor, multi.get_Args()), $CreateAnonymousDelegate(this, function (y)
        {
            return As(y, SharpAlg.Native.ConstantExpr.ctor);
        })).get_Value()));
        var expr = SharpAlg.Native.ImplementationExpressionExtensions.ExprEquals(System.Linq.Enumerable.First$1$$IEnumerable$1(SharpAlg.Native.Expr.ctor, multi.get_Args()), SharpAlg.Native.Expr.MinusOne) ? SharpAlg.Native.CoreExpressionExtensions.Tail$$MultiplyExpr(multi) : SharpAlg.Native.Expr.Multiply$$IEnumerable$1$Expr(System.Linq.Enumerable.Concat$1(SharpAlg.Native.Expr.ctor, SharpAlg.Native.FunctionalExtensions.AsEnumerable$1(SharpAlg.Native.Expr.ctor, exprConstant), SharpAlg.Native.FunctionalExtensions.Tail$1(SharpAlg.Native.Expr.ctor, multi.get_Args())));
        return new SharpAlg.Native.Printer.ExpressionPrinter.UnaryExpressionInfo(expr, 1);
    }
    return SharpAlg.Native.DefaultExpressionVisitor.prototype.Multiply.call(this, multi);
};
$Inherit(SharpAlg.Native.Printer.ExpressionPrinter.AddUnaryExpressionExtractor, SharpAlg.Native.Printer.ExpressionPrinter.UnaryExpressionExtractor);
SharpAlg.Native.Printer.ExpressionPrinter.UnaryExpressionInfo = function (expr, operation)
{
    this.Expr = null;
    this.Operation = 0;
    this.Operation = operation;
    this.Expr = expr;
};
