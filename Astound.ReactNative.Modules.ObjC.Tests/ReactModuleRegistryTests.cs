using System;
using System.Reflection;

namespace Astound.ReactNative.Modules.ObjC.Tests;

[TestClass]
public class ReactModuleRegistryTests
{
    #region Constants

    const string TypePrefix = "Astound_ReactNative_Modules_ObjC_Tests_ReactModuleRegistryTests_";

    #endregion

    #region Nested Types

    [ReactModule]
    public class MockModuleA
    {

    }

    [ReactModule]
    public abstract class MockModuleBase
    {

    }

    public class MockModuleB : MockModuleBase
    {

    }

    public class OtherMockClass
    {

    }

    #endregion

    readonly IReactFunctions _reactFunctions = Substitute.For<IReactFunctions>();
    readonly ReactModuleRegistry _registry;

    public ReactModuleRegistryTests()
    {
        _registry = new ReactModuleRegistry(_reactFunctions);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestConstructorThrowsOnNullReactFunctions()
    {
        new ReactModuleRegistry(null);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestRegisterThrowsOnNullType()
    {
        _registry.Register(default(Type));
    }

    [TestMethod]
    public void TestRegisterType()
    {
        var type = typeof(MockModuleA);

        _registry.Register(typeof(MockModuleA));

        _reactFunctions.Received().RegisterModule(TypePrefix + type.Name);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestRegisterThrowsOnNullAssembly()
    {
        _registry.Register(default(Assembly));
    }

    [TestMethod]
    public void TestRegisterAssembly()
    {
        _registry.Register(Assembly.GetExecutingAssembly());

        _reactFunctions.Received().RegisterModule(TypePrefix + typeof(MockModuleA).Name);
        _reactFunctions.Received().RegisterModule(TypePrefix + typeof(MockModuleB).Name);
        _reactFunctions.DidNotReceive().RegisterModule(TypePrefix + typeof(MockModuleBase).Name);
        _reactFunctions.DidNotReceive().RegisterModule(TypePrefix + typeof(OtherMockClass).Name);
    }
}
