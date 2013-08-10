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
if (typeof(SharpAlg) == "undefined")
    var SharpAlg = {};
if (typeof(SharpAlg.Native) == "undefined")
    SharpAlg.Native = {};
if (typeof(SharpAlg.Native.Builder) == "undefined")
    SharpAlg.Native.Builder = {};
SharpAlg.Native.Builder.ConvolutionExprBuilder = function (context)
{
    this.context = null;
    SharpAlg.Native.Builder.ExprBuilder.call(this);
    this.context = context;
};
SharpAlg.Native.Builder.ConvolutionExprBuilder.prototype.get_Context = function ()
{
    return this.context;
};
SharpAlg.Native.Builder.ConvolutionExprBuilder.prototype.Function = function (functionName, args)
{
    var func = this.context.GetFunction(functionName);
    var checkArgs = SharpAlg.Native.MayBe.With(SharpAlg.Native.FunctionalExtensions.Convert$1(SharpAlg.Native.ISupportCheckArgs.ctor, func), $CreateAnonymousDelegate(this, function (x)
    {
        return x.Check(args);
    }));
    if (!System.String.IsNullOrEmpty(checkArgs))
        throw $CreateException(new SharpAlg.Native.InvalidArgumentCountException.ctor$$String(checkArgs), new Error());
    return SharpAlg.Native.MayBe.Return(SharpAlg.Native.MayBe.With(SharpAlg.Native.FunctionalExtensions.Convert$1(SharpAlg.Native.ISupportConvolution.ctor, func), $CreateAnonymousDelegate(this, function (x)
    {
        return x.Convolute(this.get_Context(), args);
    })), $CreateAnonymousDelegate(this, function (x)
    {
        return x;
    }), $CreateAnonymousDelegate(this, function ()
    {
        return SharpAlg.Native.Expr.Function$$String$$IEnumerable$1$Expr(functionName, args);
    }));
};
SharpAlg.Native.Builder.ConvolutionExprBuilder.prototype.Parameter = function (parameterName)
{
    return (this.context.GetValue(parameterName) != null ? this.context.GetValue(parameterName) : SharpAlg.Native.Expr.Parameter(parameterName));
};
SharpAlg.Native.Builder.ConvolutionExprBuilder.prototype.Add = function (left, right)
{
    return this.Binary(left, right, 0);
};
SharpAlg.Native.Builder.ConvolutionExprBuilder.prototype.Multiply = function (left, right)
{
    return this.Binary(left, right, 1);
};
SharpAlg.Native.Builder.ConvolutionExprBuilder.prototype.Power = function (left, right)
{
    return (this.ConstantPowerConvolution(left, right) != null ? this.ConstantPowerConvolution(left, right) : (this.ExpressionPowerConvolution(left, right) != null ? this.ExpressionPowerConvolution(left, right) : SharpAlg.Native.Expr.Power(left, right)));
};
SharpAlg.Native.Builder.ConvolutionExprBuilder.prototype.Binary = function (left, right, operation)
{
    return (this.MultiConvolution(left, right, operation) != null ? this.MultiConvolution(left, right, operation) : SharpAlg.Native.Expr.Binary(left, right, operation));
};
SharpAlg.Native.Builder.ConvolutionExprBuilder.prototype.GetOpenParensArgs = function (left, right, operation)
{
    if (operation == 1)
    {
        var rightConst = SharpAlg.Native.Builder.ConvolutionExprBuilder.GetConstValue(right);
        if (SharpAlg.Native.Number.op_Inequality(rightConst, null))
        {
            var tmp = left;
            left = right;
            right = tmp;
        }
        var leftConst = SharpAlg.Native.Builder.ConvolutionExprBuilder.GetConstValue(left);
        var rightAddExpr = As(right, SharpAlg.Native.AddExpr.ctor);
        if (SharpAlg.Native.Number.op_Inequality(leftConst, null) && rightAddExpr != null)
        {
            return System.Linq.Enumerable.Select$2$$IEnumerable$1$$Func$2(SharpAlg.Native.Expr.ctor, SharpAlg.Native.Expr.ctor, rightAddExpr.get_Args(), $CreateAnonymousDelegate(this, function (x)
            {
                return this.Multiply(SharpAlg.Native.Expr.Constant(leftConst), x);
            }));
        }
    }
    return null;
};
SharpAlg.Native.Builder.ConvolutionExprBuilder.prototype.MultiConvolution = function (left, right, operation)
{
    var openParensArgs = this.GetOpenParensArgs(left, right, operation);
    if (openParensArgs != null)
    {
        operation = 0;
    }
    var args = (openParensArgs != null ? openParensArgs : this.GetSortedArgs(left, right, operation));
    return this.MultiConvolutionCore(args, operation);
};
SharpAlg.Native.Builder.ConvolutionExprBuilder.prototype.MultiConvolutionCore = function (args, operation)
{
    var argsList = System.Linq.Enumerable.ToList$1(SharpAlg.Native.Expr.ctor, args);
    for (var i = 0; i < argsList.get_Count(); i++)
    {
        var convolutionOccured = false;
        for (var j = i + 1; j < argsList.get_Count(); j++)
        {
            var convoluted = (this.ConstantConvolution(argsList.get_Item$$Int32(i), argsList.get_Item$$Int32(j), operation) != null ? this.ConstantConvolution(argsList.get_Item$$Int32(i), argsList.get_Item$$Int32(j), operation) : (this.PowerConvolution(argsList.get_Item$$Int32(i), argsList.get_Item$$Int32(j), operation) != null ? this.PowerConvolution(argsList.get_Item$$Int32(i), argsList.get_Item$$Int32(j), operation) : this.MultiplyConvolution(argsList.get_Item$$Int32(i), argsList.get_Item$$Int32(j), operation)));
            if (convoluted != null)
            {
                argsList.set_Item$$Int32(i, convoluted);
                argsList.RemoveAt(j);
                j--;
                convolutionOccured = true;
            }
        }
        if (convolutionOccured)
            i--;
    }
    return SharpAlg.Native.Expr.Multi(argsList, operation);
};
SharpAlg.Native.Builder.ConvolutionExprBuilder.prototype.GetSortedArgs = function (left, right, operation)
{
    var args = System.Linq.Enumerable.Concat$1(SharpAlg.Native.Expr.ctor, this.GetArgs(left, operation), this.GetArgs(right, operation));
    if (operation == 1)
    {
        args = System.Linq.Enumerable.Concat$1(SharpAlg.Native.Expr.ctor, System.Linq.Enumerable.Where$1$$IEnumerable$1$$Func$2(SharpAlg.Native.Expr.ctor, args, $CreateAnonymousDelegate(this, function (x)
        {
            return Is(x, SharpAlg.Native.ConstantExpr.ctor);
        })), System.Linq.Enumerable.Where$1$$IEnumerable$1$$Func$2(SharpAlg.Native.Expr.ctor, args, $CreateAnonymousDelegate(this, function (x)
        {
            return !(Is(x, SharpAlg.Native.ConstantExpr.ctor));
        })));
    }
    return args;
};
SharpAlg.Native.Builder.ConvolutionExprBuilder.prototype.GetArgs = function (expr, operation)
{
    return SharpAlg.Native.Builder.ConvolutionExprBuilder.ExpressionArgumentsExtractor.ExtractArguments(expr, operation);
};
SharpAlg.Native.Builder.ConvolutionExprBuilder.prototype.ConstantConvolution = function (left, right, operation)
{
    var leftConst = SharpAlg.Native.Builder.ConvolutionExprBuilder.GetConstValue(left);
    if (SharpAlg.Native.Number.op_Equality(leftConst, SharpAlg.Native.Number.Zero))
    {
        if (operation == 0)
            return right;
        if (operation == 1)
            return SharpAlg.Native.Expr.Zero;
    }
    if (SharpAlg.Native.Number.op_Equality(leftConst, SharpAlg.Native.Number.One))
    {
        if (operation == 1)
            return right;
    }
    var rightConst = SharpAlg.Native.Builder.ConvolutionExprBuilder.GetConstValue(right);
    if (SharpAlg.Native.Number.op_Equality(rightConst, SharpAlg.Native.Number.Zero))
    {
        if (operation == 0)
            return left;
        if (operation == 1)
            return SharpAlg.Native.Expr.Zero;
    }
    if (SharpAlg.Native.Number.op_Equality(rightConst, SharpAlg.Native.Number.One))
    {
        if (operation == 1)
            return left;
    }
    if (SharpAlg.Native.Number.op_Inequality(rightConst, null) && SharpAlg.Native.Number.op_Inequality(leftConst, null))
    {
        return SharpAlg.Native.Expr.Constant(SharpAlg.Native.ExpressionEvaluator.GetBinaryOperationEvaluator(operation)(leftConst, rightConst));
    }
    return null;
};
SharpAlg.Native.Builder.ConvolutionExprBuilder.prototype.ConstantPowerConvolution = function (left, right)
{
    var leftConst = SharpAlg.Native.Builder.ConvolutionExprBuilder.GetConstValue(left);
    if (SharpAlg.Native.Number.op_Equality(leftConst, SharpAlg.Native.Number.Zero))
    {
        return SharpAlg.Native.Expr.Zero;
    }
    if (SharpAlg.Native.Number.op_Equality(leftConst, SharpAlg.Native.Number.One))
    {
        return SharpAlg.Native.Expr.One;
    }
    var rightConst = SharpAlg.Native.Builder.ConvolutionExprBuilder.GetConstValue(right);
    if (SharpAlg.Native.Number.op_Equality(rightConst, SharpAlg.Native.Number.Zero))
    {
        return SharpAlg.Native.Expr.One;
    }
    if (SharpAlg.Native.Number.op_Equality(rightConst, SharpAlg.Native.Number.One))
    {
        return left;
    }
    if (SharpAlg.Native.Number.op_Inequality(rightConst, null) && SharpAlg.Native.Number.op_Inequality(leftConst, null))
        return SharpAlg.Native.Expr.Constant(SharpAlg.Native.Number.op_ExclusiveOr(leftConst, rightConst));
    return null;
};
SharpAlg.Native.Builder.ConvolutionExprBuilder.prototype.ExpressionPowerConvolution = function (left, right)
{
    var rightConst = SharpAlg.Native.Builder.ConvolutionExprBuilder.GetConstValue(right);
    if (SharpAlg.Native.Number.op_Inequality(rightConst, null))
    {
        var leftMultiplyExpr = As(left, SharpAlg.Native.MultiplyExpr.ctor);
        if (leftMultiplyExpr != null)
        {
            return SharpAlg.Native.Expr.Multiply$$IEnumerable$1$Expr(System.Linq.Enumerable.Select$2$$IEnumerable$1$$Func$2(SharpAlg.Native.Expr.ctor, SharpAlg.Native.Expr.ctor, leftMultiplyExpr.get_Args(), $CreateAnonymousDelegate(this, function (x)
            {
                return this.Power(x, SharpAlg.Native.Expr.Constant(rightConst));
            })));
        }
        var power = SharpAlg.Native.Builder.ConvolutionExprBuilder.PowerExpressionExtractor.ExtractPower(left);
        var leftConst = SharpAlg.Native.Builder.ConvolutionExprBuilder.GetConstValue(power.get_Right());
        if (SharpAlg.Native.Number.op_Inequality(leftConst, null))
            return SharpAlg.Native.Expr.Power(power.get_Left(), SharpAlg.Native.Expr.Constant(SharpAlg.Native.Number.op_Multiply(rightConst, leftConst)));
    }
    return null;
};
SharpAlg.Native.Builder.ConvolutionExprBuilder.prototype.PowerConvolution = function (left, right, operation)
{
    if (operation == 1)
    {
        var leftPower = SharpAlg.Native.Builder.ConvolutionExprBuilder.PowerExpressionExtractor.ExtractPower(left);
        var rightPower = SharpAlg.Native.Builder.ConvolutionExprBuilder.PowerExpressionExtractor.ExtractPower(right);
        if (SharpAlg.Native.ImplementationExpressionExtensions.ExprEquivalent(leftPower.get_Left(), rightPower.get_Left()))
        {
            return this.Power(leftPower.get_Left(), this.Add(leftPower.get_Right(), rightPower.get_Right()));
        }
    }
    return null;
};
SharpAlg.Native.Builder.ConvolutionExprBuilder.prototype.MultiplyConvolution = function (left, right, operation)
{
    if (operation == 0)
    {
        var leftMultiply = SharpAlg.Native.Builder.ConvolutionExprBuilder.MultiplyExpressionExtractor.ExtractMultiply(left);
        var rightMultiply = SharpAlg.Native.Builder.ConvolutionExprBuilder.MultiplyExpressionExtractor.ExtractMultiply(right);
        if (SharpAlg.Native.ImplementationExpressionExtensions.ExprEquivalent(leftMultiply.get_Item2(), rightMultiply.get_Item2()))
        {
            return this.Multiply(this.Add(leftMultiply.get_Item1(), rightMultiply.get_Item1()), leftMultiply.get_Item2());
        }
    }
    return null;
};
SharpAlg.Native.Builder.ConvolutionExprBuilder.GetConstValue = function (expr)
{
    return SharpAlg.Native.MayBe.With((As(expr, SharpAlg.Native.ConstantExpr.ctor)), function (x)
    {
        return x.get_Value();
    });
};
$Inherit(SharpAlg.Native.Builder.ConvolutionExprBuilder, SharpAlg.Native.Builder.ExprBuilder);
if (typeof(SharpAlg.Native.Builder.ConvolutionExprBuilder) == "undefined")
    SharpAlg.Native.Builder.ConvolutionExprBuilder = {};
