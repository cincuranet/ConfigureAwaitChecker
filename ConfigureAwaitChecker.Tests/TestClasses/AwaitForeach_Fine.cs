﻿using System.Threading.Tasks;
using ConfigureAwaitChecker.Lib;
using ConfigureAwaitChecker.Tests;

[CheckerTests.ExpectedResult(CheckerProblem.NoProblem)]
public class AwaitForEach_Fine
{
	public async Task FooBar()
	{
		await foreach (var item in TestsBase.AsyncEnumerable().ConfigureAwait(false))
		{ }
	}
}
