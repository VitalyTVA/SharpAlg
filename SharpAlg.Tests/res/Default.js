/*Generated by SharpKit 5 v5.01.1000*/
function DefaultClient_Load()
{
    Compile();
};
function btnTest_click(e)
{
    RunTests();
};
function RunTests()
{
    var fixtures = [new SharpAlg.Tests.ExprTests.ctor(), new SharpAlg.Tests.DiffTests.ctor(), new SharpAlg.Tests.ParserTests.ctor(), new SharpAlg.Tests.NumberTests.ctor(), new SharpAlg.Tests.FunctionsTests.ctor()];
    var jQuery = $(document.body);
    jQuery.append("<br/>");
    var ok = 0, failed = 0;
    for (var $i2 = 0, $l2 = fixtures.length, fixture = fixtures[$i2]; $i2 < $l2; $i2++, fixture = fixtures[$i2])
    {
        var methods = fixture.GetType().GetMethods();
        for (var $i3 = 0, $l3 = methods.length, method = methods[$i3]; $i3 < $l3; $i3++, method = methods[$i3])
        {
            if (method.get_Name().EndsWith$$String("Test"))
            {
                if (RunTest(jQuery, fixture, method))
                    ok++;
                else
                    failed++;
            }
        }
    }
    jQuery.append(System.String.Format$$String$$Object$$Object$$Object("<br/>TOTAL {0}<br/>PASSED: {1}<br/>FAILED: {2}<br/>", (ok + failed), ok, failed));
};
function RunTest(jQuery, fixture, method)
{
    var status = "OK";
    var success = true;
    try
    {
        method.Invoke$$Object$$Object$Array(fixture, null);
    }
    catch (e)
    {
        status = "Failure: " + e;
        success = false;
    }
    jQuery.append(fixture.GetType().get_Name() + "." + method.get_Name() + ": " + status + "<br/>");
    return success;
};