SharpAlg.Native.Builder.ConvolutionExprBuilder.ExpressionArgumentsExtractor = function (operation)
{
    this.operation = 0;
    SharpAlg.Native.DefaultExpressionVisitor.call(this);
    this.operation = operation;
};
SharpAlg.Native.Builder.ConvolutionExprBuilder.ExpressionArgumentsExtractor.ExtractArguments = function (expr, operation)
{
    return expr.Visit$1(System.Collections.Generic.IEnumerable$1.ctor, new SharpAlg.Native.Builder.ConvolutionExprBuilder.ExpressionArgumentsExtractor(operation));
};
SharpAlg.Native.Builder.ConvolutionExprBuilder.ExpressionArgumentsExtractor.prototype.Add = function (multi)
{
    if (this.operation == 0)
        return multi.get_Args();
    return SharpAlg.Native.DefaultExpressionVisitor.prototype.Add.call(this, multi);
};
SharpAlg.Native.Builder.ConvolutionExprBuilder.ExpressionArgumentsExtractor.prototype.Multiply = function (multi)
{
    if (this.operation == 1)
        return multi.get_Args();
    return SharpAlg.Native.DefaultExpressionVisitor.prototype.Multiply.call(this, multi);
};
SharpAlg.Native.Builder.ConvolutionExprBuilder.ExpressionArgumentsExtractor.prototype.GetDefault = function (expr)
{
    return SharpAlg.Native.FunctionalExtensions.AsEnumerable$1(SharpAlg.Native.Expr.ctor, expr);
};
$Inherit(SharpAlg.Native.Builder.ConvolutionExprBuilder.ExpressionArgumentsExtractor, SharpAlg.Native.DefaultExpressionVisitor);
SharpAlg.Native.Builder.ConvolutionExprBuilder.MultiplyExpressionExtractor = function ()
{
    SharpAlg.Native.DefaultExpressionVisitor.call(this);
};
SharpAlg.Native.Builder.ConvolutionExprBuilder.MultiplyExpressionExtractor.Instance = new SharpAlg.Native.Builder.ConvolutionExprBuilder.MultiplyExpressionExtractor();
SharpAlg.Native.Builder.ConvolutionExprBuilder.MultiplyExpressionExtractor.ExtractMultiply = function (expr)
{
    return expr.Visit$1(System.Tuple$2.ctor, SharpAlg.Native.Builder.ConvolutionExprBuilder.MultiplyExpressionExtractor.Instance);
};
SharpAlg.Native.Builder.ConvolutionExprBuilder.MultiplyExpressionExtractor.prototype.Multiply = function (multi)
{
    if (Is(System.Linq.Enumerable.First$1$$IEnumerable$1(SharpAlg.Native.Expr.ctor, multi.get_Args()), SharpAlg.Native.ConstantExpr.ctor))
        return new System.Tuple$2.ctor(SharpAlg.Native.Expr.ctor, SharpAlg.Native.Expr.ctor, System.Linq.Enumerable.First$1$$IEnumerable$1(SharpAlg.Native.Expr.ctor, multi.get_Args()), SharpAlg.Native.CoreExpressionExtensions.Tail$$MultiplyExpr(multi));
    return SharpAlg.Native.DefaultExpressionVisitor.prototype.Multiply.call(this, multi);
};
SharpAlg.Native.Builder.ConvolutionExprBuilder.MultiplyExpressionExtractor.prototype.GetDefault = function (expr)
{
    return new System.Tuple$2.ctor(SharpAlg.Native.Expr.ctor, SharpAlg.Native.Expr.ctor, SharpAlg.Native.Expr.One, expr);
};
$Inherit(SharpAlg.Native.Builder.ConvolutionExprBuilder.MultiplyExpressionExtractor, SharpAlg.Native.DefaultExpressionVisitor);
SharpAlg.Native.Builder.ConvolutionExprBuilder.PowerExpressionExtractor = function ()
{
    SharpAlg.Native.DefaultExpressionVisitor.call(this);
};
SharpAlg.Native.Builder.ConvolutionExprBuilder.PowerExpressionExtractor.Instance = new SharpAlg.Native.Builder.ConvolutionExprBuilder.PowerExpressionExtractor();
SharpAlg.Native.Builder.ConvolutionExprBuilder.PowerExpressionExtractor.ExtractPower = function (expr)
{
    return expr.Visit$1(SharpAlg.Native.PowerExpr.ctor, SharpAlg.Native.Builder.ConvolutionExprBuilder.PowerExpressionExtractor.Instance);
};
SharpAlg.Native.Builder.ConvolutionExprBuilder.PowerExpressionExtractor.prototype.Power = function (power)
{
    return power;
};
SharpAlg.Native.Builder.ConvolutionExprBuilder.PowerExpressionExtractor.prototype.GetDefault = function (expr)
{
    return SharpAlg.Native.Expr.Power(expr, SharpAlg.Native.Expr.One);
};
$Inherit(SharpAlg.Native.Builder.ConvolutionExprBuilder.PowerExpressionExtractor, SharpAlg.Native.DefaultExpressionVisitor);
SharpAlg.Native.Builder.TrivialExprBuilder = function (context)
{
    this.context = null;
    SharpAlg.Native.Builder.ExprBuilder.call(this);
    this.context = context;
};
SharpAlg.Native.Builder.TrivialExprBuilder.prototype.get_Context = function ()
{
    return this.context;
};
SharpAlg.Native.Builder.TrivialExprBuilder.prototype.Parameter = function (parameterName)
{
    return SharpAlg.Native.Expr.Parameter(parameterName);
};
SharpAlg.Native.Builder.TrivialExprBuilder.prototype.Add = function (left, right)
{
    return SharpAlg.Native.Expr.Add$$Expr$$Expr(left, right);
};
SharpAlg.Native.Builder.TrivialExprBuilder.prototype.Multiply = function (left, right)
{
    return SharpAlg.Native.Expr.Multiply$$Expr$$Expr(left, right);
};
SharpAlg.Native.Builder.TrivialExprBuilder.prototype.Power = function (left, right)
{
    return SharpAlg.Native.Expr.Power(left, right);
};
SharpAlg.Native.Builder.TrivialExprBuilder.prototype.Function = function (functionName, arguments)
{
    return SharpAlg.Native.Expr.Function$$String$$IEnumerable$1$Expr(functionName, arguments);
};
$Inherit(SharpAlg.Native.Builder.TrivialExprBuilder, SharpAlg.Native.Builder.ExprBuilder);
