using System;
using FluentAssertions;
using Lab.Test.Unit.LogNotification;
using Lab.Test.Unit.LogNotification.Interfaces;
using NSubstitute;
using Xunit.Categories;

namespace Lab.Test.Unit.LogNotificationTests;

[UnitTest("LogAnalyzer")]
public class LogAnalyzerTests
{
    private readonly IWebService _fakeWebService = Substitute.For<IWebService>();

    // private readonly LogAnalyzer _sut = new LogAnalyzer();
    private readonly LogAnalyzer _sut;

    public LogAnalyzerTests()
    {
        this._sut = new LogAnalyzer(this._fakeWebService);
    }

    [Fact(DisplayName = "IsValidLogFileName_檔名為空_Throws")]
    public void IsValidLogFileName_EmptyFileName_Throws()
    {
        // arrange
        // var sut = new LogAnalyzer();

        // act
        Action act = () => this._sut.IsValidLogFileName(null);

        // assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact(DisplayName = "IsValidLogFileName_附檔名有誤_回傳 False")]
    public void IsValidLogFileName_BadExtension_ReturnFalse()
    {
        // arrange
        // var sut = new LogAnalyzer();

        // act
        var expected = this._sut.IsValidLogFileName("filewithbadextension.foo");

        // assert
        expected.Should().BeFalse();
    }

    [Theory(DisplayName = "IsValidLogFileName_附檔名正確且不區分大小寫_回傳 True")]
    [InlineData("filewithgoodextension.slf")]
    [InlineData("filewithgoodextension.SLF")]
    public void IsValidLogFileName_GoodExtensionLowerCase_ReturnTrue(
        string fileNmae)
    {
        // arrange
        // var sut = new LogAnalyzer();

        // act
        var expected = this._sut.IsValidLogFileName(fileNmae);

        // assert
        expected.Should().BeTrue();
    }

    [Theory(DisplayName = "IsValidLogFileName_記住最後一次驗證結果_與驗證結果相符")]
    [InlineData("badfile.foo", false)]
    [InlineData("goodfile.slf", true)]
    public void IsValidLogFileName_WhenCalled_ChangeWasLastFileNameValid(
        string fileNmae,
        bool expected)
    {
        // arrange
        // var sut = new LogAnalyzer();

        // act
        this._sut.IsValidLogFileName(fileNmae);

        // assert
        this._sut.WasLastFileNameValid.Should().Be(expected);
    }

    [Fact(DisplayName = "Analyze_檔名長度太短_呼叫外部服務")]
    public void Analyze_TooShortFileName_CallWebService()
    {
        // arrange

        // act
        this._sut.Analyze("1234567");

        // assert
        this._fakeWebService.Received(1).LogError(Arg.Any<string>());
    }

    [Fact(DisplayName = "Analyze_檔名長度>=8(符合規則)_不呼叫外部服務")]
    public void Analyze_ByDefault_CallWebService()
    {
        // arrange

        // act
        this._sut.Analyze("12345678");

        // assert
        this._fakeWebService.Received(0).LogError(Arg.Any<string>());
    }
}