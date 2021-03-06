﻿using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.MissingConfigureAwaitFalse)]
[CodeFixTests.TestThis]
public class AwaitUsingWithVar_Missing
{
	public async Task FooBar()
	{
		await using (var _ = TestsBase.AsyncDisposable())
		{ }
	}
}
